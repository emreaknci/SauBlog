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

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var result = _blogService.GetByIdWithWriter(id).Result.Data;
            ViewBag.blog=result;
            return View();
        }
        [HttpGet]
        public IActionResult List(int currentPageNo = 1, int size = 2)
        {
            //var result = _blogService.GetWithPagination(currentPageNo - 1, size);
            var result = _blogService.GetAll();
            var pagedlist = result.Data.ToPagedList(currentPageNo - 1, size);

            return View(pagedlist);
        }
        [HttpGet("Blogs/Category/{categoryId}")]
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
        [HttpGet("Blogs/Writer/{writerId}")]
        public IActionResult BlogsByWriterId(int writerId)
        {
            var result = _blogService.GetAllByWriterId(writerId);
            if (!result.Success)
            {
                _toastNotification.AddInfoToastMessage(result.Message);
                return RedirectToAction("Index", "Home");
            }
            return View(result.Data);
        }
    }
  
}
