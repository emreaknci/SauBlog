using Business.Abstract;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;
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
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.DeleteAsync(id).Result;
            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);

            return RedirectToAction("Index", "Categories", new{Area="User"});
        }
        [HttpGet]
        public IActionResult Update(int id)
        {

            var result = _categoryService.GetById(id).Result;
            if (!result.Success)
            {
                _toastNotification.AddErrorToastMessage(result.Message);
                return RedirectToAction("Index");
            }

            CategoryForUpdateDto dto = new()
            {
                Id = result.Data.Id,
                Name = result.Data.Name
            };
            return View(dto);
        }
        [HttpPost]
        public IActionResult Update(CategoryForUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = _categoryService.UpdateAsync(dto).Result;
            if (result.Success)
                return RedirectToAction("Index");

            _toastNotification.AddErrorToastMessage(result.Message);
            return View(dto);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(CategoryForCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = _categoryService.AddAsync(dto).Result;
            if (result.Success)
                return RedirectToAction("Index");

            _toastNotification.AddErrorToastMessage(result.Message);
            return View(dto);
        }
    }
}
