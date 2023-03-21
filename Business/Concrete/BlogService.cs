using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Business.Abstract;
using Core.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Business.Concrete
{
    public class BlogService : IBlogService
    {
        private readonly IBlogDal _blogDal;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;

        public BlogService(IBlogDal blogDal, ICategoryService categoryService, ICommentService commentService)
        {
            _blogDal = blogDal;
            _categoryService = categoryService;
            _commentService = commentService;
        }

        public async Task<IDataResult<Blog>> AddAsync(BlogForCreateDto dto)
        {
            var imagePath = dto.Image != null
                ? FileHelper.Upload(dto.Image).Data
                : !string.IsNullOrEmpty(dto.ImagePath) ? dto.ImagePath : null;
            var result = await _categoryService.GetByList(dto.CategoryIds);
            var newBlog = new Blog()
            {
                ImagePath = imagePath,
                WriterId = dto.WriterId,
                Content = dto.Content,
                Title = dto.Title,
                Categories = result.Data
            };
            await _blogDal.AddAsync(newBlog);
            await _blogDal.SaveAsync();
            return new SuccessDataResult<Blog>(newBlog,"Blog eklendi");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            var blog = await _blogDal.Table.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == id);

            if (blog != null)
            {
                FileHelper.Delete(blog.ImagePath);
                if (!blog.Comments.IsNullOrEmpty())
                    await _commentService.RemoveRangeAsync(blog.Comments!.ToList());
                _blogDal.Remove(blog);
                await _blogDal.SaveAsync();
                return new SuccessResult("Blog Silindi");
            }
            return new ErrorResult("Blog Bulunamadı");
        }

        public async Task<IResult> RemoveRangeAsync(List<Blog> blogs)
        {
            _blogDal.RemoveRange(blogs);
            await _blogDal.SaveAsync();
            return new SuccessResult("Bloglar silindi");
        }

        public async Task<IDataResult<Blog>> UpdateAsync(BlogForUpdateDto dto)
        {

            var result = await _categoryService.GetByList(dto.CategoryIds);
            //m-t-m iliskide bagı koparmak icin include etmemiz lazım ef core otomatik karsilastiriyor yoksa ugras dur
            var blog = await _blogDal.Table.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == dto.Id);

            if (dto.NewImage != null)
            {
                if (!dto.CurrentImagePath.Contains("DefaultBlogPng"))
                {
                    var imagePath = FileHelper.Update(dto.NewImage!, dto.CurrentImagePath!);
                    blog.ImagePath = imagePath.Data;
                }
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

        public IPaginateResult<BlogForListDto> GetWithPaginate(int index, int size, string? filter)
        {
            var data = filter.IsNullOrEmpty()
                ? _blogDal.GetWithPagination(index, size)
                : _blogDal.GetWithPagination(index, size, filter: i => i.Title!.Contains(filter!.ToUpper()));

            if (index * size > data.totalCount)
                return new ErrorPaginationResult<BlogForListDto>("Hata!");

            data.list.ForEach(i =>
            {
                if (string.IsNullOrEmpty(i.ImagePath))
                    i.ImagePath = "DefaultBlogPng.png";
            });
            return new SuccessPaginationResult<BlogForListDto>(index, size, data.list, data.totalCount);
        }

        public IDataResult<List<Blog>> GetAll()
        {
            var list = _blogDal.GetAll().ToList();
            if (list.Count > 0)
            {
                list.ForEach(i =>
                {
                    if (string.IsNullOrEmpty(i.ImagePath))
                        i.ImagePath = "DefaultBlogPng.png";
                });
                return new SuccessDataResult<List<Blog>>(list);
            }
            else return new ErrorDataResult<List<Blog>>("Blog Bulunamadı");
        }

        public IDataResult<List<Blog>> GetAllByWriterId(int writerId)
        {
            var list = _blogDal.GetAll(b => b.WriterId == writerId).ToList();

            if (list.Count <= 0)
                return new ErrorDataResult<List<Blog>>(null, "Bu yazara ait blog bulunumadı");
            list.ForEach(i =>
            {
                if (string.IsNullOrEmpty(i.ImagePath))
                    i.ImagePath = "DefaultBlogPng.png";
            });
            return new SuccessDataResult<List<Blog>>(list);
        }

        public IDataResult<List<Blog>> GetAllByCategoryId(int categroyId)
        {
            var list = _blogDal.GetAllByCategoryId(categroyId);
            if (list.Count <= 0)
                return new ErrorDataResult<List<Blog>>(null, "Bu kategoriye ait blog bulunumadı");
            list.ForEach(i =>
            {
                if (string.IsNullOrEmpty(i.ImagePath))
                    i.ImagePath = "DefaultBlogPng.png";
            });
            return new SuccessDataResult<List<Blog>>(list);

        }

        public async Task<IDataResult<Blog>> GetById(int id)
        {
            var blog = await _blogDal.GetByIdAsync(id);

            if (blog != null)
            {
                if (string.IsNullOrEmpty(blog.ImagePath))
                    blog.ImagePath = "DefaultBlogPng.png";
                return new SuccessDataResult<Blog>(blog);
            }
            return new ErrorDataResult<Blog>("Blog Bulunamadı");
        }

        public async Task<IDataResult<Blog>> GetByIdWithWriter(int id)
        {
            var blog = await _blogDal.Table.Include(b => b.Writer).FirstOrDefaultAsync(b => b.Id == id);

            if (blog != null)
            {
                if (string.IsNullOrEmpty(blog.ImagePath))
                    blog.ImagePath = "DefaultBlogPng.png";
                return new SuccessDataResult<Blog>(blog);
            }
            return new ErrorDataResult<Blog>("Blog Bulunamadı");
        }

        public async Task<IDataResult<Blog>> GetByIdWithCategories(int id)
        {
            var blog = await _blogDal.Table.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
                return new ErrorDataResult<Blog>();
            if (string.IsNullOrEmpty(blog.ImagePath))
                blog.ImagePath = "DefaultBlogPng.png";
            return new SuccessDataResult<Blog>(blog);
        }

        public IDataResult<List<LastBlogDto>> GetLastBlogs(int count)
        {
            //var blogs = _blogDal.GetAll().Include(b => b.Comments).OrderByDescending(b => b.Id).ToList().Take(count).ToList();
            var blogs = _blogDal.GetAll();

            if (!blogs.Any()) return new ErrorDataResult<List<LastBlogDto>>("Blog Bulunamadı");

            var lastBlogs = blogs.Include(b => b.Comments)
                .OrderByDescending(b => b.Id).ToList().Take(count).ToList()
                .Select(i => new LastBlogDto
                {
                    Id = i.Id,
                    Title = i.Title!,
                    CreatedDate = i.CreatedDate,
                    CommentCount = i.Comments!.Count
                })
                .ToList();
            return new SuccessDataResult<List<LastBlogDto>>(lastBlogs);

        }

        public async Task<IDataResult<Blog>> GetByIdWithCommentsAsync(int id)
        {
            var blog = await _blogDal.Table.Include(b => b.Comments!).ThenInclude(b => b.Writer).FirstOrDefaultAsync(b => b.Id == id);

            if (blog != null)
            {
                if (string.IsNullOrEmpty(blog.ImagePath))
                    blog.ImagePath = "DefaultBlogPng.png";
                return new SuccessDataResult<Blog>(blog);
            }
            return new ErrorDataResult<Blog>("Blog Bulunamadı");
        }
    }
}
