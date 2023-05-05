using _2fpro.Service.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Extension;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using _2fpro.Data;
using Microsoft.AspNetCore.Http;

namespace _2fpro.Models.Realtime
{
    public class Myhub : Hub
    {
        private IDiagnosticService _diagService;
        private IDistributedCache _cache;
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private string _cid => Guid.NewGuid().ToString();

        public Myhub(IDiagnosticService diag, IDistributedCache cache, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _diagService = diag;
            _cache = cache;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                //Новый пользователь
                // await AnonymousAuthorization(Context.GetHttpContext(),_userManager, _signInManager, _roleManager,_cache, _diagService);

                //Авторизованный пользователь
                //var user = await _userManager.FindByNameAsync(Context.GetHttpContext().User.Identity.Name);
                //админ 
                var admin = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
                //геолокация
                var diag = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == Context.GetHttpContext().User.Identity.Name);

                // UserId - Signalr 
                var id = Context.UserIdentifier;
                string admConnId = admin.ConnectionID;
                // порверка на админа
                var isAdmin = Context.GetHttpContext().User.IsInRole("Admin");
                // текущий пользователь
                //var user = await _userManager.FindByNameAsync(Context.GetHttpContext().User.Identity.Name);
                var lastVisit = _diagService.getLastTimeVisit(diag);
                var firstTimeVisit = diag.CustomIntField;
                // проверка на первый заход на сайт если был на сайте более 40сек назад
                if ((lastVisit.TotalSeconds > 40 || diag.CustomIntField == 0) || isAdmin)
                {

                    //обновляем signalrUserId к текущему пользователю
                    if (!isAdmin)
                    {
                        await _diagService.EditConnId(diag, id, true, Context.GetHttpContext().Request.IsMobileBrowser());
                        await _diagService.FirstTimeVisiter(diag.ID);
                    }

                    //обновляем signalrUserId админа
                    if (isAdmin)
                        admConnId = await _diagService.EditConnId(admin, id, true, Context.GetHttpContext().Request.IsMobileBrowser());
                    // отправляем ответ Админу в реальном времени после подключения в виджет окно
                    if (admin.IsActivate)
                    {

                        await Clients.User(admConnId).SendAsync(//User(admConnId.CustomStringField).SendAsync(
                            "ReceiveMessage",
                            "Новый пользователь на сайте - " + DateTime.Now.ToShortTimeString(),
                            await _diagService.Diagnostics.Where(x => _diagService.getLastTimeVisit(x).TotalSeconds < 40 || x.CustomBoolField).CountAsync(),
                            diag.Location,
                            await _diagService.Diagnostics.Where(x => _diagService.getLastTimeVisit(x).TotalSeconds < 40 && x.Location == diag.Location).CountAsync(),
                            isAdmin,
                            diag,
                            Context.GetHttpContext().User.Identity.Name,
                            GetRoleByName(Context.GetHttpContext())
                        );

                    }
                }
                else
                {// клиент на сайте находится
                    await _diagService.UpdateCurrDateTime(diag.ID);
                    await Clients.User(admConnId).SendAsync("AliveTimerUpdater", Context.GetHttpContext().User.Identity.Name, lastVisit.TotalSeconds);

                }

            }
            catch (Exception e)
            {
                var eMess = string.Format("{0} Ошибка выполнения.", e);
                await Clients.All.SendAsync("badrequest", eMess);
            }
            await base.OnConnectedAsync();
        }
        public string GetRoleByName(HttpContext ctx)
        {
            var rolename = "";
            if (ctx.User.IsInRole("Admin")) rolename = "Admin";
            else if (ctx.User.IsInRole("Anonym")) rolename = "Anonym";
            else rolename = ctx.User.Identity.Name;
            return rolename;
        }
        //ответ на отключение
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    try
        //    {
        //            //пользователь
        //            //var user = await _userManager.FindByNameAsync(Context.GetHttpContext().User.Identity.Name);
        //            //админ 
        //            var admin = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
        //        //геолокация
        //        var diag = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == Context.GetHttpContext().User.Identity.Name);
        //        var isAdmin = Context.GetHttpContext().User.IsInRole("Admin");
        //        var test = Context.GetHttpContext().Response.StatusCode;
        //        //if (diag != null)
        //        //{

        //        //    await _diagService.DiagRemove(diag);
        //        //}
        //        var admConnId = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
        //        await Clients.All.SendAsync(
        //        "ReceiveMessageOut",
        //        "юзер ушел",
        //        await _diagService.Diagnostics.CountAsync(),
        //        diag.Location,
        //        await _diagService.Diagnostics.Where(x => x.Location == diag.Location).CountAsync(),
        //        isAdmin,
        //        test
        //        );
        //    }
        //    catch (Exception e)
        //    {
        //        var eMess = string.Format("{0} Ошибка выполнения.", e);
        //        await Clients.All.SendAsync("badrequest", eMess);
        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}

        public async Task DelHistory()
        {
            await _diagService.DelHistory();
        }
        public async Task HeartBeatTock()
        {
            var admin = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
            var diag = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == Context.GetHttpContext().User.Identity.Name);
            if (diag != null)
            {
                if (_diagService.getLastTimeVisit(diag).TotalSeconds > 10)
                {
                    await _diagService.UpdateCurrDateTime(diag.ID);
                }
                await Clients.User(admin.ConnectionID).SendAsync("hbeat", Context.GetHttpContext().User.Identity.Name, _diagService.getLastTimeVisit(diag).TotalSeconds);
            }

        }
        public async Task SignOut()
        {

            if (Context.User.Identity.IsAuthenticated)
            {
                var admin = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
                admin.CustomIntField = 0;
                admin.IsActivate = false;
                await _diagService.SaveChangesAsync();
                //await _signInManager.SignOutAsync();
            }
        }

        // пользователь не отвечает серверу, значит он умер (неактивен)
        public async Task heyho(string username)
        {

            try
            {
                //пользователь
                //var user = await _userManager.FindByNameAsync(Context.GetHttpContext().User.Identity.Name);
                //админ 
                var admin = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomBoolField);
                //геолокация
                var diag = await _diagService.Diagnostics.FirstOrDefaultAsync(x => x.CustomStringField == username);
                var isAdmin = Context.GetHttpContext().User.IsInRole("Admin");
                string admConnId = admin.ConnectionID;
                var locCount = await _diagService.Diagnostics.Where(x => _diagService.getLastTimeVisit(x).TotalSeconds < 45 && x.Location == diag.Location).CountAsync();
                var onlineCount = await _diagService.Diagnostics.Where(x => _diagService.getLastTimeVisit(x).TotalSeconds < 45 || x.CustomBoolField).CountAsync();
                if (diag != null)
                {
                    //обнуление первого захода до дефолта и статуса онлайн до оффлайн
                    await _diagService.EditConnId(diag, null, false);
                }
                //отправка клиенту с обновленными данными, покинувшего нас пользователя
                await Clients.User(admConnId).SendAsync(
                "ReceiveMessageOut",
                "Пользователь покинул сайт",
                Math.Max(0, onlineCount - 1),
                diag.Location,
                Math.Max(0, locCount - 1),
                false,
                diag,
                username
                );
            }
            catch (Exception e)
            {

                var eMess = string.Format("{0} Ошибка выполнения.", e);
                await Clients.All.SendAsync("badrequest", eMess);
            }

        }
        //return list of all active connections
        //public List<string> GetAllActiveConnections()
        //{
        //    return liveStream.ToList();
        //}

        public async Task AnonymousAuthorization(HttpContext ctx, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signIn, RoleManager<IdentityRole> roleManager,
           IDistributedCache _cache, IDiagnosticService diagService)
        {

            //await ctx.Response.WriteAsync(ctx.Request.Path.ToString()+", user - "+ctx.User.Identity.Name);
            //приемлемые пути
            string[] segments = new string[] { "/prod", "/pages" };


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

                // Проверка по ip в базе, если находим авторизуем
                var ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Diagnostic findByIp = null;// await diagService.Diagnostics.FirstOrDefaultAsync(x => x.IpAddress == ip);
                ApplicationUser hasIdentity = null;
                if (findByIp != null)
                {
                    hasIdentity = await userManager.FindByEmailAsync(findByIp.CustomStringField);
                    await signIn.SignInAsync(hasIdentity, true);
                }
                else //Создаем нового анонима
                {
                    var cid = _cid;

                    //await ctx.SignOutAsync(IdentityConstants.ExternalScheme);
                    var user = await userManager.FindByEmailAsync(cid + "@email.com");
                    var pass = "anonym@123321";
                    var str = ctx.Request.Path;

                    //создаем уже с правами анонимного пользователя
                    user = new ApplicationUser { UserName = cid + "@email.ru", Email = cid + "@email.ru" };
                    var result = await userManager.CreateAsync(user, pass);
                    if (result.Succeeded)
                    {
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        await userManager.ConfirmEmailAsync(user, code);
                    }
                    await userManager.AddToRoleAsync(user, "Anonym");
                    await signIn.SignInAsync(user, true);
                    // привязываем геолокализацию для пользователя и создаем сессию на пол года под него
                    await GeoLocalization(ctx, _cache, diagService, cid + "@email.ru", false);

                    if (ctx.User.Identity.IsAuthenticated && !ctx.User.IsInRole("Admin")) //если авторизованный пользователь, проверяем привязку геолокации
                    {
                        var getUserDiag = await diagService.GetAsync(ctx.User.Identity.Name);

                        if (getUserDiag == null)
                            await GeoLocalization(ctx, _cache, diagService, ctx.User.Identity.Name, false);
                    }
                }
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

        public class HubMessage
        {
            public string connId { get; set; }

            public string location { get; set; }

            public Diagnostic diag { get; set; }
            public string username { get; set; }
            public bool isadmin { get; set; }
            public string Message { get; set; }
            public int usersOnline { get; set; }
            public int usersOnlineByLoc { get; set; }
        }
    }
}
