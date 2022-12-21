using Core.Entities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Category;
using Entities.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<Category>> AddAsync(CategoryForCreateDto dto);
        Task<IDataResult<Category>> DeleteAsync(int id);
        Task<IDataResult<Category>> UpdateAsync(CategoryForUpdateDto dto);

        IDataResult<List<Category>> GetAll();
        Task<IDataResult<Category>> GetById(int id);
        Task<IDataResult<Category>> GetByIdWithBlogs(int id);
        Task<IDataResult<List<Category>>> GetByList(List<int> ids);

    }
}
