using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
