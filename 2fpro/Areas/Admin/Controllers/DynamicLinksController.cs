using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using _2fpro.Controllers;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Areas.Admin.Controllers
{
    public class DynamicLinksController : BaseController
    {
        //
        // GET: /Admin/DynamicLinks/
        private IMenuRepository _rep;
        private IOrderRepository _orep;
        public DynamicLinksController(IMenuRepository rep, IOrderRepository orep)
        {
            _rep = rep;
            _orep = orep;
        }

        public ActionResult ViewPage(string urlType, string culture)
        {

            Menu item = null;
            item = (culture != "ru" ? _rep.Menues.Where(x => x.Url == urlType && x.MenuSection > 1).SingleOrDefault() :
                _rep.Menues.Where(x => x.Url == urlType && x.MenuSection <= 1).SingleOrDefault());

            if (urlType == "MakeOrder" || urlType == "Portfolio")
            {
                switch (urlType)
                {
                    case ("MakeOrder"):
                        return PartialView("MakeOrder");

                    case ("Portfolio"):
                        return PartialView("Portfolio");

                }
            }
            return Json(new
            {
                Body = (culture == "ru" ? item.Body : item.BodyEng),
                viewType = (item.MenuSection == 0 ? "roller" : ""),
                url = urlType
            });
        }

        public ActionResult MakeOrder()
        {

            return PartialView(new MakeOrderViewModel());
        }

        public ActionResult Portfolio()
        {
            return PartialView();
        }



    }
}
