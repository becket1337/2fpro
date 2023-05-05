
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize(Roles = "Admin")]
    public class UploadFilesController : Controller
    {
        //
        // GET: /Admin/UploadFiles/
        private IFileManager manager;
        IHostingEnvironment _env;

        public const int FilesPerPage = 3;

        public UploadFilesController(IHostingEnvironment env, IFileManager _manager)
        {
            _env = env;
            manager = _manager;
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1, string folder = null)
        {

            var mainDirPath = Path.Combine(_env.WebRootPath, "Content/Files/Pages");
            if (!Directory.Exists(mainDirPath)) manager.CheckDirectory(mainDirPath);
            var mainFolder = new DirectoryInfo(mainDirPath);
            var defaultFolder = mainFolder.GetDirectories().Any()
                ? mainFolder.GetDirectories().FirstOrDefault().Name :
                string.Empty;

            if (folder == null) folder = defaultFolder;
            FilesViewModel vm = new FilesViewModel
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = manager.GetDir(folder).GetFiles().Count(),
                    CurrentPage = page,
                    ItemsPerPage = FilesPerPage,
                    Service = "files",
                    Dir = manager.GetDir(folder),
                    Condition = false

                },
                Files = manager.GetDir(folder).GetFiles().Skip((page - 1) * FilesPerPage).Take(FilesPerPage),
                Dirs = manager.GetDirs()

            };

            return View(vm);
        }



        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult GetFiles(int page = 1, string folder = null)
        {

            return ViewComponent("GetFiles", new { page = page, folder = folder });
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public ActionResult GetFolders()
        {

            return ViewComponent("GetFolders");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateDirectory(string name)
        {
            var path = Path.Combine(_env.WebRootPath, "Content/Files/Pages/" + name);
            manager.CreateDir(path);
            TempData["message"] = "папка создана";
            TempData["type"] = 1;
            return Json("");
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveDirectory(string name)
        {
            var path = Path.Combine(_env.WebRootPath, "Content/Files/Pages/" + name);
            manager.RemoveDir(path);
            TempData["message"] = "папка удалена";
            TempData["type"] = 1;
            return Json("");
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public ActionResult Save(List<IFormFile> files, string folder)
        {
            // The Name of the Upload component is "files"/
            if (string.IsNullOrWhiteSpace(folder)) return Json(new { type = "zero" });
            // Some browsers send file names with full path.
            if (files != null)
            {
                manager.WriteFiles(files, folder);


                return Json(new { type = "success", count = files.Count(), mb = files.Sum(x => x.Length) / 1000 * 1000, folderr = folder });
            }

            // Return an empty string to signify success
            return Json(new { type = "error", folderr = folder });
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public JsonResult CreateDir(string name)
        {

            manager.CreateDir(name);

            return Json("");
        }

        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public JsonResult GetDirs()
        {
            return Json(manager.GetDirs());
        }
        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string fileName, string dirname)
        {

            var file = Path.GetFileName(fileName);
            var physicalPath = Path.Combine(_env.WebRootPath, "Content/Files/Pages/" + dirname + "/" + file);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
                Dispose();

            }
            return Json("");
        }
        public ActionResult ContentRemove()
        {
            var path = Path.Combine(_env.WebRootPath, "Content/Files");
            var dir = new DirectoryInfo(path);
            if (dir.GetDirectories().Any())
            {
                foreach (var item in dir.GetDirectories())
                {
                    item.Delete(true);
                }
            }
            return Redirect("/Admin");
        }
        [Area("Admin")]
        public ActionResult Cancel(string fileName)
        {


            var file = Path.GetFileName(fileName);
            var physicalPath = Path.Combine(_env.WebRootPath, "Content/Files/" + file);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);

            }

            return Json("");
        }
        [Area("Admin")]
        [HttpPost]
        public ActionResult DelAll()
        {

            manager.RemoveDir(Path.Combine(_env.WebRootPath, "Content/Files/Pages"));
            return RedirectToAction("Index");
        }
        [Area("Admin")]
        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(_env.WebRootPath, "Content/Files/" + fileName);



                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                        Dispose();
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

    }
}
