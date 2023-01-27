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
		private readonly ApplicationDbContext _context;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager , ApplicationDbContext context)
		{

			if(roleManager.FindByNameAsync("Administrator").Result == null) 
			{
                IdentityRole identityRole = new IdentityRole("Administrator");
                roleManager.CreateAsync(identityRole).Wait();
            }

			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{

            return View();
		}
		
		public IActionResult Privacy()
		{
			return View();
		}

		[Authorize(Roles ="Administrator")]
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