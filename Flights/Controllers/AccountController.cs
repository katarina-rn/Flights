using Flights.Models;
using Flights.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Administrator"))
                    return RedirectToAction("Index", "Administrator");
                if (User.IsInRole("Agent"))
                    return RedirectToAction("Index", "Agent");
                if (User.IsInRole("Visitor"))
                    return RedirectToAction("Index", "Visitor");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(obj.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        if(roles[0] == "Administrator")
                            return RedirectToAction("Index", "Administrator");
                        if (roles[0] == "Agent")
                            return RedirectToAction("Index", "Agent");
                        if (roles[0] == "Posetilac")
                            return RedirectToAction("Index", "Visitor");
                    }
                    
                }
                ModelState.AddModelError("", "Neuspešno logvanje");
            }
            return View(obj);
        }

        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Flights.Helper.Helper.Administrator)
                .GetAwaiter()
                .GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Flights.Helper.Helper.Administrator));
                await _roleManager.CreateAsync(new IdentityRole(Flights.Helper.Helper.Agent));
                await _roleManager.CreateAsync(new IdentityRole(Flights.Helper.Helper.Posetilac));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM obj)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = obj.Email,
                    Email = obj.Email,
                    Name = obj.Name
                };

                var result = await _userManager.CreateAsync(user, obj.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, obj.RoleName);
                    return RedirectToAction("Index", "Administrator");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
