using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Entities.DTOs.Comment;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class CommentDal : EfBaseRepository<Comment, BlogDbContext>, ICommentDal
    {
        public CommentDal(BlogDbContext context) : base(context)
        {
        }
        //var result = GetAll().Include(b => b.Categories)
        //    .Include(b => b.Comments)
        //    .Include(b => b.Writer)
        //    .Select(b => new BlogForListDto()
        //    {
        //        Id = b.Id,
        //        ImagePath = b.ImagePath,
        //        Title = b.Title,
        //        CreatedDate = b.CreatedDate,
        //        CommentCount = b.Comments!.Count,
        //        WriterNickName = b.Writer.NickName,
        //    });

        //var list = result.Skip(index * size).Take(size);
        //int count = result.Count();

        //    if (filter == null) return (list.ToList(), count);

        //list = list.Where(filter);
        //count = list.Count();
        //return (list.ToList(), count);
        public List<CommentForListDto>? GetAllForListing()
        {
            var result = GetAll().Include(c => c.Blog)
                .Include(c => c.Writer)
                .Select(c => new CommentForListDto()
                {
                    Id = c.Id,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    Content = c.Content,
                    BlogId = c.BlogId,
                    WriterId = c.WriterId,
                    Status = c.Status,
                    BlogTitle = c.Blog.Title,
                    WriterNickName = c.Writer.NickName

                });
            return result.ToList();
        }
    }
}
