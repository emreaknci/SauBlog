using Business.Abstract;
using Core.Entities;
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
        private readonly IToastNotification _toastNotification;

        public AuthController(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }
        private Core.Entities.User GetCurrentUser()
        {
            var user = _userService!.GetById(Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value)).Result.Data;
            return user!;
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewBag.userId = GetCurrentUser().Id;
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
