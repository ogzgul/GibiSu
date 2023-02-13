using GibiSu.Data;
using GibiSu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Web;

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
            var user = from b in _signInManager.UserManager.Users.OrderBy(c => c.Deleted == true)
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

        public IActionResult AdminList(string search)
        {
            ViewData["Search"] = search;
            var user = from b in _signInManager.UserManager.Users
                       select b;
            List<ApplicationUser> members = new List<ApplicationUser>();
            foreach (ApplicationUser member in user)
            {
                if (_signInManager.UserManager.GetRolesAsync(member).Result.Count == 1)
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
        public IActionResult CreateAdmin()
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
            List<ApplicationUser> members = new List<ApplicationUser>();
            foreach (ApplicationUser member in userCasess)
            {
                if (_signInManager.UserManager.GetRolesAsync(member).Result.Count == 0)
                {
                    members.Add(member);
                }
            }
            return PartialView(members);
            
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

        //Admin Create User Start
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Id,Name,UserName,Password,Email,ConfirmPassword,Agreed,Address,PhoneNumber")] ApplicationUser applicationUser)
        {
            ModelState.Remove("Orders");

            if (ModelState.IsValid)
            {
                _signInManager.UserManager.CreateAsync(applicationUser, applicationUser.Password).Wait();
                var result1 = _signInManager.UserManager.AddToRoleAsync(applicationUser, "Administrator").Result;
                return Redirect("/Users/AdminList");
            }
           
            return View(applicationUser);

        }
        //Admin Create User End

        public IActionResult Login(string ReturnUrl = null, string error = null)
        {
            ViewData["returnUrl"] = ReturnUrl;
            ViewData["LoginFlag"] = error;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Login(string userName, string password, string? returnUrl, string? error)
        {
            Microsoft.AspNetCore.Identity.SignInResult identityResult;

            if (ModelState.IsValid)
            {
                var users = _signInManager.UserManager.FindByNameAsync(userName).Result;
                if (users != null && users.Deleted == true )
                {
                    
                    Response.Redirect("/");
                }
                identityResult = _signInManager.PasswordSignInAsync(userName, password, false, false).Result;
                
                if (identityResult.Succeeded == true)
                {
                    if(_signInManager.UserManager.IsInRoleAsync(users, "Administrator").Result == true)
                    {
                        Response.Redirect("/Admin/Index");
                    }
                    else { 
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
                    }
                    //return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    string encodedUrl = HttpUtility.UrlEncode(returnUrl);

                    Response.Redirect("/Users/Login?error=Kullan%c4%b1c%c4%b1+Ad%c4%b1+veya+Parola+Hatal%c4%b1&ReturnUrl=" + encodedUrl);
                }
            }
            else
            {
                Response.Redirect("/Login");
            }
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

        public async Task<IActionResult> Edit(string id)
        {
            var Users = await _signInManager.UserManager.FindByIdAsync(id);
            return View(Users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("Id,Name,UserName,Password,Email,ConfirmPassword,Agreed,Address,PhoneNumber")] ApplicationUser applicationUser)
        {
            ModelState.Remove("Orders");
            
            var Users = await _signInManager.UserManager.FindByIdAsync(id);
            if (Users != null)
            {
                
                Users.Name = applicationUser.Name;
                Users.Address = applicationUser.Address;
                Users.UserName = applicationUser.UserName;
                Users.PhoneNumber = applicationUser.PhoneNumber;
                Users.PasswordHash = _signInManager.UserManager.PasswordHasher.HashPassword(applicationUser, applicationUser.Password);
                Users.Password = applicationUser.Password;
                Users.ConfirmPassword = applicationUser.ConfirmPassword;
                Users.Email = applicationUser.Email;
                await _signInManager.UserManager.UpdateAsync(Users);
            }


            return RedirectToAction(nameof(Index));

        }

        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> IsAdmin(string id)
        {
            
            var Users = await _signInManager.UserManager.FindByIdAsync(id);
            
            var result1 = _signInManager.UserManager.AddToRoleAsync(Users, "Administrator").Result;

            return Redirect("~/");
        }
    }

}
