using Business.Abstract;
using Business.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class WriterController : Controller
    {
        private readonly IWriterService _writerService;
        private readonly IToastNotification _toastNotification;

        private Writer GetCurrentWriter()
        {
            var writer = _writerService.GetByUserId(Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value)).Result
                .Data;
            return writer!;
        }

        public WriterController(IWriterService writerService, IToastNotification toastNotification)
        {
            _writerService = writerService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Detail(int id)
        {
            var result = _writerService.GetByIdWithUserInfoAsync(id).Result;
            if (result.Success) return View(result.Data);

            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index", "Home", new { Area = "User" });
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            var result = _writerService.GetAllWithUserInfo();
            if (result.Success) return View(result.Data);
            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index", "Home", new { Area = "User" });
        }

        [HttpGet]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult EditNickName()
        {
            ViewBag.nickName = GetCurrentWriter().NickName;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EditNickName(string newNickName)
        {
            ViewBag.nickName = GetCurrentWriter().NickName;
            if (newNickName.IsNullOrEmpty())
            {
                ViewBag.nickName = GetCurrentWriter().NickName;
                return View();
            }
            await _writerService.ChangeNickNameAsync(((int)GetCurrentWriter().UserId!), newNickName);
            ViewBag.nickName = GetCurrentWriter().NickName;

            return View();
        }
    }
}
