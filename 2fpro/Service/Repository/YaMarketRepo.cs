using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace _2fpro.Service.Repository
{
    public class YaMarketRepo : IYaMarketRepo
    {

        private ApplicationDbContext _db;
        private IFileManager _fm;
        private IHostingEnvironment _env;
        private ICategoryRepository _cats;
        private IProductRepository _prods;
        private IUrlHelper _urlHelper;
        private IHttpContextAccessor _httpContextAccessor;

        public YaMarketRepo(ApplicationDbContext db, IFileManager fm, IHostingEnvironment env, ICategoryRepository cats, IProductRepository prods, IUrlHelperFactory urlHelperFactory,
                   IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _fm = fm;
            _env = env;
            _cats = cats;
            _prods = prods;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _httpContextAccessor = httpContextAccessor;

        }

        public IQueryable<YaMakret> YaMarkets { get { return _db.YaMakrets; } }

        public async Task<YaMakret> GetMarketProfile()
        {
            return await YaMarkets.FirstOrDefaultAsync();
        }


        public async Task<List<YaMakret>> Get()
        {
            return await YaMarkets.ToListAsync();
        }
        public async Task CreateProfile(YaMakret yam)
        {
            await _db.YaMakrets.AddAsync(yam);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProfile(YaMakret yam)
        {

            _db.Entry(yam).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task YamExport()
        {
            var uriScheme = _httpContextAccessor.HttpContext.Request.IsHttps || _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps ? "https" : "http";
            var yam = await GetMarketProfile();
            yam = yam == null ? new YaMakret() : yam;

            var catlist = await _cats.GetShowed();
            var prodlist = await _prods.GetShowed();


            var path = Path.Combine(_env.WebRootPath, "yaMarketExport.xml");

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    "
            };
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {

                writer.WriteStartDocument();
                writer.WriteStartElement("yml_catalog");
                writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd H:mm"));

                writer.WriteStartElement("shop");

                writer.WriteStartElement("name");
                writer.WriteString(yam.CompanyName);
                writer.WriteEndElement();

                writer.WriteStartElement("company");
                writer.WriteString(yam.CompanyName);
                writer.WriteEndElement();

                writer.WriteStartElement("url");
                writer.WriteString(yam.UrlSite);
                writer.WriteEndElement();

                writer.WriteStartElement("email");
                writer.WriteString(yam.Email);
                writer.WriteEndElement();

                writer.WriteStartElement("currencies");
                writer.WriteStartElement("currency");
                writer.WriteAttributeString("id", "RUR");
                writer.WriteAttributeString("rate", "1");
                writer.WriteEndElement();
                writer.WriteEndElement();
                /* cats */
                writer.WriteStartElement("categories");
                foreach (var item in catlist)
                {
                    writer.WriteStartElement("category");
                    writer.WriteAttributeString("id", item.ID.ToString());
                    writer.WriteString(item.CategoryName);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                /* cats end */
                /* offers */
                writer.WriteStartElement("offers");

                foreach (var offer in prodlist.Where(x => x.ToYaMarket && catlist.Where(y => !y.DoShow).Any(c => c.ID == x.CategoryID)))
                {
                    /* start offer */
                    writer.WriteStartElement("offer");
                    writer.WriteAttributeString("id", offer.ID.ToString());
                    if (offer.Cloth != null && offer.Decor != null)
                    {
                        writer.WriteAttributeString("type", "vendor.model");

                        writer.WriteStartElement("vendor");
                        writer.WriteString(offer.Cloth);
                        writer.WriteEndElement();

                        writer.WriteStartElement("model");
                        writer.WriteString(offer.Decor);
                        writer.WriteEndElement();

                    }
                    else
                    {
                        writer.WriteStartElement("name");
                        writer.WriteString(offer.ProductName);
                        writer.WriteEndElement();
                    }

                    writer.WriteStartElement("url");
                    var offerLink = Path.Combine(_env.WebRootPath, _urlHelper.Action("Details", "Product", new { id = offer.ID, title = offer.GenerateSlug() }));
                    writer.WriteString($"{uriScheme}://{_httpContextAccessor.HttpContext.Request.Host}{offerLink}");
                    writer.WriteEndElement();

                    writer.WriteStartElement("price");
                    writer.WriteString(offer.DiscountedPrice().ToString().Replace(".00", ""));
                    writer.WriteEndElement();

                    writer.WriteStartElement("currencyId");
                    writer.WriteString("RUR");
                    writer.WriteEndElement();

                    writer.WriteStartElement("categoryId");
                    writer.WriteString(offer.CategoryID.ToString());
                    writer.WriteEndElement();

                    /*pictures*/
                    var href = "";
                    if (offer.ProdImages.Any(x => x.IsPreview == 1))
                    {
                        var previewImg = offer.ProdImages.FirstOrDefault(x => x.IsPreview == 1);
                        href = Path.Combine(_env.WebRootPath, "/Content/Files/Product/" + offer.ID.ToString() + "/" + previewImg.ImageMimeType);
                        writer.WriteStartElement("picture");
                        writer.WriteString($"{uriScheme}://{_httpContextAccessor.HttpContext.Request.Host}{href}");
                        writer.WriteEndElement();
                    }
                    foreach (var prodImg in offer.ProdImages.Where(x => x.IsPreview == 0))
                    {

                        href = Path.Combine(_env.WebRootPath, "/Content/Files/Product/" + offer.ID.ToString() + "/height500/" + prodImg.ImageMimeType);
                        writer.WriteStartElement("picture");
                        writer.WriteString($"{uriScheme}://{_httpContextAccessor.HttpContext.Request.Host}{href}");
                        writer.WriteEndElement();
                    }

                    /*end pictures*/


                    if (!string.IsNullOrWhiteSpace(offer.Description))
                    {
                        writer.WriteStartElement("description");
                        writer.WriteCData(offer.Description);
                        writer.WriteEndElement();

                    }

                    if (!string.IsNullOrWhiteSpace(yam.SalesNotes))
                    {
                        writer.WriteStartElement("sales_notes");
                        writer.WriteString(yam.SalesNotes);
                        writer.WriteEndElement();
                    }


                    if (!string.IsNullOrWhiteSpace(offer.Manufacturer))
                    {
                        writer.WriteStartElement("country_of_origin");

                        writer.WriteString(offer.Manufacturer);
                        writer.WriteEndElement();
                    }
                    if (!string.IsNullOrWhiteSpace(offer.Color))
                    {
                        writer.WriteStartElement("param");
                        writer.WriteAttributeString("name", "Цвет");
                        writer.WriteString(offer.Color);
                        writer.WriteEndElement();
                    }
                    if (!string.IsNullOrWhiteSpace(offer.Material))
                    {
                        writer.WriteStartElement("param");
                        writer.WriteAttributeString("name", "Материал");
                        writer.WriteString(offer.Material);
                        writer.WriteEndElement();
                    }
                    if (!string.IsNullOrWhiteSpace(offer.Size))
                    {
                        writer.WriteStartElement("param");
                        writer.WriteAttributeString("name", "Размер");
                        writer.WriteString(offer.Size);
                        writer.WriteEndElement();
                    }

                    if (offer.PackagingSize > 0)
                    {
                        writer.WriteStartElement("param");
                        writer.WriteAttributeString("name", "Кол-во в упаковке");
                        writer.WriteString(offer.PackagingSize.ToString());
                        writer.WriteEndElement();
                    }


                    writer.WriteEndElement();
                    /* end offer */
                }


                writer.WriteEndElement();
                /* offers end*/

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();


            }

        }
    }
}
