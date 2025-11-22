using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels;
using GymMangementBLL.ViewModels.AccountViewModel;
using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProjectMangementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _acountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService acountService, SignInManager<ApplicationUser> signInManager)
        {
            _acountService = acountService;
            _signInManager = signInManager;
        }

        #region Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _acountService.ValidataUser(model);

            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }
            var Result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");

            if (Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Locked Out");

            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(model);



        }
        #endregion

        #region LogOut
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
