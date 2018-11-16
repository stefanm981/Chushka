using System.Linq;
using Chushka.Models;
using Chushka.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Chushka.Web.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ChushkaUser> signIn;
        private UserManager<ChushkaUser> userManager;

        public AccountController(SignInManager<ChushkaUser> signIn, UserManager<ChushkaUser> userManager)
        {
            this.signIn = signIn;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signIn.SignOutAsync().Wait();
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LogInInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var user = this.userManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (user == null) //user does not exists
            {
                return this.BadRequest("Invalid username or password.");
            }

            var result = this.signIn.PasswordSignInAsync(model.Username, model.Password, true, false).Result;

            if (result == SignInResult.Success) //successfully logged in
            {
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.BadRequest("Invalid username or password.");
            }  
        }

        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = new ChushkaUser
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    UserName = model.Username
                };

                var result = this.userManager.CreateAsync(user, model.Password).Result;

                if (this.userManager.Users.Count() == 1)
                {
                    var roleResult = this.userManager.AddToRoleAsync(user, "Administrator").Result;
                    if (roleResult.Errors.Any())
                    {
                        return this.View();
                    }
                }
                else
                {
                    var roleResult = this.userManager.AddToRoleAsync(user, "User").Result;
                    if (roleResult.Errors.Any())
                    {
                        return this.View();
                    }
                }

                if (result.Succeeded)
                {
                    this.signIn.SignInAsync(user, true).Wait();
                    return this.RedirectToAction("Index", "Home");
                }
            }

            return this.View();
        }
    }
}