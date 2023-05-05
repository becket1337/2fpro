
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using _2fpro.Data;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/
        IPostRepository _postCtx;
        IMenuRepository _menuCtx;
        IProductRepository _prodCtx;
        ICategoryRepository _catCtx;
        IOrderRepository _orderCtx;

        public AdminController(
            IPostRepository postCtx,
            IMenuRepository menuCtx,
            IProductRepository prodCtx,
            ICategoryRepository catCtx,
            IOrderRepository orderCtx
            )
        {
            _postCtx = postCtx;
            _menuCtx = menuCtx;
            _prodCtx = prodCtx;
            _catCtx = catCtx;
            _orderCtx = orderCtx;
        }


        public async Task<IActionResult> Index()
        {

            var input = HttpContext.User.Identity.Name;
            var name = Regex.Replace(input, @"@\w+.\w+", " ");
            ViewBag.regex = name;
            return View(
                new AdminManageViewModel
                {
                    cats = await _catCtx.Categories.ToListAsync(),
                    prods = await _prodCtx.Products.ToListAsync(),
                    menus = await _menuCtx.Menues.ToListAsync(),
                    orders = await _orderCtx.Orders.ToListAsync(),
                    posts = await _postCtx.Posts.ToListAsync()

                });
        }

        #region -- Robots() Method --
        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
        #endregion


    }
}
