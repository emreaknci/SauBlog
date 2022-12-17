namespace WebUI.Models
{
    public class BlogForCreateViewModel
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
        public int? WriterId { get; set; }
    }
}
