using Business.Abstract;
using Business.Concrete;
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
        private IToastNotification _toastNotification;

        public CommentsController(ICommentService commentService, IToastNotification toastNotification)
        {
            _commentService = commentService;
            _toastNotification = toastNotification;
        }
        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            var list = _commentService.GetAllForListing().Data;
            return View(list);
        }
        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id)
        {
            var result = _commentService.DeleteAsync(id).Result;
            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);

            return RedirectToAction("Index", "Comments", new { Area = "User" });
        }
    }
}
