using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int? statusCode)
        {
            switch (statusCode.Value)
            {
                case 404:
                    return RedirectToAction("Error404");
                    break;
            }
            return View("Error");
        }

        [HttpGet("404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
