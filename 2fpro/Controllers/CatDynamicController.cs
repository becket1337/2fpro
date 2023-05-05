using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Controllers
{
    public class CatDynamicController : Controller
    {
        //
        // GET: /CatDynamic/

        ICategoryRepository _rep;
        public CatDynamicController(ICategoryRepository rep)
        {
            _rep = rep;
        }

        public ActionResult Index(int id)
        {
            var str = _rep.Categories.Single(x => x.ID == id);
            ViewBag.Body = str.CatDescription;
            return View(str);
        }

    }
}
