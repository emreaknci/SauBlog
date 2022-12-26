using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

    public class HomeController : Controller
    {
        private IToastNotification _toastNotification;

        public HomeController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            _toastNotification.AddSuccessToastMessage("Hoşgeldiniz");
            return View();
        }
    }
}
