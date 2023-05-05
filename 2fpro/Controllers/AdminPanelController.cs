using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Controllers
{
    [AllowAnonymous]
    public class AdminPanelController : Controller
    {
        SignInManager<ApplicationUser> signIn;
        public AdminPanelController(SignInManager<ApplicationUser> signin)
        {
            signIn = signin;
        }

        [ResponseCache(NoStore = true)]
        [HttpGet(Name = "adminlogin")]
        public async Task<IActionResult> Gate()
        {
            if (HttpContext.User.IsInRole("Anonym"))
                await signIn.SignOutAsync();
            return Redirect("/Identity/Account/AdminLogin");
        }
    }
}