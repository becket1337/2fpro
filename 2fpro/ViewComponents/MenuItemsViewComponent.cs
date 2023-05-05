using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class MenuItemsViewComponent : ViewComponent
    {

        private IMenuRepository _menuCtx;

        public MenuItemsViewComponent(IMenuRepository menuCtx)
        {
            _menuCtx = menuCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(int menuSection = 0)
        {
            var items = await _menuCtx.Menues.Where(x => x.MenuSection == 0).ToListAsync();
            ViewBag.MenuSec = menuSection;
            return View(items);
        }

    }
}
