using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CategoryListViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategoryListViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IViewComponentResult Invoke()
    {
        var result = _categoryService.GetAllWithBlogs();

        
        return View(result.Data);
    }
}