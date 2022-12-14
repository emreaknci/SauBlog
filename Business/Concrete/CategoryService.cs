using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryService : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryService(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task<IDataResult<Category>> AddAsync(CategoryForCreateDto dto)
        {
            var newCategory = new Category()
            {
                Name = dto.Name,
            };
            await _categoryDal.AddAsync(newCategory);
            await _categoryDal.SaveAsync();
            return new SuccessDataResult<Category>(newCategory);
        }

        public Task<IDataResult<Category>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Category>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<Category>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<Category>> GetWithCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<Category>> UpdateAsync(CategoryForUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
