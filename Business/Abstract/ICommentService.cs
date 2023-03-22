using Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs.Comment;
using System.Linq.Expressions;
using Entities.DTOs.Category;

namespace Business.Abstract
{
    public interface ICommentService
    {
        Task<IDataResult<Comment>> AddAsync(CommentForCreateDto dto);
        Task<IResult> RemoveAsync(int id);
        Task<IResult> RemoveRangeAsync(List<Comment> comments);

        IDataResult<List<Comment>> GetAllByBlogId(int id);
        IDataResult<List<Comment>> GetAll();
        IDataResult<List<Comment>> GetAllByWriterId(int writerId);
        IDataResult<List<CommentForListDto>> GetAllForListing(Expression<Func<CommentForListDto, bool>>? filter = null);
        IDataResult<List<Comment>> GetAllWithWriterByBlogId(int id);
        IPaginateResult<CommentForListDto> GetWithPaginate(CommentForPaginationRequest request);

    }
}
