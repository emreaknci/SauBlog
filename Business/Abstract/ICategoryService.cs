using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Category;


namespace Business.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<Category>> AddAsync(CategoryForCreateDto dto);
        Task<IDataResult<Category>> DeleteAsync(int id);
        Task<IDataResult<Category>> UpdateAsync(CategoryForUpdateDto dto);

        IDataResult<List<Category>> GetAll();
        IDataResult<List<Category>> GetAllWithBlogs();
        Task<IDataResult<Category>> GetById(int id);
        Task<IDataResult<Category>> GetByIdWithBlogs(int id);
        Task<IDataResult<List<Category>>> GetByList(List<int> ids);
        IPaginateResult<Category> GetWithPaginate(int index, int size, string? filter);

        Task<IDataResult<List<CategoryForListDto>>> GetListWithBlogCount();

    }
}
