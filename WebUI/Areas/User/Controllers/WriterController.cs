using Business.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class WriterController : Controller
    {
        private readonly IWriterService _writerService;
        private IToastNotification _toastNotification;

        public WriterController(IWriterService writerService, IToastNotification toastNotification)
        {
            _writerService = writerService;
            _toastNotification = toastNotification;
        }
        [HttpGet("Detail")]
        public IActionResult GetUserById(int id)
        {
            var result= _writerService.GetByIdWithUserInfoAsync(id).Result;
            if (result.Success)
            {
                _toastNotification.AddErrorToastMessage(result.Message);
                return RedirectToAction("Index", "Home", new { Area = "User" });
            }
            return View(result.Data);
        }

        public IActionResult Index()
        {
            var result = _writerService.GetAllWithUserInfo();
            if (result.Success) return View(result.Data);
            _toastNotification.AddErrorToastMessage(result.Message);
            return RedirectToAction("Index", "Home", new { Area = "User" });
        }
    }
}
