using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IWriterService _writerService;

        public SidebarViewComponent(IUserService userService, IWriterService writerService)
        {
            _userService = userService;
            _writerService = writerService;
        }

        public IViewComponentResult Invoke()
        {
            //var email = HttpContext.User.Claims.ToList()[1].Value;
            //var user = _userService.GetUserByMailAsync(email).Result.Data;
            //var writer = _writerService.GetByUserId(user.Id).Result.Data;

            //bool hasWriter = writer != null;
            //ViewBag.hasWriter = hasWriter;
            return View();
        }
    }
}
