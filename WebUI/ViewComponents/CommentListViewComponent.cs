using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CommentListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Comment> comments)
    {
        return View(comments);
    }
}