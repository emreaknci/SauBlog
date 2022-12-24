using Business.Abstract;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWriterService _writerService;
        private readonly ICategoryService _categoryService;

        public BlogsController(IBlogService blogService, IWriterService writerService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _writerService = writerService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var userid = HttpContext.User.Claims.ToList()[0].Value;
            var writerId = _writerService.GetByUserId(Convert.ToInt32(userid)).Result.Data!.Id;
            ViewBag.writerId = writerId;

            var categories = _categoryService.GetAll().Data;
            ViewData["categories"] = categories;
            return View();
        }

        [HttpPost]
        public IActionResult Add(BlogForCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var userid = HttpContext.User.Claims.ToList()[0].Value;
                var writerId = _writerService.GetByUserId(Convert.ToInt32(userid)).Result.Data!.Id;
                ViewBag.writerId = writerId;
                var categories = _categoryService.GetAll().Data;
                ViewData["categories"] = categories;
                return View(dto);
            }

            var result = _blogService.AddAsync(dto).Result;
            return RedirectToAction("Index", "Home", new { Area = "User" });

        }
    }
}
