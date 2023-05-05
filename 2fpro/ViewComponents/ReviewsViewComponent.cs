using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class ReviewsViewComponent : ViewComponent
    {
        IPostRepository _postR;
        public ReviewsViewComponent(IPostRepository postR)
        {
            _postR = postR;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _postR.Posts.CountAsync() > 10 ? await _postR.Posts.OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync() : await _postR.Posts.ToListAsync();
            return View(list);
        }
    }
}
