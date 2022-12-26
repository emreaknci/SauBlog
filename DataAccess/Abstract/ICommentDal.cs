using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.Comment;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IRepository<Comment>
    {
        List<CommentForListDto>? GetAllForListing(Expression<Func<CommentForListDto, bool>>? filter = null,
            bool tracking = true);

    }
}
