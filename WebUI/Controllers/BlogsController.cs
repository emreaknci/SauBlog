using Business.Abstract;
using Core.Helpers;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWriterService _writerService;


        public BlogsController(IBlogService blogService, IWriterService writerService)
        {
            _blogService = blogService;
            _writerService = writerService;
        }

        [HttpGet("[action]")]
        public IActionResult Detail(int id = 0)
        {
            return View();
        }

        public IActionResult Add()
        {
            var list = _writerService.GetAll().Data;
            ViewBag.writers = list;
            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(BlogForCreateViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var imagePath= FileHelper.Upload(viewModel.Image);
            BlogForCreateDto dto = new()
            {
                ImagePath = imagePath.Data,
                Content = viewModel.Content,
                Title = viewModel.Title,
                WriterId = viewModel.WriterId
            };
            var result = await _blogService.AddAsync(dto);

            return RedirectToAction("Index", "Home");
        }

    }
}
