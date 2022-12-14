using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class TrendingBlogsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}