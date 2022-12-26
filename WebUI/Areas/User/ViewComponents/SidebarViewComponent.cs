using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public SidebarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        private Core.Entities.User CurrentUser()
        {
            var email = HttpContext.User.Claims.ToList()[1].Value;
            var user = _userService.GetUserByMailAsync(email).Result.Data;
            return user;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.userFullName = $"{CurrentUser().FirstName} {CurrentUser().LastName}";
            return View();
        }
    }
}
