using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                //create account
                var userModel = new AppUser();
                userModel.UserName = userVM.UserName;
                userModel.PasswordHash = userVM.Password;
                userModel.Address = userVM.Address;
                userModel.PhoneNumber = userVM.PhoneNumber;
                userModel.Email = userVM.Email;
                userModel.SSN = userVM.SSN;

                var result = await userManager.CreateAsync(userModel, userVM.Password);
                if (result.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(userModel, "User");
                    //creat cookie
                    await signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(userVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var userModel = await userManager.FindByNameAsync(userVM.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, userVM.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(userModel, userVM.RememberMe);
                        //return RedirectToAction("index", "Student");
                        return RedirectToAction("index","RentedCars");
                    }
                }
                ModelState.AddModelError("", "Username and password invalid");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }
    }
}