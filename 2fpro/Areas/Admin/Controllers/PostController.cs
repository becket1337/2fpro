using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using _2fpro.Controllers;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Areas.Admin.Controllers
{

    public class PostController : Controller
    {
        IPostRepository _rep;

        public PostController(IPostRepository rep)
        {
            _rep = rep;
        }
        //
        // GET: /Admin/Post/
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;
            PostViewModel vm = new PostViewModel
            {
                Posts = _rep.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = _rep.Posts.Count(),

                    Condition = false,
                    Service = "post"

                }
            };
            return View(vm);
        }
        public async Task<IActionResult> Reviews(int page = 1)
        {
            int pageSize = 15;
            PostViewModel vm = new PostViewModel
            {
                Posts = _rep.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = await _rep.Posts.CountAsync(),

                    Condition = false,
                    Service = "post"

                }
            };
            return View(vm);

        }
        [ResponseCache(Duration = 60)]
        public ActionResult LastNews()
        {
            PostViewModel vm = new PostViewModel
            {
                Posts = _rep.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .Take(3)
            };
            return PartialView(vm);
        }
        //
        // GET: /Admin/Post/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null) return BadRequest("500");
            Post post = _rep.Get((int)id);
            if (post == null)
            {
                return BadRequest("400");
            }
            return View(post);
        }

        [Area("Admin")]
        public ActionResult ManageDetails(int? id)
        {
            if (id == null) return BadRequest("500");
            Post post = _rep.Get((int)id);
            if (post == null)
            {
                return BadRequest("400");
            }
            return View(post);
        }
        //
        // GET: /Admin/Post/Create
        [Area("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Post/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public ActionResult Create(Post post, IFormFile file)
        {
            if (ModelState.IsValid)
            {


                _rep.Create(post);


                TempData["message"] = "Статья опубликована";
                TempData["type"] = 1;
                return LocalRedirect("/Admin/Post/Index");
            }
            else
            {
                ModelState.AddModelError("", "заполните все поля");
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Edit/5              
        [Area("Admin")]
        public ActionResult Edit(int id = 0, int cpage = 1)
        {
            Post post = _rep.Get(id);
            ViewBag.page = cpage;
            if (post == null)
            {
                return BadRequest("400");
            }
            return View(post);
        }

        //
        // POST: /Admin/Post/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public ActionResult Edit(Post post, int cpage = 1)
        {

            if (ModelState.IsValid)
            {

                _rep.Edit(post);

                TempData["message"] = "Статья отредактирована";
                TempData["type"] = 1;
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }

            post.Body = System.Net.WebUtility.HtmlDecode(post.Body);
            post.Preview = System.Net.WebUtility.HtmlDecode(post.Preview);
            return View(post);
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

                    await _rep.SavePhoto(file, pid);

                    //list.Add(item);
                }
            }

            return Json(new { pimgid = pid });
        }

        [Area("Admin")]
        public ActionResult Delete(int id = 0, int cpage = 1)
        {
            Post post = _rep.Get(id);
            if (post != null)
            {
                ViewBag.page = cpage;
                var physicalPath = Path.Combine("~/Content/Files/Post/", id.ToString());
                _rep.RemoveDir(physicalPath);
                _rep.Delete(post);

                TempData["message"] = "Статья удалена";
                TempData["type"] = 1;
            }
            return LocalRedirect("/Admin/Post/Index?page=" + cpage);
        }
        [Area("Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetPostImages(int pid)
        {
            return ViewComponent("GetPostImages", new { id = pid });
        }
        //
        // POST: /Admin/Post/Delete/5
        [HttpPost]
        [Area("Admin")]
        public JsonResult DelPhoto(int pimgid)
        {
            var cat = _rep.Get(pimgid);
            _rep.PhotoDel(cat);
            return Json(new { id = pimgid });
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Area("Admin")]
        public ActionResult DeleteConfirmed(int id, int cpage = 1)
        {
            Post post = _rep.Get(id);
            _rep.Delete(post);
            TempData["message"] = "Статья удалена";
            TempData["type"] = 1;
            return LocalRedirect("/Admin/Post/Index?page=" + cpage);
        }


    }
}