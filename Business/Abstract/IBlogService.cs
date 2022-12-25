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
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<Blog>> UpdateAsync(BlogForUpdateDto dto);
        IPaginateResult<BlogForListDto> GetWithPaginate(int index, int size);

        IDataResult<List<Blog>> GetAll();
        IDataResult<List<Blog>> GetAllByCategoryId(int categroyId);
        IDataResult<List<Blog>> GetLastBlogs(int count);
        Task<IDataResult<Blog>> GetById(int id);
        Task<IDataResult<Blog>> GetByIdWithCategories(int id);
        Task<IDataResult<Blog>> GetByIdWithCommentsAsync(int id);
    }
}
