using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CommentListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int blogId = 1)
    {
        return View();
    }
}