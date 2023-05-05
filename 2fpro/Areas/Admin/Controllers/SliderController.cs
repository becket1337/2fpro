using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using _2fpro.Controllers;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _2fpro.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        //
        // GET: /Admin/Slider/

        ISliderRepository _db;

        public SliderController(ISliderRepository repository)
        {
            _db = repository;

        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //dfg
            return View(_db.getAll().OrderBy(x => x.Sortindex));
        }
        [ResponseCache(Duration = 180)]
        public ActionResult GetSlider()
        {
            var list = _db.getAll();
            return PartialView(list);
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Portfolio folio, IFormFile file)
        {
            if (file.Length > 4000000) return StatusCode(413);
            if (folio != null)
            {
                await _db.Create(folio, file);
                return RedirectToAction("Index");
            }
            else return View();

        }

        [Area("Admin")]
        public async Task<IActionResult> DeleteAll()
        {
            await _db.DeleteAll();
            return RedirectToAction("Index");
        }
        public JsonResult GetData()
        {
            var list = _db.getAll().Select(x => new Portfolio
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                ProdLink = x.ProdLink
            }).ToArray();
            return Json(new { dataList = list });
        }
        public JsonResult GetSlide(int id)
        {
            var item = _db.GetPortfolio(id);
            return Json(new { title = item.Title, prodlink = item.ProdLink, desc = item.Description, price = item.Price });
        }

        public static int GetFirstSlide(ISliderRepository db)
        {

            return db.getAll().First().ID;
        }


        [Area("Admin")]
        public ActionResult Edit(int id)
        {

            var item = _db.GetPortfolio(id);
            return View(item);
        }
        [HttpPost]
        [Area("Admin")]
        public async Task<IActionResult> Edit(Portfolio folio, IFormFile file)
        {


            if (ModelState.IsValid)
            {

                await _db.Edit(folio, file);
                return RedirectToAction("Index");
            }
            else return View();
        }
        [Area("Admin")]
        public ActionResult Delete(int id)
        {
            var item = _db.GetPortfolio(id);
            _db.Delete(item);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                await _db.UpdateSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return Json("ok");
        }
    }
}
