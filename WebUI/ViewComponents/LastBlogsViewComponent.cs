using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class LastBlogsViewComponent : ViewComponent
{
    private readonly IBlogService _blogService;

    public LastBlogsViewComponent(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public IViewComponentResult Invoke()
    {
        var result = _blogService.GetLastBlogs(3);

        return View(result.Data);
    }
}