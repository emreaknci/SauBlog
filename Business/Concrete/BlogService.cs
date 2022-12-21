using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class BlogService : IBlogService
    {
        private readonly IBlogDal _blogDal;
        private readonly ICategoryService _categoryService;

        public BlogService(IBlogDal blogDal, ICategoryService categoryService)
        {
            _blogDal = blogDal;
            _categoryService = categoryService;
        }

        public async Task<IDataResult<Blog>> AddAsync(BlogForCreateDto dto)
        {
            var imagePath = FileHelper.Upload(dto.Image);
            var result = await _categoryService.GetByList(dto.CategoryIds);
            var newBlog = new Blog()
            {
                ImagePath = imagePath.Data,
                WriterId = dto.WriterId,
                Content = dto.Content,
                Title = dto.Title,
                Categories = result.Data
            };
            await _blogDal.AddAsync(newBlog);
            await _blogDal.SaveAsync();
            return new SuccessDataResult<Blog>(newBlog);
        }

        public async Task<IDataResult<Blog>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Blog>> UpdateAsync(BlogForUpdateDto dto)
        {

            var result = await _categoryService.GetByList(dto.CategoryIds);
            //m-t-m iliskide bagı koparmak icin include etmemiz lazım ef core otomatik karsilastiriyor yoksa ugras dur
            var blog = await _blogDal.Table.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == dto.Id);

            if (dto.NewImage != null)
            {
                var imagePath = FileHelper.Update(dto.NewImage!, dto.CurrentImagePath!);
                blog.ImagePath = imagePath.Data;
            }
            else
                blog.ImagePath = dto.CurrentImagePath;

            blog.Content = dto.Content;
            blog.WriterId = dto.WriterId;
            blog.Title = dto.Title;
            blog.Categories = result.Data;

            _blogDal.Update(blog);
            await _blogDal.SaveAsync();

            return new SuccessDataResult<Blog>(blog);
        }

        public IDataResult<List<Blog>> GetAll()
        {
            var list = _blogDal.GetAll().ToList();
            return new SuccessDataResult<List<Blog>>(list);
        }

        public async Task<IDataResult<Blog>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Blog>> GetByIdWithCategories(int id)
        {
            var blog = await _blogDal.Table.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
                return new ErrorDataResult<Blog>();
            return new SuccessDataResult<Blog>(blog);
        }
    }
}
