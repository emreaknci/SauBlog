using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.Comment;
using System.Linq.Expressions;
using Entities.DTOs.Category;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IRepository<Comment>
    {
        IQueryable<CommentForListDto>? GetAllForListing(Expression<Func<CommentForListDto, bool>>? filter = null,
            bool tracking = true);

        (List<CommentForListDto> entities, int totalCount) GetWithPagination(CommentForPaginationRequest request,
            bool tracking = true,
            Expression<Func<CommentForListDto, bool>>? filter = null);
    }
}
