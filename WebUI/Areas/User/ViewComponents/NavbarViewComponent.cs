using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}