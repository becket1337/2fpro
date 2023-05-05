using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Models;

namespace _2fpro.ViewComponents
{
    public class LastNewsViewComponent : ViewComponent
    {
        IPostRepository _postR;
        public LastNewsViewComponent(IPostRepository postR)
        {
            _postR = postR;
        }
        public async Task<IViewComponentResult> InvokeAsync(bool isPartial = false)
        {
            List<Post> items = null;
            if (isPartial)
                items = await _postR.Posts.OrderBy(x => x.CreatedAt).ToListAsync();
            else
                items = await _postR.Posts.OrderBy(x => x.CreatedAt).Take(3).ToListAsync();
            return View(items);
        }
    }
}
