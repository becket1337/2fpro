using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Extension;

namespace _2fpro.ViewComponents
{
    public class LastnewProdsViewComponent : ViewComponent
    {
        IProductRepository _prodRep;
        IDistributedCache _cache;
        public LastnewProdsViewComponent(IProductRepository prodRep, IDistributedCache cache)
        {
            _prodRep = prodRep;
            _cache = cache;
        }


        public async Task<IViewComponentResult> InvokeAsync(int id = 0, int prodid = 0)
        {


            SessionStorage session = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                session = await _cache.GetAsync<SessionStorage>(HttpContext.User.Identity.Name);
            }
            if (session == null)
            {

                var lastLogged = await _cache.GetAsync("lastLogged");
                var encLogged = System.Text.Encoding.UTF8.GetString(lastLogged);
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
            List<Product> prod = new List<Product>();


            if (id != 0 && await _prodRep.Products.AnyAsync())
                prod = await _prodRep.Products.Include("ProdImages").Where(x => !x.DoShow).Where(x => x.ID != prodid && x.CategoryID == id).OrderBy(x => x.Sortindex).Take(10).ToListAsync();
            else
                prod = _prodRep.Products.Count() > 15 ? await _prodRep.Products.Where(x => !x.DoShow).Include("ProdImages").OrderBy(x => x.Sortindex).Take(15).ToListAsync() : await _prodRep.Products.Where(x => !x.DoShow).Include("ProdImages").OrderBy(x => x.Sortindex).ToListAsync();

            return View(new ProductListViewModel() { Products = prod, CategoryID = id });

        }
    }
}
