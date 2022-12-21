using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}