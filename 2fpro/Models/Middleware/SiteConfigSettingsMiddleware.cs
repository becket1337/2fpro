using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using _2fpro.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using _2fpro.Data;
using _2fpro.Service.Repository;
using Microsoft.EntityFrameworkCore;
using _2fpro.Service.Interface;

namespace _2fpro.Models.Middleware
{
    public class SiteConfigSettingsMiddleware
    {
        private RequestDelegate _next;
        private SiteConfig _sconf;
        private string _cid => Guid.NewGuid().ToString();
        string path;
        public SiteConfigSettingsMiddleware(RequestDelegate next, SiteConfig sconf)
        {
            _next = next;
            _sconf = sconf;
        }

        public async Task InvokeAsync(HttpContext context, SignInManager<ApplicationUser> _signInManager,
        UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> roleManager, IDistributedCache _cache, IDiagnosticService diagService, IConfigRepository _conf)
        {
            //context.Response.StatusCode = 200;
            //context.Response.ContentType = "text/html;charset=utf-8";
            // context.Response.WriteAsync(context.Request.Path.Value).Wait();
            path = context.Request.Path.ToString();
            context.Response.Headers["X-XN"] = typeof(RuntimeEnvironment).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            context.Response.Headers["X-AspNetCore-Mvc"] = "2.2";
            var conf = await _conf.Options();
            if (conf.SelectedIsOnlineID)
            { // тех обслуживание

                var str = context.User.IsInRole("Admin");
                var path = context.Request.Path;
                //string[] allowPath = { "admin"}
                if (!str && !path.Value.ToLower().Contains("admin"))
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($@"<p style=""padding-top: 50px;text-align:center;font-size: 21px;color: black;font-weight: bold;"">Сайт на тех обслуживании</p>");
                }
                else { await _next(context); }

            }


            //проверка на админа в  панели администр.
            //var cid = Guid.NewGuid().ToString();
            //var sessionId = context.Request.Cookies["XN_SessionId"];
            //var cachedId =sessionId==null?null: await _cache.GetAsync<SessionStorage>(EncryptHelper.Decrypt(sessionId));
            var urlpath = context.Request.Path.Value;
            //context.Response.Redirect("/Identity/Account/AdminLogin");
            //if (!context.User.IsInRole("Admin") && urlpath.ToLower().StartsWith("/admin"))
            //{
            //    context.Response.Redirect("/Identity/Account/AdminLogin");
            //}

            //MAIN COOKIE

            if (context.Request.Cookies["_xn_cul"] == null)
            {
                context.Response.Cookies.Append("_xn_cul", CultureInfo.CurrentCulture.TwoLetterISOLanguageName, new CookieOptions { Expires = DateTime.Now.AddDays(7) });

            }
            if (context.Request.Cookies["_xn_ie"] == null)
            {
                context.Response.Cookies.Append("_xn_ie", (context.Request.UserAgent().StartsWith("internet") ? "1" : "0"), new CookieOptions { Expires = DateTime.Now.AddDays(7) });

            }
            if (context.Request.Cookies["_xn_ismob"] == null)
            {
                context.Response.Cookies.Append("_xn_ismob", (context.Request.IsMobileBrowser() ? "1" : "0"), new CookieOptions { Expires = DateTime.Now.AddDays(6) });

            }
            //await AdminLogin(context);
            // обнуляем identity на путь - /Identity/Account/AdminLogin

            //обновляем онлайн статус админа           

            await CreateRoles(roleManager, _userManager, context, diagService, _cache);
            await RobotsVisit(context, _cache);
            await AnonymousAuthorization(context, _userManager, _signInManager, roleManager, _cache, diagService);

            ResetCookie(context);

            //END BLOCK - COOKIE
            if (context.Request.Path == "/Identity/Account/AdminLogin" && context.Request.Method == "GET" && context.User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                context.Response.Redirect(context.Request.Path);
            }

            await _next(context);


        }
        public void ResetCookie(HttpContext context)
        {


            //context.Response.Cookies.Delete("Mozilla%2F5.0%20%28Windows%20NT%206.3%3B%20Win64%3B%20x64%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F71.0.3578.98%20Safari%2F537.36", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //context.Response.Cookies.Delete("ismob",new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //context.Response.Cookies.Delete("_xn_life", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //    context.Response.Cookies.Delete("_xn_life", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //    context.Response.Cookies.Delete("_xn_lv", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //    context.Response.Cookies.Delete("_xn_tmins", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //    context.Response.Cookies.Delete("_XN_CID", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
            //    context.Response.Cookies.Delete("_xn_t", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });


        }

        private async Task RobotsVisit(HttpContext ctx, IDistributedCache _cache)
        {
            // Записываем дату захода поисковых роботов яндекс и гугл
            if (!ctx.User.Identity.IsAuthenticated && !ctx.Request.IsLocal())
            {
                var ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();
                IPGeoLocation model = await IPGeoLocation.QueryGeographicalLocationAsync(ip);
                if (model.Org != null)
                {
                    if (model.Org.Contains("Yandex") || model.Org.Contains("yandex"))
                    {// запись даты повления поискового робота в журнал

                        await _cache.SetStringAsync("yandex", DateTime.Now.ToString());
                    }
                    else if (model.Org.Contains("Google") || model.Org.Contains("google"))
                    {
                        await _cache.SetStringAsync("google", DateTime.Now.ToString());
                    }
                }
                //await diagService.DiagUpdate(new Diagnostic { IsActivate = true, ConnectionID = "", Location = model.CountryName + ", " + model.City, CurrDateTime = DateTime.Now, CustomStringField = username, CustomBoolField = isAdmin, IpAddress = model.IP, City = model.City, Country = model.CountryName, CountryCode = model.CountryCode, IsMobileConn = model.isMobile, Latitude = model.Latitude, Longitude = model.Longitude, AsNumberAndName = model.As, OrganizationName = model.Org, Timezone = model.TimeZone, RegionName = model.RegionName, RegionCode = model.RegionCode, Status = model.Status, Zip = model.ZipCode, ISPName = model.Isp });
            }
        }

        private async Task GeoLocalization(HttpContext ctx, IDistributedCache _cache, IDiagnosticService diagService, string username, bool isAdmin)
        {

            var ip = "";
            //сохраняем геолокацию под нового пользователя
            var diag = await diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == username);
            if (diag == null)
            {
                if (string.IsNullOrEmpty(ip))
                {
                    ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }

                IPGeoLocation model = await IPGeoLocation.QueryGeographicalLocationAsync(ip);
                await diagService.DiagUpdate(new Diagnostic { IsActivate = true, ConnectionID = "", Location = model.CountryName + ", " + model.City, CurrDateTime = DateTime.Now, CustomStringField = username, CustomBoolField = isAdmin, IpAddress = model.IP, City = model.City, Country = model.CountryName, CountryCode = model.CountryCode, IsMobileConn = model.isMobile, Latitude = model.Latitude, Longitude = model.Longitude, AsNumberAndName = model.As, OrganizationName = model.Org, Timezone = model.TimeZone, RegionName = model.RegionName, RegionCode = model.RegionCode, Status = model.Status, Zip = model.ZipCode, ISPName = model.Isp });
            }
            else
            {
                await diagService.EditConnId(diag);
            }
            // создаем сессию для нового пользователя
            var expiredTimeCache = await _cache.GetAsync<SessionStorage>(username);

            if (expiredTimeCache == null)
            {
                await _cache.SetAsync<SessionStorage>(username, new SessionStorage() { Cart = new Cart() });
            }
        }


        public async Task AnonymousAuthorization(HttpContext ctx, UserManager<ApplicationUser>
                userManager, SignInManager<ApplicationUser>
                signIn, RoleManager<IdentityRole>
                roleManager,
                IDistributedCache _cache, IDiagnosticService diagService)
        {

            //await ctx.Response.WriteAsync(ctx.Request.Path.ToString()+", user - "+ctx.User.Identity.Name);
            //приемлемые пути
            string[] segments = new string[] { "/prod", "/pages", "/products", "/ProdList" };
            if (segments.Any(x => ctx.Request.Path.ToString().StartsWith(x)) || ctx.Request.Path.ToString() == "/")//!segments.Any(x => ctx.Request.Path.StartsWithSegments(x)))
            {
                //Удаление куков после удаление пользователя из базы
                if (ctx.User.Identity.IsAuthenticated)
                {

                    var resetUser = await userManager.FindByEmailAsync(ctx.User.Identity.Name);
                    if (resetUser == null)
                    {
                        await signIn.SignOutAsync();
                    }
                }

                if (!ctx.User.IsInRole("Admin") && !ctx.User.Identity.IsAuthenticated) // если пользователь не авторизован
                {
                    IdentityResult roleResult;
                    var roleExist = await roleManager.RoleExistsAsync("Anonym");
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole("Anonym"));
                    }
                    //var list = diagService.Diagnostics.Where(x => (DateTime.Now - x.CurrDateTime).TotalDays > 180);
                    //if (await list.AnyAsync()) await diagService.DiagRangeRemove(list);

                    // Проверка гелокации по ip в базе, если находим авторизуем
                    var ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    Diagnostic findByIp = await diagService.Diagnostics.FirstOrDefaultAsync(x => x.IpAddress == ip);

                    ApplicationUser hasIdentity = null;
                    // Находим пользователя по привязке геолокации к имени
                    hasIdentity = findByIp != null ? await userManager.FindByEmailAsync(findByIp.CustomStringField) : null;
                    //Если не админ авторизуем по ip, если нашли админа по ip не авторизуем, а создаем для него анонима( админ авторизовывается через панель /admin)
                    bool isAdmin = hasIdentity != null ? await userManager.IsInRoleAsync(hasIdentity, "Admin") : true;
                    if (findByIp != null && !isAdmin)
                    {

                        await signIn.SignInAsync(hasIdentity, true);
                        // обновляем дату захода на сайт
                        await diagService.UpdateCurrDateTime(findByIp.ID);
                        var expiredTimeCache = await _cache.GetAsync<SessionStorage>(hasIdentity.UserName);

                        if (expiredTimeCache == null)
                        {
                            await _cache.SetAsync<SessionStorage>(hasIdentity.UserName, new SessionStorage() { Cart = new Cart() });
                        }

                    }
                    else //Создаем нового анонима
                    {
                        var cid = _cid;

                        //await ctx.SignOutAsync(IdentityConstants.ExternalScheme);
                        var user = await userManager.FindByEmailAsync(cid + "@email.com");
                        var pass = "anonym@123321";
                        var str = ctx.Request.Path;

                        //создаем уже с правами анонимного пользователя
                        user = new ApplicationUser { UserName = cid + "@email.ru", Email = cid + "@email.ru", ResetUser = 1 };
                        var result = await userManager.CreateAsync(user, pass);
                        if (result.Succeeded)
                        {
                            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                            await userManager.ConfirmEmailAsync(user, code);
                        }
                        await userManager.AddToRoleAsync(user, "Anonym");
                        await signIn.SignInAsync(user, true);
                        // проверяем сессию

                        await _cache.SetAsync("lastLogged", user.UserName);


                        // привязываем геолокализацию для пользователя и создаем сессию на пол года под него
                        await GeoLocalization(ctx, _cache, diagService, cid + "@email.ru", false);
                        //ctx.Response.Redirect(ctx.Request.Path);
                        //if (ctx.User.Identity.IsAuthenticated && !ctx.User.IsInRole("Admin")) //если авторизованный пользователь, проверяем привязку геолокации
                        //{
                        //    var getUserDiag = await diagService.GetAsync(ctx.User.Identity.Name);

                        //    if (getUserDiag == null)
                        //        await GeoLocalization(ctx, _cache, diagService, ctx.User.Identity.Name, false);
                        //}
                    }
                }
                else//авторазиванный пользователь зашел на сайт
                {

                    //Проверяем дефолтную сессию 
                    var defaultUser = await _cache.GetAsync("lastLogged");
                    if (defaultUser == null) await _cache.SetAsync("lastLogged", ctx.User.Identity.Name);
                    // проверяем сессию пользователя
                    var expiredTimeCache = await _cache.GetAsync<SessionStorage>(ctx.User.Identity.Name);
                    //обновляем дату захода на сайта 
                    var diag = await diagService.GetAsync(ctx.User.Identity.Name);
                    //проверяем геолокацию пользователя
                    if (diag == null) await GeoLocalization(ctx, _cache, diagService, ctx.User.Identity.Name, false);

                    await diagService.UpdateCurrDateTime(diag.ID);
                    if (expiredTimeCache == null)
                    {
                        await _cache.SetAsync<SessionStorage>(ctx.User.Identity.Name, new SessionStorage() { Cart = new Cart() });
                    }
                }
            }


        }


        private async Task UpdateAdminOnlineStatus(IDiagnosticService diagnosticService)
        {
            var admDiag = await diagnosticService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
            await diagnosticService.EditAdminOnlineStatus(admDiag);
        }
        private static readonly Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public bool IsMobileBrowser(HttpContext request)
        {
            var userAgent = request.Request.Headers["User-Agent"].ToString();
            if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))) || (userAgent.ToLower().Contains("iphone;") || userAgent.ToLower().Contains("ipad;")))
            {
                return true;
            }

            return false;
        }

        //public async Task AdminLogin(HttpContext ctx)
        //{
        //    if (ctx.Request.Path.StartsWithSegments("/adminpanel") && ctx.User.IsInRole("Anonym")) { 
        //    await ctx.SignOutAsync();
        //    ctx.Response.Redirect("/Identity/Account/AdminLogin");
        //}
        //}


        private async Task CreateRoles(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> UserManager, HttpContext ctx, IDiagnosticService diagnosticService, IDistributedCache _cache)
        {
            //initializing custom roles   
            //string[] segments = new string[] { "/js","/css","/lib","/Admin","/Identity","/identity","/admin","/Content" };
            if (path == "/")//!segments.Any(x => ctx.Request.Path.StartsWithSegments(x)))
            {
                string[] roleNames = { "Admin", "Customer", "User", "Manager", "Anonym" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                ApplicationUser user = await UserManager.FindByEmailAsync("admin@mail.com");

                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = "admin@mail.com",
                        Email = "admin@mail.com",
                        ResetUser = 1

                    };
                    await UserManager.CreateAsync(user, "admin@123222");
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    await UserManager.ConfirmEmailAsync(user, token);
                }
                await UserManager.AddToRoleAsync(user, "Admin");
                var getUserDiag = await diagnosticService.GetAsync(user.UserName);
                if (getUserDiag == null)
                    await GeoLocalization(ctx, _cache, diagnosticService, user.UserName, true);

                ApplicationUser user1 = await UserManager.FindByEmailAsync("tester@gmail.com");

                if (user1 == null)
                {
                    user1 = new ApplicationUser()
                    {
                        UserName = "tester@gmail.com",
                        Email = "tester@gmail.com",
                        ResetUser = 1
                    };
                    await UserManager.CreateAsync(user1, "user@123222");
                }
                await UserManager.AddToRoleAsync(user1, "User");

            }
        }
    }
}
