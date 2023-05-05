using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Areas.Admin.Controllers
{
    public class YaMarketController : Controller
    {
        private IYaMarketRepo _repo;
        private ICategoryRepository _crep;


        public YaMarketController(IYaMarketRepo repo, ICategoryRepository crep)
        {
            _repo = repo;
            _crep = crep;

        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string xmlLink = "yaMarketExport.xml";
            var uriScheme = HttpContext.Request.IsHttps || HttpContext.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps ? "https" : "http";
            ViewBag.YaLink = $"{uriScheme}://{HttpContext.Request.Host}/{xmlLink}";

            ViewBag.ExportDone = TempData["info"] != null ? TempData["info"].ToString() : "";
            ViewBag.ExportError = TempData["error"] != null ? TempData["error"].ToString() : "";

            var yam = await _repo.GetMarketProfile();
            return View(yam);
        }

        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> MarketExport(YaMakret yam)
        {

            try
            {
                await _repo.UpdateProfile(yam);
                await _repo.YamExport();
                TempData["info"] = "SUCCESS";
            }
            catch (Exception err)
            {

                TempData["error"] = err.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
