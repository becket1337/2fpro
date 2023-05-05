using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace _2fpro.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AdminLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminLoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public string FormattedUserName { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "заполните поле логин")]
            [Display(Name = "Логин")]
            public string Email { get; set; }

            [Required(ErrorMessage = "заполните поле пароль")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync()
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await _signInManager.SignOutAsync();

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = "";

        }

        public async Task<IActionResult> OnPostAsync()
        {

            //returnUrl = returnUrl ?? Url.Content("~/");
            if (Input.Email.ToLower() == "admin")
                FormattedUserName = Input.Email + "@mail.com";
            else
                FormattedUserName = Input.Email;
            if (ModelState.IsValid)
            {
                var isRemember = Input.RememberMe;
                var user = await _userManager.FindByNameAsync(FormattedUserName);
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, true, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect("/Admin");
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильные логин или пароль");
                    return Page();
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}