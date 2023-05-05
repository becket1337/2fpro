using System;
using System.Collections.Generic;
using System.Linq;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Extension;
using _2fpro.ViewModel;
using System.Threading.Tasks;
using _2fpro.Service.Repository;
using _2fpro.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using _2fpro.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using _2fpro.Extension;

namespace _2fpro.Controllers
{

    public class HomeController : Controller
    {

        //private IMenuRepository _mrepo;
        private IConfigRepository _conf;
        UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _env;
        private ILogger _logger;
        private IMenuRepository _menuCtx;
        SignInManager<ApplicationUser> _signIn;

        IDistributedCache _cache;
        public HomeController(UserManager<ApplicationUser> userManager, IConfigRepository conf, ILogger<HomeController> logger, IMenuRepository menuCtx, IHostingEnvironment env,
            SignInManager<ApplicationUser> signIn, IDistributedCache cache
            )
        {
            _userManager = userManager;
            _conf = conf;
            _logger = logger;
            _menuCtx = menuCtx;
            _env = env;
            _signIn = signIn;
            _cache = cache;
        }


        //public IConfigRepository config { get { return _conf; } private set { _conf = value; } }
        [ResponseCache(Duration = 30)]
        public async Task<IActionResult> Index(string userid = null, string codeAuth = null, string credentialToken = null, int style = 0)
        {
            //if (!HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var lastLogged = await _cache.GetAsync("lastLogged");
            //    var encLogged = System.Text.Encoding.UTF8.GetString(lastLogged).Trim();

            //    // var user = new ApplicationUser { UserName = encLogged, Email = encLogged };
            //    //await _signIn.SignInAsync(user, true);

            //    var expiredTimeCache = await _cache.GetAsync<SessionStorage>(encLogged);
            //    if (expiredTimeCache == null)
            //    {
            //        await _cache.SetAsync<SessionStorage>(encLogged, new SessionStorage() { Cart = new Cart() });
            //    }
            //}
            //IConfigRepository _conf = new ConfigRepository();
            //IMenuRepository _mrepo = new MenuRepository();
            //IProductRepository _mprod = new ProductRepository();
            // var currUser = await _userManager.GetUserAsync(User);

            //GetCart().AddItem(new Product { ID = 1, ProductName = "sdfsdf", Price = 234324 }, 1, "adsd", null, 0, null, 32423);


            //if (_conf.Options().SelectedIsOnlineID)
            //{
            //    throw new Exception(410, "Offline");
            //}

            //var user = await UserManager.FindByIdAsync(userid);
            ViewBag.ConfirmEmail = userid != null && !User.Identity.IsAuthenticated && codeAuth == null ? true : false;
            ViewBag.SetPassword = userid != null && codeAuth != null ? true : false;
            //ViewBag.UserName = userid != null && !User.Identity.IsAuthenticated ? currUser.UserName : "";
            //ViewBag.UserId = userid != null && codeAuth != null ? userid : "";
            //ViewBag.Code = codeAuth != null && userid != null ? codeAuth : "";
            //ViewBag.Style = style;
            //ViewBag.Token = credentialToken;
            var conf = await _conf.Options();
            ViewBag.Titlee = conf.SiteName == null ? "title" : conf.SiteName;
            //var homeIndex = await _menuCtx.GetAsync(1);
            return View();
        }

        private Cart GetCart()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.Set<Cart>("Cart", cart);
            }
            return cart;
        }

        public ActionResult AttachSession()
        {
            HttpContext.Session.SetString("тест", "тестовая сессия - " + HttpContext.Session.Id);
            return Redirect("/");
        }
        //public ActionResult SetCulture(string culture)
        //{
        //    culture = CultureHelper.GetImplementedCulture(culture);

        //    HttpCookie cookie = Request.Cookies["_culture"];
        //    if (cookie != null)
        //        cookie.Value = culture;
        //    else
        //    {
        //        cookie = new HttpCookie("_culture");
        //        cookie.Value = culture;
        //        cookie.Expires = DateTime.Now.AddYears(1);
        //    }
        //    Response.Cookies.Add(cookie);/*
        //    RouteData.Values["culture"] = culture;*/
        //    return RedirectToAction("Index");
        //}
        public ActionResult ImgHolder()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
