using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.Controllers;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Controllers
{

    public class DynamicController : BaseController
    {
        //
        // GET: /Dynamic/

        IMenuRepository _rep;
        public DynamicController(IMenuRepository rep)
        {
            _rep = rep;
        }

        [ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "routes" })]
        [Route("pages/{url}", Name = "PageIndex")]
        public ActionResult Indexx(string url)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            var str = _rep.Menues.FirstOrDefault(x => x.Url == url);
            if (str == null) return NotFound();
            return View(str);
        }



    }
}
