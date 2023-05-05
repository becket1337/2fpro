using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.ViewModel;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace _2fpro.Areas.Admin.Controllers
{

    public class StaticSectionController : Controller
    {
        //
        // GET: /Admin/StaticSection/
        IStaticSectionRepository _rep;

        public StaticSectionController(IStaticSectionRepository rep)
        {
            _rep = rep;
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;

            SectionsViewModel vm = new SectionsViewModel
            {
                StaticSections = _rep.StaticSections
                  .OrderBy(x => x.ID)
                  .ToList()
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _rep.StaticSections.Count(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize

                }
            };
            return View(vm);

        }
        [ResponseCache(Duration = 30)]
        public JsonResult GetSections()
        {
            var items = _rep.StaticSections
                .Select(x => new
                {
                    ID = x.ID,
                    Content = WebUtility.HtmlDecode(x.Content)
                }).ToArray();
            return Json(new { sections = items });
        }
        [Area("Admin")]
        public ActionResult Delete(int id, int cpage = 1)
        {
            var item = _rep.Get(id);
            _rep.Delete(item);
            TempData["message"] = "Статический блок удален";
            TempData["type"] = 1;
            return LocalRedirect("/Admin/StaticSection/Index?page=" + cpage);
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, int cpage = 1)
        {
            ViewBag.page = cpage;
            return View(_rep.Get(id));
        }
        [HttpPost]
        [Area("Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StaticSection model, int cpage = 1)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = "Статический блок изменен";
                TempData["type"] = 1;
                _rep.Edit(model);
                return LocalRedirect("/Admin/StaticSection/Index?page=" + cpage);
            }
            else return View();
        }

        public ActionResult GetSection(SectionType? type)
        {
            switch (type)
            {
                case SectionType.TopQuote:
                    return PartialView(_rep.GetSection(1));
                case SectionType.BottomQuote:
                    return PartialView(_rep.GetSection(2));
                case SectionType.Phone:
                    return PartialView(_rep.GetSection(3));
                case SectionType.Counter:
                    return PartialView(_rep.GetSection(4));
                case SectionType.FirstSection:
                    return PartialView(_rep.GetSection(5));
                case SectionType.SecondSection:
                    return PartialView(_rep.GetSection(5));
                default:
                    return Content("");
            }
        }


        public enum SectionType
        {
            TopQuote,
            BottomQuote,
            Phone,
            FirstSection,
            SecondSection,
            Counter
        }
    }
}
