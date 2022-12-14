using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class BlogsSidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}