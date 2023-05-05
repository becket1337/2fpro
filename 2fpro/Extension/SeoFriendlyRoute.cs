using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace _2fpro.Extension
{
    public class SeoFriendlyRoute : Route
    {
        public SeoFriendlyRoute(IRouter router, string temp, IInlineConstraintResolver resolver)
            : base(router, temp, resolver)
        {
        }
        public override Task RouteAsync(RouteContext context)
        {
            var routeData = context.RouteData;
            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("id"))
                    routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
            }
            return base.RouteAsync(context);

        }
        //public override RouteData GetRouteData(HttpContextBase httpContext)
        //{
        //    var routeData = base.GetRouteData(httpContext);

        //    if (routeData != null)
        //    {
        //        if (routeData.Values.ContainsKey("id"))
        //            routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
        //    }

        //    return routeData;
        //}

        private object GetIdValue(object id)
        {
            if (id != null)
            {
                string idValue = id.ToString();

                var regex = new Regex(@"^(?<id>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }

            return id;
        }
    }
}