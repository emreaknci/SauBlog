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
using Microsoft.IdentityModel.Tokens;
using Entities.DTOs.Blog;

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
            var newCategory = new Category
            {
                Name = dto.Name.ToUpper()
            };
            await _categoryDal.AddAsync(newCategory);
            await _categoryDal.SaveAsync();
            return new SuccessDataResult<Category>(newCategory);
        }

        public async Task<IDataResult<Category>> GetByIdWithBlogs(int id)
        {
            var category = await _categoryDal.Table.Include(c => c.Blogs).FirstOrDefaultAsync(b => b.Id == id);
            if (category != null)
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

        public IPaginateResult<Category> GetWithPaginate(int index, int size, string? filter)
        { 
            var data = filter.IsNullOrEmpty() ? _categoryDal.GetWithPagination(index, size)
                : _categoryDal.GetWithPagination(index, size, filter: i => i.Name!.Contains(filter!.ToUpper()));

            if (index * size > data.totalCount)
                return new ErrorPaginationResult<Category>("Hata!");

            return new SuccessPaginationResult<Category>(index, size, data.entities, data.totalCount);
        }



        public async Task<IDataResult<List<CategoryForListDto>>> GetListWithBlogCount()
        {
            var categories = _categoryDal.Table.Include(c => c.Blogs).ToList();
            if (categories.IsNullOrEmpty()) return new ErrorDataResult<List<CategoryForListDto>>("Kategori Bulunamadı");

            return new SuccessDataResult<List<CategoryForListDto>>(categories
                .Select(i => new CategoryForListDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    BlogCount = i.Blogs.Count
                })
                .ToList());
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
                category.Name = dto.Name.ToUpper();
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
