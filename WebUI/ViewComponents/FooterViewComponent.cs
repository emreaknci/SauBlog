using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}