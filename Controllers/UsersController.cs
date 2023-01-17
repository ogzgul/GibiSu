using GibiSu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GibiSu.Controllers
{
    public class UsersController : Controller
    {

        SignInManager<ApplicationUser> _signInManager;

        public UsersController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(_signInManager.UserManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName,Password,Email,ConfirmPassword,Agreed,Address")] ApplicationUser applicationUser)
        {
            ModelState.Remove("Orders");

            if (ModelState.IsValid)
            {
                _signInManager.UserManager.CreateAsync(applicationUser, applicationUser.Password).Wait();
                return Redirect("~/");
            }

            return View(applicationUser);

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password)
        {
            Microsoft.AspNetCore.Identity.SignInResult identityResult;

            if (ModelState.IsValid)
            {
                identityResult = _signInManager.PasswordSignInAsync(userName, password, false, false).Result;

                if (identityResult.Succeeded == true)
                {
                    return Redirect("~/");
                }

            }

            return View();

        }

    }

}
