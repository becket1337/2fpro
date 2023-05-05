using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetCatImageViewComponent : ViewComponent
    {
        private ICategoryRepository _prodCtx;

        public GetCatImageViewComponent(ICategoryRepository prodCtx)
        {
            _prodCtx = prodCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var prod = await _prodCtx.Categories.FirstOrDefaultAsync(x => x.ID == id);

            return View(prod);
        }
    }
}
