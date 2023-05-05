using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Areas.Identity.Pages.Account;
using _2fpro.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _2fpro.Areas.Admin.Controllers
{

    public class AuthSecurityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public AuthSecurityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminLogout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"пользователь не найден '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Пользователь не найден'{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            ViewBag.StatusMessage = "Пароль успешно изменен.";

            return View();
        }
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListOfProfiles()
        {
            var list = await _userManager.Users.ToListAsync();
            return View(list);
        }
    }


    public class InputModel
    {
        [Required(ErrorMessage = "Пустое поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Пустое поле")]
        [StringLength(100, ErrorMessage = "минимум 4 символа", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повтор нового пароля")]
        [Compare("NewPassword", ErrorMessage = "новый пароль не удовлетворяет требованиям")]
        public string ConfirmPassword { get; set; }
    }
}