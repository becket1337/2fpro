using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{

    public class CatItemsViewComponent : ViewComponent
    {

        private ICategoryRepository _catCtx;

        public CatItemsViewComponent(ICategoryRepository catCtx)
        {
            _catCtx = catCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool ismain=true)
        {
            ViewBag.IsMain = ismain;
            var items = await _catCtx.Categories.Include(x => x.Products).Where(x => !x.DoShow).OrderBy(x => x.Sortindex).ToListAsync();
            return View(items);
        }

    }
}
