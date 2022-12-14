using Business.Abstract;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

                return View(dto);
            }
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
