using _2fpro.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _2fpro.Controllers
{

    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signIn;
        public AccountController(SignInManager<ApplicationUser> signIn)
        {
            _signIn = signIn;

        }
        public async Task<IActionResult> LogoutVisitor(string username)
        {
            await _signIn.SignOutAsync();
            return Redirect("/");
        }


    }
}