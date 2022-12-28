using Business.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Admins()
        {
            TempData["roleName"] = "Admin";
            var result = _roleService.GetWithUsersByName("Admin").Result;
            TempData["url"] = HttpContext.Request.Path.Value;
            return View(result.Data);
        }
    }
}
