using System;
using System.Collections.Generic;
using System.Linq;


using Kendo.Mvc.UI;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Kendo.Mvc.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _2fpro.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;

namespace _2fpro.Areas.Admin.Controllers
{

    public class ProductController : Controller
    {
        //

        public int PageSize = 15;


        IProductRepository _pRepository;
        private IDistributedCache _sqlCache;
        ICategoryRepository _cRep;
        IConfigRepository _conf;
        private readonly SiteConfig _settings;
        private IYaMarketRepo _yamService;



        public ProductController(IProductRepository pRepository, IConfigRepository conf, ICategoryRepository cRep, IOptionsMonitor<SiteConfig> settings, IDistributedCache cache, IYaMarketRepo yamService)
        {
            _cRep = cRep;
            _pRepository = pRepository;
            _conf = conf;
            _settings = settings.CurrentValue;
            _sqlCache = cache;
            _yamService = yamService;
        }
        [HttpGet("products/{id}/{title}")]
        public async Task<IActionResult> Details(string title, int id)
        {

            ViewBag.Sitename = _settings.SiteName;
            ViewBag.IsLiveStatus = _settings.SiteLiveStatus;
            ViewBag.Sitepath = _settings.SitePath;
            //return new RedirectToRouteResult(RouteValues<"str","sdf">());
            //@"(-)|[A-Za-z]", ""

            //string pattern = @"(\d+)$";
            //Regex regex = new Regex(pattern);
            //Match match = regex.Match(title);
            //int prodId;
            Product item = null;

            //var succ = int.TryParse(match.Value.Trim(), out prodId);
            //if (succ)
            //{
            //    item = await _pRepository.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == id);
            //}
            //else { return NotFound(); }
            item = await _pRepository.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == id);

            if (item == null || item.GenerateSlug() != title) return NotFound();
            var cat = await _cRep.GetAsync(item.CategoryID);
            ViewBag.ModelName = cat.ParentCategoryId;
            ViewBag.catname = cat.ParentCatname;
            item.VisitesCount += 1;
            _pRepository.Save();
            return View(item);
        }

        [Route("ProdList/{catid}/{catname}/{page}", Name = "Prodlist")]
        [Route("ProdList", Name = "prodindex")]
        public async Task<IActionResult> ProdList(int page = 1, int catid=0)
        {
            var conf = await _conf.Options();
            ViewBag.Titlee = conf.SiteName == null ? "title" : conf.SiteName;
            if (catid == 0) return BadRequest("500");
            var cat = await _cRep.GetAsync(catid);
            if(cat==null)return NotFound();

            //else
            //{
            //    ViewBag.Page = page;
            //    ViewBag.CatName = cat.CategoryName;
            //    ViewBag.CatID = catid == 0 && _pRepository.Products.Count() > 0 ? _cRep.Categories.Include("Products").Where(x => x.Products.Count() > 0).FirstOrDefault().ID : catid;
            //    ViewBag.MinPrice = cat.Products.Any() ? await _pRepository.Products.Where(x => x.CategoryID == catid).MinAsync(x => x.Price) : 4500;

            //var encryptedId = Int32.Parse(SecurityService.Decrypt(catId));
            //ViewBag.Catid = encryptedId;
            ProductListViewModel vm = new ProductListViewModel
            {
                Products = cat.Products                    
                    .OrderBy(x => x.ID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    Service = "Product",
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = cat.Products.Count(),
                    CatId = catid

                }

                    };


            return View(cat);



        }
        public async Task<ActionResult> CatalogView(int catid=0)
        {
            if (catid == 0) return BadRequest("500");
            var cat = await _cRep.GetAsync(catid);
            if (cat == null) return NotFound();
            ViewBag.Cat = cat;
            ViewBag.Titlee = cat.CategoryName;
            var list = await _pRepository.GetAsync();
            return View(list.Where(x=>x.CategoryID==catid&& !x.DoShow));
        }
        [ResponseCache(Duration = 10, VaryByQueryKeys = new string[] { "jsonData" })]
        public IActionResult ProdListPartial(string jsonData)
        {

            //JObject obj2 = JObject.Parse(jsonData);
            //int id = obj2.SelectToken("catId").Value<int>();
            ////int decryptedId =Int32.Parse(SecurityService.Decrypt(id));
            //int num = obj2.SelectToken("page").Value<int>();
            //var list = _pRepository.Products.Where(x => x.CategoryID == id);
            //ProductListViewModel model2 = new ProductListViewModel
            //{
            //    Products = await list.OrderByDescending(x=>x.ID).Skip<Product>(((num - 1) * this.PageSize)).Take<Product>(this.PageSize).ToListAsync(),
            //    Category = await _cRep.Categories.SingleOrDefaultAsync(x=>x.ID==id)

            //};
            //PagingInfo info = new PagingInfo
            //{
            //    Service = "Product",
            //    CurrentPage = num,
            //    Sort = this.PageSize.ToString(),
            //    ItemsPerPage = this.PageSize,
            //    TotalItems = await list.CountAsync(),
            //    CatId = id
            //};
            //model2.PagingInfo = info;
            //ProductListViewModel model = model2;
            return ViewComponent("ProdList", jsonData);
        }
        [ResponseCache(Duration = 10)]
        public IActionResult LastProds()
        {
            return ViewComponent("LastnewProds");
        }
        //public PartialViewResult RndProds(int catId,int pid) {
        //    var list = _pRepository.Products.Where(x => x.CategoryID == catId && x.ID != pid).ToArray();
        //    var rnd = new Random();
        //    var result = rnd.Next(list.Count());
        //    var rndlist = new List<Product>();
        //    for(var i=0;)


        //    return PartialView(result);
        //}
        public ActionResult ExcelImport()
        {

            return View();
        }
        public ActionResult ExcelImport(IFormFile file)
        {

            return View();
        }

        public bool ProdIsAdded(int id)
        {

            Cart cart = _sqlCache.Get<Cart>("Cart");
            bool isAdded = false;
            if (cart != null)
            {
                isAdded = cart.Lines.Select(x => x.Product.ID).Contains(id);
            }
            return isAdded;
        }

        public async Task<IActionResult> ItemInfo(int id) // каталог - быстрый просмотр
        {
            // Thread.Sleep(500000);
            var item = await _pRepository.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == id);
            var isAdded = GetCart().Lines.Select(x => x.Product.ID).Contains(id);
            return Json(new
            {
                cat = await _pRepository.GetCatName(item.CategoryID),
                name = item.ProductName,
                added = isAdded,
                desc = item.Description,
                si = item.ProdImages.Select(x => x.ID).ToArray(),
                price = item.Price
            });
        }


        [ResponseCache(Duration = 360)]
        public async Task<IActionResult> ClothList(int id, string name = null, string price = null)
        {

            //ViewBag.ProdName = name;
            //ViewBag.Price = price;
            ViewBag.Product = await _pRepository.GetAsync(id);
            return PartialView();
        }
        [ResponseCache(Duration = 360)]
        public ActionResult ColorList()
        {


            return PartialView();
        }

        [HttpPost]
        public JsonResult EditProdSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                _pRepository.UpdateProdSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return base.Json("");
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var list = _cRep.Categories.OrderBy(x => x.Sortindex).Select(x => new { ID = x.ID, CategoryName = x.CategoryName });
            ViewBag.Cats = new SelectList(list, "ID", "CategoryName");
            ViewBag.Fill = "QQQQQ";
            ViewData["Catss"] = _cRep.Categories.OrderBy(x => x.Sortindex)
                        .Select(e => new
                        {
                            ID = e.ID,
                            CategoryName = e.CategoryName //+ " (" + e.CatType + ")",

                        });
            return View();
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            DataSourceResult result = _pRepository.Products.Include("ProdImages").ToDataSourceResult(request, o => new ProductViewModel
            {
                ProductName = o.ProductName,
                Description = o.Description,
                Price = o.Price,
                Material = o.Material,
                Size = o.Size,
                PackagingSize = o.PackagingSize,
                Color = o.Color,
                Cloth = o.Cloth,
                Decor = o.Decor,
                DoShow = o.DoShow,
                ToYaMarket = o.ToYaMarket,
                InStock = o.InStock,
                Lacquering = o.Lacquering,
                Manufacturer = o.Manufacturer,
                Weight = o.Weight,
                Fill = o.Fill,
                Discount = o.Discount,
                Sortindex = (int)o.Sortindex,
                ProductType = o.ProductType,
                CategoryName = o.CatName,
                ID = o.ID,
                CategoryID = o.CategoryID,
                imgLink = o.ProdImages.Where(x => x.IsPreview == 1).FirstOrDefault() != null ? "/Content/Files/Product/" + o.ID + "/height500/" + o.ProdImages.Where(x => x.IsPreview == 1).FirstOrDefault().ImageMimeType : ""

            });

            return Json(result);
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editing_Create([DataSourceRequest] DataSourceRequest request, ProductViewModel prod)
        {

            var results = new List<ProductViewModel>();

            if (prod != null && ModelState.IsValid)
            {

                await _pRepository.CreateAsync(null, prod);
                prod.ID = _pRepository.Products.OrderByDescending(x => x.ID).FirstOrDefault().ID;
                results.Add(prod);
                //await _yamService.YamExport();
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editing_Update([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var target = await _pRepository.Products.FirstOrDefaultAsync(x => x.ID == product.ID);
                if (target != null)
                {
                    var cat = await _cRep.GetAsync(product.CategoryID);
                    target.ID = product.ID;
                    target.CategoryID = product.CategoryID;
                    target.CatName = cat.CategoryName;
                    target.Price = product.Price;
                    target.ProductName = product.ProductName;
                    target.Manufacturer = product.Manufacturer;
                    target.DoShow = product.DoShow;
                    target.ToYaMarket = product.ToYaMarket;
                    target.InStock = product.InStock;
                    target.Material = product.Material;
                    target.Packaging = product.Packaging;
                    target.PackagingSize = product.PackagingSize;
                    target.Weight = product.Weight;
                    target.Color = product.Color;
                    target.Cloth = product.Cloth;
                    target.Decor = product.Decor;
                    target.Discount = product.Discount;
                    target.Fill = product.Fill;
                    target.Description = product.Description;
                    target.ModifiedDate = DateTime.Now;
                    target.ProductType = product.ProductType;
                    target.Size = product.Size;

                    await _pRepository.EditAsync(target);
                    //await _yamService.YamExport();
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EditSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                await _pRepository.UpdateSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return base.Json("");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editing_Destroy([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null)
            {
                var item = _pRepository.Get(product.ID);
                _pRepository.PhotoDelAll(product.ID);
                _pRepository.Delete(item);
                // await _yamService.YamExport();
            }


            return Json(ModelState.ToDataSourceResult());
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadProdImages()
        {

            return PartialView();
        }

        [HttpPost]
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> UploadProdImages(List<IFormFile> photos, int pid)
        {
            List<int> list = new List<int>();

            if (photos != null)
            {
                foreach (var file in photos)
                {

                    await _pRepository.SavePhoto(file, pid);

                    //list.Add(item);
                }
                //await _yamService.YamExport();
            }

            return Json(new { pimgid = pid });
        }

        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProdImages(int pid)
        {
            return ViewComponent("GetProdImages", new { id = pid });
        }

        [ResponseCache(Duration = 180, VaryByQueryKeys = new string[] { "id", "prodid" })]
        public async Task<IActionResult> LastnewProds(int id = 0, int prodid = 0)
        {

            List<Product> prod = new List<Product>();

            ViewBag.ID = id;
            if (id != 0 && await _pRepository.Products.AnyAsync())
                prod = await _pRepository.Products.Where(x => x.ID != prodid).OrderByDescending(x => x.ID).Take(10).ToListAsync();
            if (await _pRepository.Products.AnyAsync())
                prod = await _pRepository.Products.ToListAsync();
            return PartialView(prod);
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public async Task<IActionResult> ImgPreview(int pimgid)
        {
            await _pRepository.SetPreview(pimgid);
            return Json("");
        }

        public async Task<bool> CheckPreview(int pimgid)
        {

            var check = await _pRepository.CheckPreview(pimgid);
            return check;
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DelAll()
        {

            await _pRepository.ClearTable();
            //await _yamService.YamExport();
            return Json("");
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DeleteImages(int id)
        {
            _pRepository.PhotoDelAll(id);
            //await _yamService.YamExport();
            return Json("");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DelPhoto(int pimgid)
        {
            var img = _pRepository.GetImg(pimgid);
            _pRepository.PhotoDel(img);
            //await _yamService.YamExport();
            return Json(new { id = img.ID });
        }
        [HttpPost]
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetPreview(int pimgid)
        {

            await _pRepository.SetPreview(pimgid);
            var prodImg = await _pRepository.GetImgAsync(pimgid);
            var prod = await _pRepository.GetAsync(prodImg.ProductID);
            //await _yamService.YamExport();
            return Json(new { src = "/Content/Files/Product/" + prod.ID + "/" + prodImg.ImageMimeType, pid = _pRepository.GetImg(pimgid).ProductID });
        }
        private Cart GetCart()
        {
            var cart = _sqlCache.Get<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                _sqlCache.Set<Cart>("Cart", cart);
            }
            return cart;
        }
        private void UpdateCart(Cart cart)
        {
            _sqlCache.Set<Cart>("Cart", cart);

        }


    }
}
