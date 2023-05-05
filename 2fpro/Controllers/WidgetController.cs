using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

using System.IO;

using _2fpro.Controllers;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using _2fpro.Service.Interface;
using _2fpro.Models;
using _2fpro.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using _2fpro.Extension;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Posbank.Controllers
{
    public class WidgetController : BaseController
    {

        IPhotoGallery _grep;
        IProductRepository _prep;
        ICategoryRepository _crep;
        public WidgetController(IPhotoGallery gRepo, IProductRepository pRepository, ICategoryRepository crep)
        {
            _grep = gRepo;
            _prep = pRepository;
            _crep = crep;
        }
        public ActionResult InscreaserView()
        {
            return PartialView();

        }


        public async Task<IActionResult> Inscreaser(int type = 0, string folder = null, int id = 0, string selector = null, string jsonData = null)
        {
            ViewBag.result = 0;
            ViewBag.el = 0;
            ViewBag.IsMobile = HttpContext.Request.IsMobileBrowser();
            if (type == 3 && folder != null)  // ПАПКА
            {
                var path = Path.Combine("~", folder);

                var dir = new DirectoryInfo(path);
                var result = dir.GetFiles().Select(x => string.Concat(folder, x.Name)).ToList();
                var json = JsonConvert.SerializeObject(result);

                //var formated =json;
                //var path1 = Server.MapPath("~/data.json");
                //using (StreamWriter file = new StreamWriter(path1))
                //{
                //    JsonSerializer serializer = new JsonSerializer();
                //    serializer.Serialize(file, formated);
                //}

                ViewBag.result = json;

            }
            else if (type == 2 && id != 0) // ГАЛЛЕРЕЯ
            {

                var result = _grep.GetGallery(id).Images.Select(x => x.ID).ToList();
                var json = JsonConvert.SerializeObject(result);
                ViewBag.result = json;

            }
            else if (type == 1 && id != 0) // КАТАЛОГ
            {
                var item = await _prep.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == id);
                item.VisitesCount++;
                _prep.Save();
                var cart = HttpContext.Session.Get<Cart>("Cart");
                var isAdded = cart != null ? cart.Lines.Select(x => x.Product.ID).Contains(id) : false;
                var cat = await _crep.GetAsync(item.CategoryID);
                JObject result = new JObject
                {
                    {"id",id},
                    {"cat",cat.CategoryName },
                    {"name",item.ProductName},
                    {"added", isAdded},
                    {"desc",item.Description},
                    {"mat",item.Material},
                    {"pack",item.Packaging},
                    {"fill",item.Fill},
                    {"packsize",item.PackagingSize},
                    {"weight",item.Weight},
                    {"manufacturer",item.Manufacturer},
                    {"disc",item.Discount},
                    {"size",item.Size},

                    {"si",new JArray{
                        item.ProdImages.OrderByDescending(x=>x.Sortindex).Select(x=> new JObject {new JProperty("id",x.ID),new JProperty("src", x.ImageMimeType)}).ToArray()
                    }},
                    {"price",item.DiscountedPrice().ToString("N0")}//gh
                };

                var json = result.ToString();
                ViewBag.result = json;
                ViewBag.Prod = item;
            }
            else  // КОНТЕНТ
            {
                if (jsonData != null)
                {
                    var obj = JObject.Parse(jsonData);
                    ViewBag.el = obj["el"];
                    ViewBag.result = obj["pack"];
                    return Json("");
                }
            }
            ViewBag.type = type;
            ViewBag.folder = folder;
            ViewBag.selector = selector;

            return PartialView();
        }

    }
}
