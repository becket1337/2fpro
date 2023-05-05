//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using _2fpro.Service.Interface;
//using _2fpro.Service.Repository;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity.EntityFramework;
//using _2fpro.Models;


//namespace _2fpro.Areas.Admin.Controllers
//{
//    public class UsersController : Controller
//    {
//        //
//        // GET: /Admin/Users/

//        public UsersController() { }

//        public UsersController(ApplicationUserManager userManager)
//        {
//            UserManager = userManager;
//        }
//        private ApplicationUserManager _userManager;

//        public ApplicationUserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }


//        //public async Task<ActionResult> Index()
//        //{
//        //    var manager = await UserManager.FindByIdAsync(User.Identity.GetUserId());
//        //    string currentUserId = User.Identity.GetUserId();
//        //    return View(manager.g);
//        //}
//        //public static async Task<string> IsOnline(string username) {
//        //    //IUserRepository _db = new UsersRepository();
//        //    var manager = await UserManager.FindByIdAsync(User.Identity.GetUserId());
//        //    string currentUserId = User.Identity.GetUserId();

//        //    return "user";
//        //}

//        //protected ApplicationUserManager UserManagerCreate() {
//        //    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();;
//        //    return userManager;
//        //}
//    }
//}
