using Business.Abstract;
using Entities.DTOs.Comment;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class CommentController : Controller
    {
        private readonly IWriterService _writerService;
        private readonly ICommentService _commentService;

        public CommentController(IWriterService writerService, ICommentService commentService)
        {
            _writerService = writerService;
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CommentForCreateDto dto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            var writerId = _writerService.GetByUserId(userId).Result.Data.Id;
            dto.WriterId = writerId;

            if (!ModelState.IsValid)
                return PartialView(dto);
            await _commentService.AddAsync(dto);
            return RedirectToAction("Detail", "Blogs", new { Area = "", id = dto.BlogId });
        }
    }
}
