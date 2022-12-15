using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class BlogsController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult Detail(int id = 0)
        {
            return View();
        }
    }
}
