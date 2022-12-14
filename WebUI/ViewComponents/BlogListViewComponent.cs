using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class BlogListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}