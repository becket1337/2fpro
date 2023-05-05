using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class MakeOrderViewComponent : ViewComponent
    {
        IProductRepository _prod;
        public MakeOrderViewComponent(IProductRepository prod)
        {
            _prod = prod;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Quan = 1, int Id = 0)
        {

            var getMask = await _prod.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == Id);
            ViewBag.price = getMask.Price * Quan;
            ViewBag.Quan = Quan;
            ViewBag.Prod = getMask;
            return View();
        }
    }
}
