using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.Comment;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IRepository<Comment>
    {
        List<CommentForListDto>? GetAllForListing();

    }
}
