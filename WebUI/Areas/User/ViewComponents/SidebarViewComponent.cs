using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
    
            return View();
        }
    }
}
