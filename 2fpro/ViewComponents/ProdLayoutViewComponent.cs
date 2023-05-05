using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Extension;
using _2fpro.Models;

namespace _2fpro.ViewComponents
{
    public class ProdLayoutViewComponent : ViewComponent
    {
        IProductRepository _prodRep;
        IDistributedCache _cache;
        public ProdLayoutViewComponent(IProductRepository prodRep, IDistributedCache cache)
        {
            _prodRep = prodRep;
            _cache = cache;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await _prodRep.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ProductName == "Медицинская маска");

            var lastLogged = await _cache.GetAsync("lastLogged");
            var encLogged = System.Text.Encoding.UTF8.GetString(lastLogged);
            SessionStorage session = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                session = await _cache.GetAsync<SessionStorage>(HttpContext.User.Identity.Name);
            }
            if (session == null)
            {


                session = await _cache.GetAsync<SessionStorage>(encLogged);
                if (session == null)
                {
                    session = new SessionStorage();
                    await _cache.SetAsync<SessionStorage>(encLogged, session);
                }
                ViewBag.GetCart = session;
            }
            else
            {
                ViewBag.GetCart = session;
            }
            return View(item);
        }
    }
}
