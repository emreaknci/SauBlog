using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogService
    {
        Task<IDataResult<Blog>> AddAsync(BlogForCreateDto dto);
        Task<IResult> RemoveAsync(int id);
        Task<IResult> RemoveRangeAsync(List<Blog> blogs);
        Task<IDataResult<Blog>> UpdateAsync(BlogForUpdateDto dto);
        IPaginateResult<BlogForListDto> GetWithPaginate(BlogForPaginationRequest request);
        IDataResult<List<Blog>> GetAll();
        IDataResult<List<Blog>> GetAllByWriterId(int writerId);
        IDataResult<List<Blog>> GetAllByCategoryId(int categroyId);
        IDataResult<List<LastBlogDto>> GetLastBlogs(int count);
        Task<IDataResult<Blog>> GetById(int id);
        Task<IDataResult<Blog>> GetByIdWithWriter(int id);
        Task<IDataResult<Blog>> GetByIdWithCategories(int id);
        Task<IDataResult<Blog>> GetByIdWithCommentsAsync(int id);
    }
}
