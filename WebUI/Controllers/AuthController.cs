using System.Security.Claims;
using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using Core.Extensions;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        public AuthController(IAuthService authService, IToastNotification toastNotification, IUserService userService)
        {
            _authService = authService;
            _toastNotification = toastNotification;
            _userService = userService;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserForLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(dto);

                if (result.Success)
                {
                    var user = result.Data;
                    _toastNotification.AddSuccessToastMessage(result.Message);
                    var identity = new ClaimsIdentity(_userService.GetClaims(user!, user!.Roles!.ToList()),
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                }
                _toastNotification.AddErrorToastMessage("Giriş başarısız");
                return View(dto);
            }
            _toastNotification.AddErrorToastMessage("Eksik alanlar var");
            return View(dto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(WriterForRegisterDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterForWriterAsync(dto);
                if (result.Success)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message);
                    return RedirectToAction("Index", "Home");
                }
                _toastNotification.AddErrorToastMessage(result.Message);
            }
            _toastNotification.AddErrorToastMessage("Eksik alanlar var");
            return View(dto);
        }

    }
}
