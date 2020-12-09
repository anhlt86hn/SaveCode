using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RicoCore.Areas.Admin.Models;
using RicoCore.Data.Entities.System;
using RicoCore.Utilities.Dtos;
using System.Threading.Tasks;

namespace RicoCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model)
        {
            //returnUrl = Url.Content("~/Admin/Home") ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã đăng nhập");
                    //return LocalRedirect("~/Admin/Home");
                    return new OkObjectResult(new GenericResult(true));
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("~/Account/LoginWith2fa", new { ReturnUrl = "~/Admin/Home", RememberMe = model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản đã bị khóa");
                    //return RedirectToPage("~/Account/Lockout");
                    return new ObjectResult(new GenericResult(false, "Tài khoản đã bị khoá"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //return View();
                    return new ObjectResult(new GenericResult(false, "Đăng nhập sai"));
                }
            }

            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(false, model));
        }
    }
}