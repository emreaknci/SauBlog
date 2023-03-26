using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Entities.DTOs.Comment;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Entities.DTOs.Category;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.Concrete
{
    public class CommentDal : EfBaseRepository<Comment, BlogDbContext>, ICommentDal
    {
        public CommentDal(BlogDbContext context) : base(context)
        {
        }
        public IQueryable<CommentForListDto>? GetAllForListing(Expression<Func<CommentForListDto, bool>>? filter = null, bool tracking = true)
        {
            IQueryable<CommentForListDto> result = null!;
            if (!tracking)
                result = result.AsNoTracking();
            result = GetAll().Include(c => c.Blog)
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

            return filter == null ? result : result.Where(filter);
        }

        public (List<CommentForListDto> entities, int totalCount) GetWithPagination(
            CommentForPaginationRequest request, bool tracking = true, Expression<Func<CommentForListDto, bool>>? filter = null)
        {
            IQueryable<CommentForListDto> result = null!;
            if (!tracking)
                result = result.AsNoTracking();
            var comments = GetAll();
            var query = CheckIfRequestParams(comments, request);

            if (!request.WriterId.Equals(null))
                query = query.Where(c => c.WriterId==request.WriterId);


            result = query.Include(c => c.Blog)
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
                 }); ;
           
            result = filter == null
                ? result
                : result.Where(filter);
            return (result.Skip(request.Index * request.Size).Take(request.Size).ToList()
                , comments.Count());
        }
    }
}
