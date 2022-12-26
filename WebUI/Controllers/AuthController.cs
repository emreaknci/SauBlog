using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using WebUI.Models;
using IResult = Core.Utilities.Results.IResult;

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
        public IActionResult LogIn(string? returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
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
                    if (TempData["returnUrl"] == null)
                        return RedirectToAction("Index", "Home");
                    return Redirect((string)TempData["returnUrl"]);
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
                return View(dto);
            }
            _toastNotification.AddErrorToastMessage("Eksik alanlar var");
            return View(dto);
        }

        [HttpGet]
        public PartialViewResult ForgotPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            using HttpClient httpClient = new HttpClient();
            var response = httpClient.PostAsync($"https://localhost:7144/api/Auth/SendResetPassword?email={email}", null).Result;
            var content = await response.Content.ReadAsStringAsync();
            // Deserialize the JSON response
            var responseResult = JsonConvert.DeserializeObject<Response>(content);
            if (response.IsSuccessStatusCode)
            {
                // Read the response content

                if (responseResult!.Success)
                {
                    _toastNotification.AddSuccessToastMessage(responseResult.Message);
                    return RedirectToAction("Index", "Home");
                }
                _toastNotification.AddErrorToastMessage(responseResult.Message);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(responseResult.Message);
            }
            return RedirectToAction("LogIn", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string? resetPasswordToken)
        {
            ViewContext context = new ViewContext();
            resetPasswordToken = (string)RouteData.Values["id"]!;

            var result = await _authService.VerifyResetTokenAsync(resetPasswordToken!);
            if (!result.Success)
            {
                _toastNotification.AddErrorToastMessage(result.Message);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.userId = result.Data.UserId;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var result = await _userService.ResetPasswordAsync(viewModel.UserId, viewModel.Password);
            if (!result.Success)
                _toastNotification.AddErrorToastMessage(result.Message);
            _toastNotification.AddSuccessToastMessage(result.Message);

            return RedirectToAction("Index", "Home");
        }
    }

    class Response
    {
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
