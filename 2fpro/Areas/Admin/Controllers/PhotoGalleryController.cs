using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using _2fpro.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace _2fpro.Areas.Admin.Controllers
{
    public class PhotoGalleryController : Controller
    {
        //
        // GET: /Admin/PhotoGallery/

        IPhotoGallery _gRepo;

        public PhotoGalleryController(IPhotoGallery gRepo)
        {
            _gRepo = gRepo;
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;

            PhotoGalleryViewModel vm = new PhotoGalleryViewModel
            {
                Galleries = _gRepo.GalAll()
                    .OrderByDescending(x => x.ID)
                    .ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _gRepo.Galleries.Count(),
                    ItemsPerPage = pageSize,
                    CurrentPage = page
                },
                Images = _gRepo.Images.ToList()
            };

            return View(vm);

        }


        public FileContentResult GetPhoto(int id)
        {
            var item = _gRepo.GetImage(id);
            if (item != null)
            {

                return File(item.ImageData, item.ImageMimeType);
            }
            else return null;
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetPostImages(int pid)
        {

            return ViewComponent("GetImagesFromFolder");
        }
        public FileContentResult GetGalleryPhoto(int id)
        {
            var item = _gRepo.GetGallery(id);
            if (item != null)
            {

                return File(item.GalleryData, item.GalleryMimeType);
            }
            else return null;
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
        public ActionResult Create(Gallery gallery, IFormFile file, Image image = null, int galId = 0)
        {

            if (gallery == null)
            {
                if (ModelState.IsValid)
                {
                    _gRepo.Create(null, file, galId);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TempData["message"] = "Галлерея - " + gallery.GalleryTitle + " создана";
                    TempData["type"] = 1;
                    _gRepo.Create(gallery, file, 0);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {

            var item = _gRepo.GetGallery(id);

            return View(item);

        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageDetails(int id)
        {

            var item = _gRepo.GetGallery(id);

            return View(item);

        }
        public JsonResult Folder(int id)
        {
            List<int> arr = _gRepo.GetGallery(id).Images.Select(x => x.ID).ToList();
            return Json(new { list = arr });
        }
        public FileContentResult GetImage(int id)
        {
            Image image = _gRepo.GetImage(id);
            if (image != null)
            {
                return File(image.ImageData, image.ImageMimeType);
            }
            else { return null; }
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public JsonResult UpdateImage(int id, string title)
        {
            if (id != 0)
            {
                this._gRepo.UpdateImage(id, title);
            }
            return base.Json(true);
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadImages(IEnumerable<IFormFile> files, int galId)
        {
            string data = "";
            if (files != null)
            {
                foreach (IFormFile base2 in files)
                {
                    this._gRepo.Create(null, base2, galId);
                }
            }
            data = JsonConvert.SerializeObject((from x in this._gRepo.GetGallery(galId).Images
                                                orderby x.ID
                                                select x.ID).ToList<int>());
            return base.Json(data);
        }
        public ActionResult UploadStart(int galId)
        {
            ViewBag.GalId = galId;
            return PartialView();
        }

        public ActionResult DeleteImg(int id)
        {
            if (id != 0)
            {
                var item = _gRepo.GetImage(id);
                _gRepo.Delete(null, item);
                return Json(new { imgId = id });
            }
            return Content("");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteAll(int galid)
        {
            this._gRepo.DeleteAll(galid);
            return base.Json("");
        }

      
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var gal = _gRepo.GetGallery(id);
            if (gal.Images.Count() != 0)
            {
                foreach (var item in gal.Images.ToArray())
                {
                    _gRepo.Delete(null, item);
                }
            }
            TempData["message"] = "Галлерея - " + gal.GalleryTitle + " Удалена";
            TempData["type"] = 1;
            _gRepo.Delete(gal);
            return RedirectToAction("Index");


        }
        [HttpPost]
        public JsonResult EditSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                _gRepo.UpdateSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return base.Json("");
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, int page)
        {
            ViewBag.Page = page;
            var item = _gRepo.GetGallery(id);
            return View(item);
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Gallery gal, IFormFile file = null)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = "Галлерея - " + gal.GalleryTitle + " отредактированна";
                TempData["type"] = 1;
                _gRepo.Edit(gal, null, file);
            }
            return RedirectToAction("Index");
        }
    }
}
