using _2fpro.Models;
using _2fpro.Models.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Extension
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseSitemap(this IApplicationBuilder app,
            string rootUrl = "https://localhost:44356")
        {
            return app.UseMiddleware<SitemapMiddleware>(new[] { rootUrl });
        }

        public static IApplicationBuilder UseSiteConfiguration(this IApplicationBuilder app, SiteConfig conf)
        {
            return app.UseMiddleware<SiteConfigSettingsMiddleware>(conf);
        }
    }
}
