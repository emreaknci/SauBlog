using Core.Entities;

namespace Entities.Concrete;

public class Comment : BaseEntity
{
    public string? Content { get; set; }
    public int? WriterId { get; set; }
    public Writer? Writer { get; set; }
    public int? BlogId { get; set; }
    public Blog? Blog { get; set; }
}