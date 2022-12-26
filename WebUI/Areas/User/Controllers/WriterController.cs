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
        private Writer CurrentWriter()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            var writer = _writerService.GetByUserId(userId).Result.Data;
            return writer!;
        }
        public WriterController(IWriterService writerService, IToastNotification toastNotification)
        {
            _writerService = writerService;
            _toastNotification = toastNotification;
        }
        [HttpGet("Detail")]
        public IActionResult GetUserById(int id)
        {
            var result = _writerService.GetByIdWithUserInfoAsync(id).Result;
            if (result.Success)
            {
                _toastNotification.AddErrorToastMessage(result.Message);
                return RedirectToAction("Index", "Home", new { Area = "User" });
            }
            return View(result.Data);
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
        public IActionResult EditNickName()
        {
            ViewBag.nickName = CurrentWriter().NickName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditNickName(string newNickName)
        {
            ViewBag.nickName = CurrentWriter().NickName;
            if (newNickName.IsNullOrEmpty())
            {
                ViewBag.nickName = CurrentWriter().NickName;
                return View();
            }
            await _writerService.ChangeNickNameAsync(((int)CurrentWriter().UserId!), newNickName);
            ViewBag.nickName = CurrentWriter().NickName;

            return View();
        }
    }
}
