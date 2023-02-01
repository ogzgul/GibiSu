using GibiSu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GibiSu.Data;
using Microsoft.EntityFrameworkCore;

namespace GibiSu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (roleManager.FindByNameAsync("Administrator").Result == null)
            {
                IdentityRole identityRole = new IdentityRole("Administrator");
                roleManager.CreateAsync(identityRole).Wait();

                var user = new ApplicationUser();
                
                user.UserName = "Kralperen";
                user.Email = "kralperen@gmail.com";
                user.Name = "Kralp";
                user.Password = "Hfdgjs15!+";
                user.ConfirmPassword = "Hfdgjs15!+";
                user.Agreed = true;
                user.Address = "gslfdkgs";
                user.PhoneNumber = "5445";

                var chkUser = userManager.CreateAsync(user,user.Password).Result;


                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRoleAsync(user, "Administrator").Result;

                }

            }

            _logger = logger;
        }

		public IActionResult Index()
		{

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Admin()
        {
			//var applicationDbContext = _context.Pages.Include(p => p.Menu);
			//Page page = _context.Pages.Include(p => p.Contents.OrderBy(c => c.Order)).Where(d => d.Url == "Index").FirstOrDefault();
			//return View(page);
			return View();
			
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}