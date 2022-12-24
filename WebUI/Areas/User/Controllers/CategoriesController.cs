using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class CategoriesController : Controller
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var list = _categoryService.GetAll().Data;
            return View(list);
        }
    }
}
