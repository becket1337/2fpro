using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _2fpro.Areas.Admin.Controllers
{

    public class ConfigController : Controller
    {



        //
        // GET: /Admin/Config/
        private IConfigRepository _rep;
        private IMenuRepository _menu;
        private IProductRepository _cat;
        public ConfigController(IConfigRepository rep, IMenuRepository menu, IProductRepository cat)
        {
            _menu = menu;
            _rep = rep;
            _cat = cat;
        }
        public ActionResult GetPartials()
        {
            return PartialView();
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Change()
        {
            var item = _rep.Configs.First();
            return View(item);
        }
        [HttpPost]
        public ActionResult Abandon()
        {
            // Session.Abandon();
            //Session.Clear();
            return Json("");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Change(Config conf)
        {
            if (ModelState.IsValid)
            {
                await _rep.Edit(conf);
                TempData["message"] = "настройки сохранены";
                TempData["type"] = 1;
            }
            return Redirect("/Admin");
        }

        public ActionResult OfflineMess()
        {
            var item = _rep.Configs.First();
            return PartialView(item);
        }



        public async Task<Config> SiteOptions()
        {

            var item = await _rep.Options();
            return item;
        }



    }
}
