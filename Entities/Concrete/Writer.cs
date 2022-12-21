using Core.Entities;

namespace Entities.Concrete;

public class Writer : BaseEntity
{
    public Writer()
    {
        Blogs = new HashSet<Blog>();
        Comments = new HashSet<Comment>();
    }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Blog>? Blogs { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public string NickName { get; set; }
}