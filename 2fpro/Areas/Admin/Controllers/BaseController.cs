using System;

using System.Threading;

using _2fpro.Extension;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Controllers
{
    public class BaseController : Controller
    {

        IConfigRepository _conf;
        public BaseController() { }
        public BaseController(IConfigRepository conf)
        {
            _conf = conf;



        }

        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{



        //    string cultureName = null;




        //   var cultureCookie = HttpContext.Request.Cookies["_culture"];
        //    if (cultureCookie != null)
        //        cultureName = cultureCookie;
        //    else
        //    {
        //        cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>() != null && Request.HttpContext.Features.Get<IRequestCultureFeature>().Provider!=null ?
        //                Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.EnglishName :  // obtain it from HTTP header AcceptLanguages
        //                null;
        //        cultureCookie = HttpContext.Response.Cookies.Append("_culture",cultureName,new CookieOptions() { Expires=DateTime.Now.AddYears(1) });

        //    }
        //    // Validate culture name
        //    cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

        //    // Modify current thread's cultures           
        //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
        //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //    return base.BeginExecuteCore(callback, state);
        //}
    }
}
