using Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs.Comment;

namespace Business.Abstract
{
    public interface ICommentService
    {
        Task<IDataResult<Comment>> AddAsync(CommentForCreateDto dto);
        Task<IResult> DeleteAsync(int id);

        IDataResult<List<Comment>> GetAllByBlogId(int id);
    }
}
