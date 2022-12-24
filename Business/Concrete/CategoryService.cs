using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.Category;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IDataResult<Category>> GetByIdWithBlogs(int id)
        {
            var category = await _categoryDal.Table.Include(c=>c.Blogs).FirstOrDefaultAsync(b=>b.Id==id);
            if (category!=null)
            {
                return new SuccessDataResult<Category>(category);
            }
            return new ErrorDataResult<Category>("Category Bulunamadı");
        }

        public async Task<IDataResult<List<Category>>> GetByList(List<int> ids)
        {
            var categories = new List<Category>();
            foreach (var id in ids)
                categories.Add((await _categoryDal.GetByIdAsync(id))!);
            return new SuccessDataResult<List<Category>>(categories);
        }
        public async Task<IDataResult<Category>> DeleteAsync(int id)
        {
            var category = await _categoryDal.GetByIdAsync(id);

            if (category != null)
            {
                _categoryDal.Remove(category);
                await _categoryDal.SaveAsync();
                return new SuccessDataResult<Category>(category, "Kategori Silindi");
            }
            return new ErrorDataResult<Category>("Kategori Bulunamadı");
        }

        public IDataResult<List<Category>> GetAll()
        {
            var list = _categoryDal.GetAll().ToList();
            return new SuccessDataResult<List<Category>>(list);
        }

        public async Task<IDataResult<Category>> GetById(int id)
        {
            var category = await _categoryDal.GetByIdAsync(id);

            if (category != null)
            {
                return new SuccessDataResult<Category>(category);
            }
            return new ErrorDataResult<Category>("Category Bulunamadı");
        }

        public async Task<IDataResult<Category>> UpdateAsync(CategoryForUpdateDto dto)
        {
            var category = await _categoryDal.GetByIdAsync(dto.Id);
            if (category != null)
            {
                category.Name = dto.Name;
                _categoryDal.Update(category);
                await _categoryDal.SaveAsync();
                return new SuccessDataResult<Category>(category, "Kategori Güncellendi");
            }
            return new ErrorDataResult<Category>("Kategori Bulunamadı");
        }

        public IDataResult<List<Category>> GetAllWithBlogs()
        {
            var categories = _categoryDal.GetAll().Include(x => x.Blogs).ToList();

            return new SuccessDataResult<List<Category>>(categories);
        }
    }
}
