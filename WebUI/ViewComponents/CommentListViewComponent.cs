using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CommentListViewComponent : ViewComponent
{
    ICommentService _commentService;

    public CommentListViewComponent(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public IViewComponentResult Invoke(int blogId)
    {
        var comments = _commentService.GetAllWithWriterByBlogId(blogId).Data;
        ViewBag.comments= comments;
        return View();
    }
}