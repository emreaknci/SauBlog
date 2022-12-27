using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.ViewComponents;

public class AssignRoleViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public AssignRoleViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public IViewComponentResult Invoke()
    {
        var result = _userService.GetAllNonAdmin();
        ViewBag.users = result.Data;

        return View();
    }
}