using Business.Abstract;
using Core.Helpers;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using WebUI.Models;
using X.PagedList;

namespace WebUI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWriterService _writerService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;


        public BlogsController(IBlogService blogService, IWriterService writerService, ICategoryService categoryService,
            IToastNotification toastNotification)
        {
            _blogService = blogService;
            _writerService = writerService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        [HttpGet("[action]")]
        public IActionResult Detail(int id)
        {
            var result = _blogService.GetByIdWithCommentsAsync(id).Result.Data;

            return View(result);
        }



        [HttpGet]

        [Authorize(Roles = "Admin,User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Add()
        {
            var writers = _writerService.GetAll().Data;
            ViewData["writers"] = writers;
            var categories = _categoryService.GetAll().Data;
            ViewData["categories"] = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BlogForCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = _categoryService.GetAll().Data;
                ViewData["categories"] = categories;
                var writers = _writerService.GetAll().Data;
                ViewData["writers"] = writers;
                return View(dto);
            }

            var result = await _blogService.AddAsync(dto);
            _toastNotification.AddSuccessToastMessage("Blog başarıyla eklendi!");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Update(int id)
        {
            var blog = _blogService.GetByIdWithCategories(id).Result.Data;
            var blogCategories = blog.Categories?.ToList();
            var blogCategoryIds = new List<int>();
            blogCategories?.ForEach(i => blogCategoryIds.Add(i.Id));

            var categories = _categoryService.GetAll().Data;
            var writers = _writerService.GetAll().Data;
            ViewBag.writers = writers!; List<CategoryListItem> categoryListItems = new();
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
        public async Task<IActionResult> Update(BlogForUpdateDto dto)
        {
            dto.CurrentImagePath = (string)TempData["currentImagePath"]!;
            var result = await _blogService.UpdateAsync(dto);
            _toastNotification.AddSuccessToastMessage("Blog başarıyla güncellendi!");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult List(int currentPageNo = 1, int size = 2)
        {
            //var result = _blogService.GetWithPaginate(currentPageNo - 1, size);
            var result = _blogService.GetAll();
            var pagedlist = result.Data.ToPagedList(currentPageNo - 1, size);

            return View(pagedlist);
        }
        [HttpGet("Blogs/Category{categoryId}")]
        public IActionResult BlogsByCategoryId(int categoryId)
        {
            var result = _blogService.GetAllByCategoryId(categoryId);
            if (!result.Success)
            {
                _toastNotification.AddInfoToastMessage(result.Message);
                return RedirectToAction("Index","Home");
            }
            return View(result.Data);
        }
    }
    public class CategoryListItem
    {
        public Category? Category { get; set; }
        public bool Checked { get; set; }
    }
}
