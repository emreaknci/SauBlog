using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CategoryListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {List<Category> categories = new()
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" },
            new() { Id = 4, Name = "D" },
            new() { Id = 5, Name = "E" },
            new() { Id = 6, Name = "F" },
        };
        return View(categories);
    }
}