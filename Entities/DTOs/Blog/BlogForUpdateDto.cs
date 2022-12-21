using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.Blog;

public class BlogForUpdateDto
{
    public int Id { get; set; }

    public string? Title { get; set; }
    public string? Content { get; set; }
    public int? WriterId { get; set; }
    public List<int>? CategoryIds { get; set; }
    public string? CurrentImagePath { get; set; }
    public IFormFile? NewImage { get; set; }

}