using Core.Entities;

namespace Entities.Concrete;

public class Category : BaseEntity
{
    public Category()
    {
        Blogs = new HashSet<Blog>();
    }
    public string? Name { get; set; }
    public ICollection<Blog>? Blogs { get; set; }

}