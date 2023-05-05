using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetPostImagesViewComponent : ViewComponent
    {
        private IPostRepository _prodCtx;

        public GetPostImagesViewComponent(IPostRepository prodCtx)
        {
            _prodCtx = prodCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var prod = await _prodCtx.Posts.FirstOrDefaultAsync(x => x.ID == id);

            return View(prod);
        }
    }
}
