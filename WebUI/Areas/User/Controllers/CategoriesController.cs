using Business.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]

    public class CategoriesController : Controller
    {
        private ICategoryService _categoryService;
        private IToastNotification _toastNotification;
        public CategoriesController(ICategoryService categoryService, IToastNotification toastNotification)
        {
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            var list = _categoryService.GetAll().Data;
            return View(list);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.DeleteAsync(id).Result;
            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);

            return RedirectToAction("Index");
        }
    }
}
