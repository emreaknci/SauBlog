using Business.Abstract;
using Business.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IToastNotification _toastNotification; 
        private readonly IWriterService _writerService;

        public CommentsController(ICommentService commentService, IToastNotification toastNotification, IWriterService writerService)
        {
            _commentService = commentService;
            _toastNotification = toastNotification;
            _writerService = writerService;
        }

        private Writer GetCurrentWriter()
        {
            var writer = _writerService.GetByUserId(Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value)).Result.Data;
            return writer!;
        }

        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            var list = _commentService.GetAllForListing().Data;
            return View(list);
        }

        [HttpGet]
        public IActionResult Delete(int id, string previousUrl)
        {
            var result = _commentService.DeleteAsync(id).Result;
            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);

            return Redirect(previousUrl);
        }

        [HttpGet]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult MyComments()
        {
            var list = _commentService.GetAllForListing(c=>c.WriterId==GetCurrentWriter().Id).Data;
            return View(list);
        }

    }
}
