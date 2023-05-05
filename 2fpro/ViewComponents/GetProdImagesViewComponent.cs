using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetProdImagesViewComponent : ViewComponent
    {

        private IProductRepository _prodCtx;

        public GetProdImagesViewComponent(IProductRepository prodCtx)
        {
            _prodCtx = prodCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var prod = await _prodCtx.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == id);

            return View(prod);
        }

    }
}
