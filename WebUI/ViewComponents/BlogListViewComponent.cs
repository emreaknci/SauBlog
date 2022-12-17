using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class BlogListViewComponent : ViewComponent
{
    private readonly IBlogService _blogService;

    public BlogListViewComponent(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public IViewComponentResult Invoke()
    {
        var list = _blogService.GetAll().Data;
        return View(list);
    }
}