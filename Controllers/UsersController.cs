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

        public IActionResult Index(string search)
        {
            ViewData["Search"] = search;
            var user = from b in _signInManager.UserManager.Users
                       select b;
            List<ApplicationUser> members = new List<ApplicationUser>();
            foreach (ApplicationUser member in user)
            {
                if (_signInManager.UserManager.GetRolesAsync(member).Result.Count == 0)
                {
                    members.Add(member);
                }
            }
            if (!String.IsNullOrEmpty(search))
            {
                members = members.Where(x => x.Name.Contains(search) || x.Address.Contains(search) || x.Email.Contains(search) || x.PhoneNumber.Contains(search) || x.UserName.Contains(search)).ToList();

            }
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<PartialViewResult> UserCases(bool Deleted)
        {
            if (Deleted == true)
            {
            var userCases= _signInManager.UserManager.Users.Where(x => x.Deleted == true);
            return PartialView(await userCases.ToListAsync());

            }

            var userCasess = _signInManager.UserManager.Users.Where(x => x.Deleted == false);
            return PartialView(await userCasess.ToListAsync());
            

            


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName,Password,Email,ConfirmPassword,Agreed,Address,PhoneNumber")] ApplicationUser applicationUser)
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
                var users = _signInManager.UserManager.FindByNameAsync(userName).Result;
                if (users.Deleted == true)
                {
                    
                    return Redirect("~/");
                }

                identityResult = _signInManager.PasswordSignInAsync(userName, password, false, false).Result;
                
                if (identityResult.Succeeded == true)
                {
                        return Redirect("~/");
                }

            }

            return View();

        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _signInManager.UserManager.Users == null)
            {
                return NotFound();
            }

            var Users = await _signInManager.UserManager.FindByIdAsync(id);
            if (Users == null)
            {
                return NotFound();
            }
            return View(Users);
        }
        //POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_signInManager.UserManager.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pages'  is null.");
            }
            var Users = await _signInManager.UserManager.FindByIdAsync(id);
            if (Users != null)
            {
                Users.Deleted = true;
                await _signInManager.UserManager.UpdateAsync(Users);
            }

            return RedirectToAction(nameof(Index));
        }
        
    }

}
