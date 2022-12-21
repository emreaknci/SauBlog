using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class BlogsController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
    }
}
