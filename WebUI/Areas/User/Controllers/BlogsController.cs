using Business.Abstract;
using Business.Concrete;
using Core.Entities;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using WebUI.Controllers;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWriterService _writerService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;

        public BlogsController(IBlogService blogService, IWriterService writerService, ICategoryService categoryService, IToastNotification toastNotification)
        {
            _blogService = blogService;
            _writerService = writerService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }
        private  Writer GetCurrentWriter()
        {
            var writer = _writerService.GetByUserId(Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value)).Result.Data;
            return writer!;
        }

        [HttpGet]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Add()
        {
            ViewBag.writerId = GetCurrentWriter().Id;

            var categories = _categoryService.GetAll().Data;
            ViewData["categories"] = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Add(BlogForCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.writerId = GetCurrentWriter().Id;

                var categories = _categoryService.GetAll().Data;
                ViewData["categories"] = categories;
                return View(dto);
            }

            var result = _blogService.AddAsync(dto).Result;
            return RedirectToAction("MyBlogs", "Blogs", new { Area = "User" });

        }
        [HttpGet]
        public IActionResult Delete(int id, string previousUrl)
        {
            var result = _blogService.DeleteAsync(id).Result;
            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);

            return Redirect(previousUrl);
        }

        [HttpGet]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Update(int id)
        {
            var blog = _blogService.GetByIdWithCategories(id).Result.Data;
            var blogCategories = blog.Categories?.ToList();
            var blogCategoryIds = new List<int>();
            blogCategories?.ForEach(i => blogCategoryIds.Add(i.Id));

            var categories = _categoryService.GetAll().Data;
            List<CategoryListItem> categoryListItems = new();
            categories.ForEach(i => categoryListItems.Add(new()
            {
                Category = i,
                Checked = false
            }));
            categoryListItems.ForEach(i =>
            {
                if (blogCategoryIds.Contains(i.Category!.Id))
                {
                    i.Checked = true;
                }
            });
            ViewBag.categories = categoryListItems;
            TempData["id"] = blog.Id;
            TempData["currentImagePath"] = blog.ImagePath;
            BlogForUpdateDto dto = new()
            {
                Content = blog.Content,
                Title = blog.Title,
                WriterId = blog.WriterId,
                CurrentImagePath = blog.ImagePath,
            };
            return View(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(BlogForUpdateDto dto)
        {
            dto.CurrentImagePath = (string)TempData["currentImagePath"]!;
            var result = await _blogService.UpdateAsync(dto);
            _toastNotification.AddSuccessToastMessage("Blog başarıyla güncellendi!");
            return RedirectToAction("MyBlogs", "Blogs",new{Area="User"});
        }

        [HttpGet]
        [Authorize(Roles = "Writer", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult MyBlogs()
        {
            var list = _blogService.GetAllByWriterId(GetCurrentWriter().Id).Data;
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            var result = _blogService.GetAll();

            return View(result.Data);
        }
    }
    public class CategoryListItem
    {
        public Category? Category { get; set; }
        public bool Checked { get; set; }
    }
}
