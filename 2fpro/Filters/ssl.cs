//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using System.Web.Mvc;
//using System.Web.Routing;


//namespace _2fpro.Filters
//{
//    public class RequireHttpsAttribute : System.Web.Mvc.RequireHttpsAttribute
//    {
//        public bool RequireSecure = false;
//        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
//        {
//            if (RequireSecure)
//            {
//                // default RequireHttps functionality
//                base.OnAuthorization(filterContext);
//            }
//            else
//            {
//                // non secure requested
//                if (filterContext.HttpContext.Request.IsSecureConnection)
//                {
//                    HandleNonHttpRequest(filterContext);
//                }
//            }
//        }

//        protected virtual void HandleNonHttpRequest(AuthorizationContext filterContext)
//        {
//            if (String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
//            {
//                // redirect to HTTP version of page
//                string url = "http://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
//                filterContext.Result = new RedirectResult(url);
//            }
//        }
//    }



//}