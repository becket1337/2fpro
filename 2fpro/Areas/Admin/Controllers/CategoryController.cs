using System;




using Kendo.Mvc.UI;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Kendo.Mvc.Extensions;
using _2fpro.Controllers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

using _2fpro.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace _2fpro.Areas.Admin.Controllers
{

    public class CategoryController : BaseController
    {
        //
        // GET: /Category/

        //
        // GET: /Category/

        public int PageSize = 4;
        ICategoryRepository _repository;
        IYaMarketRepo _yamService;

        public CategoryController(ICategoryRepository repository, IYaMarketRepo yamService)
        {
            _repository = repository;
            _yamService = yamService;
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var list = _repository.Categories.Where(x => x.ParentCategoryId == 0).Select(x => new { ID = x.ID, CategoryName = x.CategoryName });
            ViewBag.Cats = new SelectList(list, "ID", "CategoryName");


            return View();
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult CatList()
        {

            var list = _repository.Categories.Where(x => x.ParentCategoryId == 0).Select(x => new { ID = x.ID, CategoryName = x.CategoryName });
            ViewBag.Cats = new SelectList(list, "ID", "CategoryName");


            return View();
        }
        public ActionResult Catalog()
        {
            var list = _repository.Get();
            return View(list.ToList());
        }
        public async Task<IActionResult> CatsMenu()
        {
            List<Category> cats = new List<Category>();
            cats = await _repository.Categories.OrderBy(x => x.Sortindex).ToListAsync();

            return PartialView(cats);
        }
        public PartialViewResult CatsMenu2()
        {
            return PartialView(_repository.Categories.Where(x => x.CatType == "игрушка").OrderBy(x => x.Sequance).ToList());
        }
        public ActionResult Create(int id)
        {
            var result = _repository.Categories.FirstOrDefault(x => x.ID == id);

            return View(result);

        }

        [HttpPost]
        public JsonResult EditSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                _repository.UpdateSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return base.Json("");
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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

                    await _repository.SavePhoto(file, pid);

                    //list.Add(item);
                }
            }

            return Json(new { pimgid = pid });
        }
        [HttpPost]
        [Area("Admin")]
        public JsonResult DelPhoto(int pimgid)
        {
            var cat = _repository.Get(pimgid);
            _repository.PhotoDel(cat);
            return Json(new { id = pimgid });
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetProdImages(int pid)
        {
            return ViewComponent("GetCatImage", new { id = pid });
        }

        public ActionResult Filter_Categories()
        {
            var items = _repository.Categories.Select(
                    x => new CategoryViewModel
                    {
                        ID = x.ID,
                        CategoryName = x.CategoryName
                    }
                );
            return Json(items);
        }

        public JsonResult GetCategories()
        {
            var jsonResult = Json(_repository.Categories.ToList());

            return jsonResult;
        }

        [ResponseCache(Duration = 20)]
        public IActionResult CatTags()
        {
            return ViewComponent("CatItems", new { ismain = true });
        }
        [Area("Admin")]
        public async Task<IActionResult> Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _repository.Categories.Include("Products").Where(x => x.ParentCategoryId > 0).ToDataSourceResult(request, o => new CategoryViewModel
            {
                CatDescription = o.CatDescription,
                CatType = o.CatType,
                CategoryName = o.CategoryName,
                DoShow = o.DoShow,
                Sortindex = o.Sortindex,
                ID = o.ID,
                Prods = _repository.Get(o.ID).Products.Count(),
                imgLink = string.IsNullOrWhiteSpace(o.ImageMimeType) ? "//:0:" : "/Content/Files/Category/" + o.ID + "/height500/" + o.ImageMimeType,
                ImageMimeType = o.ImageMimeType,
                ParentCategoryId = o.ParentCategoryId,
                ParentCategoryName = o.ParentCategoryId == 0 ? "" : _repository.Get(o.ParentCategoryId).CategoryName

            });
            return Json(result);
        }
        [Area("Admin")]
        public async Task<IActionResult> Editing_ReadModel([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _repository.Categories.Include("Products").Where(x => x.ParentCategoryId == 0).ToDataSourceResult(request, o => new CategoryViewModel
            {
                CatDescription = o.CatDescription,
                CatType = o.CatType,
                CategoryName = o.CategoryName,
                DoShow = o.DoShow,
                Sortindex = o.Sortindex,
                ID = o.ID,
                Prods = _repository.Get(o.ID).Products.Count(),
                imgLink = string.IsNullOrWhiteSpace(o.ImageMimeType) ? "//:0:" : "/Content/Files/Category/" + o.ID + "/height500/" + o.ImageMimeType,
                ImageMimeType = o.ImageMimeType,
                ParentCategoryId = o.ParentCategoryId,
                ParentCategoryName = o.ParentCategoryId == 0 ? "" : _repository.Get(o.ParentCategoryId).CategoryName

            });
            return Json(result);
        }
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Editing_Create([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {

            var results = new List<CategoryViewModel>();

            if (cat != null && ModelState.IsValid)
            {

                await _repository.Create(cat);
                cat.ID = _repository.Categories.First().ID;
                results.Add(cat);
                //await _yamService.YamExport();
            }


            return Json(results.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> NewCat(string catname)
        {
            if (catname != null)
            {
                await _repository.newCat(catname);
            }
            return Json("");
        }
        [ResponseCache(Duration = 20)]
        public IActionResult CatSlider()
        {

            return ViewComponent("Mainslider");
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<ActionResult> Editing_Update([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {
            if (cat != null && ModelState.IsValid)
            {

                await _repository.Edit(cat);
                //await _yamService.YamExport();
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Editing_Destroy([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {
            if (cat != null)
            {
                var item = _repository.Get(cat.ID);
                //_repository.PhotoDel(item);
                _repository.Delete(item);
               // await _yamService.YamExport();
            }


            return Json(ModelState.ToDataSourceResult());
        }

    }
}
