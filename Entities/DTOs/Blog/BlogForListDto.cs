namespace Entities.DTOs.Blog
{
    public class BlogForListDto
    {
        public int Id { get; set; }
        public int CommentCount{ get; set; }
        public string? Title { get; set; }
        public string? ImagePath { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public string? WriterNickName { get; set; }
        public bool Status { get; set; }
    }
}
