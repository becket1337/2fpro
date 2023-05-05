using Newtonsoft.Json;


using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace _2fpro.Areas.Admin.Controllers
{

    public class MenuController : Controller
    {
        //
        // GET: /Menu/
        private IApplicationLifetime ApplicationLifetime { get; set; }

        IMenuRepository _repository;
        private IHostingEnvironment _env;

        public MenuController(IMenuRepository repository, IApplicationLifetime appLifetime, IHostingEnvironment env)
        {

            _repository = repository;
            ApplicationLifetime = appLifetime;
            _env = env;
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult MenuList(int type = 0, int page = 1)
        {
            //var menuSectionList = new SelectList(new MenuViewModel().MenuSelectList, "ID", "Text");//.Where(x => x.Value == "0");
            //menuSectionList.FirstOrDefault(x=>x.Value=="0").Selected= true;
            var mlist = _repository.Menues.Where(x => x.ParentId == 0).ToList();
            mlist.Add(new Menu { Text = "Выбрать..", Id = 0, Url = "ddffkkll", Body = "ddffgghhww", MenuSection = 0 });
            ViewBag.MenuSectionList = new SelectList(new MenuViewModel().MenuSelectList, "ID", "Text", type); //menuSectionList;
            ViewBag.ParentIdList = new SelectList(mlist, "Id", "Text");
            //int pageSize = 25;
            MenuViewModel vm = new MenuViewModel
            {
                //Menues = _repository.Menues
                //    .Where(x => x.ParentId == 0 && x.MenuSection == (int)type)
                //    .OrderBy(x => x.SortOrder)
                //    .ToList()
                //    .Skip((page - 1) * pageSize)
                //    .Take(pageSize),
                Menues = _repository.Menues.Where(x => x.MenuSection == type).ToList(),
                //PagingInfo = new PagingInfo
                //{
                //    TotalItems = _repository.Menues.Where(x => x.ParentId == 0 && x.MenuSection == (int)type).Count(),
                //    CurrentPage = page,
                //    ItemsPerPage = pageSize

                //},
                ctype = type
            };


            return View(vm);

        }
        public async Task<IActionResult> Departments()
        {
            var list =await  _repository.GetAsync();
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> SS(int menuSection = 0)
        {

            var items = new List<Menu>();//= await _repository.Menues.ToListAsync();

            // _logger.LogInformation("Menu item count - {count}", _repository.Get().Count());
            //if (menuSection == 1)
            //{
            //    items = await _repository.Menues.Where(x => x.MenuSection == 1).ToListAsync();
            //}
            //if (menuSection == 0)
            //{
            //    ViewBag.IsFoot = true;
            //    items = await _repository.Menues.Where(x => x.MenuSection == 0).ToListAsync();
            //}

            ViewBag.Str = items.Count();

            return ViewComponent("MenuItems", menuSection);
        }

        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> Create(int id = 0, int type = 0)
        {

            /*var menu = new Menu();*/

            if (id != 0)
            {
                var item = await _repository.GetAsync(id);
                ViewBag.ParentId = id;
                ViewBag.MenuSection = item.MenuSection;
            }
            else
            {
                ViewBag.ParentId = 0;
                ViewBag.type = type;
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (_repository.Menues.Any(x => x.Url == menu.Url && x.MenuSection == menu.MenuSection))
            {
                //ModelState.AddModelError("", "Раздел с таким именем-URL уже существует");
                TempData["message"] = "Раздел с таким именем-URL уже существует";
                TempData["type"] = 4;
                return View();
            }
            if (ModelState.IsValid)
            {
                _repository.Create(menu);

                //ApplicationLifetime.StopApplication();
                TempData["message"] = "Раздел " + menu.Text + " создан";
                TempData["type"] = 1;
                if (menu.ParentId == 0)
                {
                    return RedirectToAction("MenuList", new { type = menu.MenuSection });
                }

                return RedirectToAction("Details", new { id = menu.ParentId });

            }
            if (menu.ParentId != 0)
            {
                var item = await _repository.GetAsync(menu.ParentId);
                ViewBag.ParentId = menu.ParentId;
                ViewBag.type = item.MenuSection;
            }
            else
            {
                ViewBag.ParentId = 0;
            }
            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return View(menu);
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
        public async Task<IActionResult> Details(int id, int page = 1)
        {
            var item = await _repository.GetAsync(id);

            if (item != null)
            {
                ViewBag.page = page;
                ViewBag.type = item.MenuSection;
                return View(item);
            }

            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return RedirectToAction("MenuList", new { type = item.MenuSection, page = page });
        }

        public ActionResult ChildMenus(int id)
        {
            if (id != 0)
            {
                var objs = _repository.Menues.Where(x => x.ParentId == id).OrderBy(x => x.SortOrder).ToList();
                return PartialView(objs);
            }
            return Content("");
        }
        [Area("Admin")]
        public async Task<IActionResult> Edit(int id, int page = 1)
        {
            if (id != 0)
            {

                var item = await _repository.GetAsync(id);
                ViewBag.type = item.MenuSection;
                ViewBag.page = page;
                ViewBag.sort = item.SortOrder;
                return View(item);
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Area("Admin")]
        public ActionResult Edit(Menu menu, int page = 1)
        {
            if (ModelState.IsValid)
            {
                _repository.Edit(menu);
                //HttpRuntime.UnloadAppDomain();
                TempData["message"] = menu.ParentId == 0 ? "Раздел " + menu.Text + " отредактирован" : "Подраздел " + menu.Text + " отредактирован";
                TempData["type"] = 1;
                if (menu.ParentId == 0) return RedirectToAction("MenuList", new { type = menu.MenuSection, page = page });
                else return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }
            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return View(menu);
        }

        [Authorize(Roles = "Admin")]
        [Area("Admin")]
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            var item = await _repository.GetAsync(id);
            ViewBag.page = page;
            return View(item);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public async Task<IActionResult> Delete(Menu menu, int page = 1)
        {

            if (_repository.Menues.Any(x => x.ParentId == menu.Id))
            {
                TempData["message"] = "Удалите сначала все подразделы раздела " + menu.Text;
                TempData["type"] = 4;
                //ModelState.AddModelError("", "Удалите сначала все подразделы меню");
                return RedirectToAction("Details", new { id = menu.Id, page = page });
            }
            var item = await _repository.GetAsync(menu.Id);
            if (item == null)
            {
                TempData["message"] = "что-то пошло не так";
                TempData["type"] = 4;
                return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }
            if (item.ParentId == 0)
            {
                TempData["message"] = item.ParentId == 0 ? "Раздел " + item.Text + " удален" : "Подраздел " + item.Text + " удален";
                TempData["type"] = 1;
                _repository.Delete(item);
                return RedirectToAction("MenuList", new { type = menu.MenuSection, page = page });
            }
            else
            {
                TempData["message"] = item.ParentId == 0 ? "Раздел " + item.Text + " удален" : "Подраздел " + item.Text + " удален";
                TempData["type"] = 1;
                _repository.Delete(item);

                return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }


        }
        [Area("Admin")]
        public async Task<IActionResult> Editing_Read_MenuChilds([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, int parentId = 0)
        {

            Kendo.Mvc.UI.DataSourceResult result = null;
            var item = await _repository.GetAsync();
            result = item.Where(x => x.ParentId == parentId).ToDataSourceResult(request, o => new MenuViewModel
            {
                Url = o.Url,
                Text = o.Text,
                Body = o.Body,
                MenuSection = o.MenuSection,
                Id = o.Id,
                SortOrder = o.SortOrder,
                ParentId = o.ParentId,
                SeoKeywords = o.SeoKeywords,
                SeoDescription = o.SeoDescription,
                BodyEng = o.BodyEng,
                CustomField=o.CustomField
                
                //MenuSectionList=new List<SelectListItem> {
                //    new SelectListItem{ Text="Первый раздел",Value="0"},    
                //    new SelectListItem{ Text="Второй раздел",Value="1"}
                //}


            });

            return Json(result);
        }
        [Area("Admin")]
        public async Task<IActionResult> Editing_Read([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request)
        {
            Kendo.Mvc.UI.DataSourceResult result = null;

            var item = await _repository.GetAsync();

            result = item.Where(x => x.ParentId == 0 || x.ParentId == x.Id).ToDataSourceResult(request, o => new MenuViewModel
            {
                Url = o.Url,
                Text = o.Text,
                Body = o.Body,
                MenuSection = o.MenuSection,
                Id = o.Id,
                SortOrder = o.SortOrder,
                ParentId = o.ParentId,
                SeoKeywords = o.SeoKeywords,
                SeoDescription = o.SeoDescription,
                BodyEng = o.BodyEng,
                HasChildMenu = _repository.Menues.Where(x => x.ParentId == o.Id).Any(),
                CustomField=o.CustomField
                
                //MenuSectionList = new List<SelectListItem> {
                //    new SelectListItem{ Text="Первый раздел",Value="0"},
                //    new SelectListItem{ Text="Второй раздел",Value="1"}
                //}


            });
            //var path = Path.Combine(_env.ContentRootPath, "app_offline.htm");
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}
            return Json(result);
        }
        [HttpPost]
        [Area("Admin")]
        public ActionResult Editing_Create([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, MenuViewModel cat)
        {

            var results = new List<MenuViewModel>();

            if (cat != null && ModelState.IsValid)
            {
                var item = new Menu();
                item.Url = cat.Url;
                item.Text = cat.Text;
                item.Body = cat.Body;
                item.BodyEng = cat.BodyEng;
                item.IsPublish = cat.IsPublish;
                item.MenuSection = cat.MenuSection;
                item.ParentId = cat.ParentId;
                item.SeoKeywords = item.SeoKeywords;
                item.SeoDescription = item.SeoDescription;
                item.CustomField = cat.CustomField;



                _repository.Create(item);

                //var path = Path.Combine(_env.ContentRootPath, "app_offline.htm");
                //if (!System.IO.File.Exists(path))
                //{
                //    System.IO.File.Create(path).Dispose();
                //}

                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}
                //cat.Id = _repository.Get().FirstOrDefault().Id;
                //results.Add(cat);
            }


            return Json(ModelState.ToDataSourceResult());//results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Editing_Update([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, MenuViewModel cat)
        {
            if (cat != null && ModelState.IsValid)
            {
                var item = await _repository.GetAsync(cat.Id);
                item.Url = cat.Url;
                item.Text = cat.Text;
                item.Body = cat.Body;
                item.BodyEng = cat.BodyEng;
                item.IsPublish = cat.IsPublish;
                item.MenuSection = cat.MenuSection;
                item.ParentId = cat.ParentId;
                item.SeoKeywords = cat.SeoKeywords;
                item.SeoDescription = cat.SeoDescription;

                _repository.Edit(item);
                //var path = Path.Combine(_env.ContentRootPath, "app_offline.htm");
                //if (!System.IO.File.Exists(path))
                //{
                //    System.IO.File.Create(path).Dispose();
                //}

                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Editing_Destroy([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, MenuViewModel cat)
        {
            if (cat != null)
            {
                var item = await _repository.GetAsync(cat.Id);

                _repository.Delete(item);
            }


            return Json(ModelState.ToDataSourceResult());
        }


        #region Ext

        public async Task<string> GetMenuName(int? id)
        {
            int newId = id ?? 0;


            if (newId != 0)
            {
                var item = await _repository.GetAsync(newId);
                return item.Text;
            }
            return "";

        }

        //public static string GetMenuUrl(int? id)
        //{
        //    int newId = id ?? 0;


        //    if (newId != 0)
        //    {
        //        var item = _repository.Get(newId);
        //        return item.Url;
        //    }
        //    return "";

        //}
        public enum NotificationMessage
        {
            Create,
            Edit,
            Delete,
            Error
        }
        #endregion
    }

}
