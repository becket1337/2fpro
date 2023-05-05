using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidecode.NET;

namespace _2fpro.Models.Middleware
{
    public class SitemapMiddleware
    {
        private RequestDelegate _next;
        private string _rootUrl;

        private int prodsPerPage = 16;
        private int totalProds = 1;
        public int TotalPage
        {
            get { return (int)Math.Ceiling((double)totalProds / (double)prodsPerPage); }
        }
        public SitemapMiddleware(RequestDelegate next, string rootUrl)
        {
            _next = next;
            _rootUrl = rootUrl;
        }

        public async Task InvokeAsync(HttpContext context, LinkGenerator linkGen, IProductRepository _prod, IMenuRepository _menuRep, ICategoryRepository _catRep, IConfigRepository _confRep, IOptionsMonitor<SiteConfig> _config)
        {
            if (context.Request.Path.Value.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase))
            {
                var stream = context.Response.Body;
                context.Response.Headers.Clear();
                context.Response.StatusCode = 200;
                context.Response.Headers.Add("X-Sitemap-Tag", "noindex");
                context.Response.ContentType = "application/xml";

                var prod = await _prod.GetAsync();
                var modifyProd = prod.OrderBy(x => x.ModifiedDate).FirstOrDefault();
                string sitemapContent = @"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">";
                //Add domain default link to sitemap
                sitemapContent += $@"<url><loc>{_config.CurrentValue.SitePath}</loc>
                        <priority>1.0</priority>
                       
                        <changefreq>weekly</changefreq>
                        </url>";
                //START  Sitemap.xml
                foreach (var menu in await _menuRep.Menues.Where(x => x.Url != "ProdList").ToListAsync())
                {
                    if (menu.ParentId == 0)
                    {
                        sitemapContent += $@"<url><loc>{_rootUrl}/pages/{menu.Url}</loc>
                        <priority>0.8</priority>
                        <lastmod>{menu.LastModifiedDate.ToShortDateString()}</lastmod>
                        <changefreq>weekly</changefreq>
                        </url>";
                    }
                    else
                    {
                        sitemapContent += $@"<url><loc>{_rootUrl}/pages/{await _menuRep.GetMenuUrl(menu.ParentId)}/{menu.Url}</loc>
                        <priority>0.8</priority>
                        <lastmod>{menu.LastModifiedDate}</lastmod>
                        <changefreq>weekly</changefreq>
                        </url>";
                    }

                }
                sitemapContent += $@"<url><loc>{_rootUrl}/ProdList</loc>
                        <priority>0.8</priority>
                        <lastmod>{modifyProd.ModifiedDate.ToShortDateString()}</lastmod>
                        <changefreq>weekly</changefreq>
                        </url>";
                foreach (var item in await _prod.GetAsync())
                {
                    var total = _catRep.Categories.Count();

                    var datetime = item.ModifiedDate; //cat.Products.Any() ? cat.Products.OrderByDescending(x => x.ID).FirstOrDefault().ModifiedDate : DateTime.Now;
                    var getUrl = linkGen.GetUriByAction(context, "Details", "Product", new { id = item.ID, title = item.GenerateSlug() });

                    sitemapContent += $@"<url><loc>{getUrl}</loc>
                        <priority>0.5</priority>
                        <lastmod>{datetime.ToShortDateString()}</lastmod>
                        <changefreq>weekly</changefreq>
                        </url>";

                }
                sitemapContent += "</urlset>";
                using (var memoryStream = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(sitemapContent);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(stream, bytes.Length);
                }
            }
            //Start ROBOTS.tXT
            //else if (context.Request.Path.Value.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase))
            //{
            //    var stream = context.Response.Body;
            //    context.Response.StatusCode = 200;
            //    context.Response.Headers.Add("X-Robots-Tag", "noindex");
            //    context.Response.ContentType = "text/html;charset=utf-8";
            //    string robotsContent = null;

            //    robotsContent = !string.IsNullOrEmpty(_confRep.Configs.FirstOrDefault().Robots) ? System.Net.WebUtility.HtmlDecode(_confRep.Configs.FirstOrDefault().Robots) : "User-agent: *";
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        var bytes = Encoding.UTF8.GetBytes(robotsContent);
            //        memoryStream.Write(bytes, 0, bytes.Length);
            //        memoryStream.Seek(0, SeekOrigin.Begin);
            //        await memoryStream.CopyToAsync(stream, bytes.Length);
            //    }
            //    //await _next(context);
            //}
            else { await _next(context); }

        }
    }

}
