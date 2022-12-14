using Business.Abstract;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IToastNotification _toastNotification;

        public AuthController(IAuthService authService, IToastNotification toastNotification)
        {
            _authService = authService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(dto);
                if (result.Success)
                {
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
            var result = await _authService.RegisterForWriterAsync(dto);
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(dto);
        }
    }
}
