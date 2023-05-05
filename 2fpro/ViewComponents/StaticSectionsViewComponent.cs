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
    public class StaticSectionsViewComponent : ViewComponent
    {
        IStaticSectionRepository _postR;
        public StaticSectionsViewComponent(IStaticSectionRepository postR)
        {
            _postR = postR;
        }
        public async Task<IViewComponentResult> InvokeAsync(int type = 1)
        {
            var item = await _postR.StaticSections.Where(x => x.ID == type).FirstOrDefaultAsync();
            if (item == null) item = new StaticSection() { Title = "", Content = "qq" };
            return View(item);
        }
    }
}
