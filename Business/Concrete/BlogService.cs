using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Blog;

namespace Business.Concrete
{
    public class BlogService : IBlogService
    {
        private readonly IBlogDal _blogDal;

        public BlogService(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public async Task<IDataResult<Blog>> AddAsync(BlogForCreateDto dto)
        {
            var newBlog = new Blog()
            {
                ImagePath = dto.ImagePath,
                WriterId = dto.WriterId,
                Content = dto.Content,
                Title = dto.Title,
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
            throw new NotImplementedException();
        }

        public IDataResult<List<Blog>> GetAll()
        {
            var list = _blogDal.GetAll().ToList();
            return new SuccessDataResult<List<Blog>>(list);
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Blog>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Blog>> GetByIdWithCategories(int id)
        {
            throw new NotImplementedException();
        }
    }
}
