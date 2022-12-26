using Business.Abstract;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using WebUI.Areas.User.Models;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private IToastNotification _toastNotification;

        public AuthController(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var id = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            ViewBag.userId = id;
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            ViewBag.userId = model.UserId;

            if (!ModelState.IsValid)
            {
                ViewBag.userId = model.UserId;
                return View(model);
            }

            UserForChangePasswordDto dto = new()
            {
                UserId = model.UserId,
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword,
            };
            var result = _userService.ChangePasswordAsync(dto).Result;
            if (result.Success)
            {
                _toastNotification.AddSuccessToastMessage(result.Message);
                return RedirectToAction("LogOut", "Auth",new{Area=""});
            }
            _toastNotification.AddErrorToastMessage(result.Message);
            return View(model);
        }
    }
}
