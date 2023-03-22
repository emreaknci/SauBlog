using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs.Blog;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace DataAccess.Concrete
{
    public class BlogDal : EfBaseRepository<Blog, BlogDbContext>, IBlogDal
    {
        public BlogDal(BlogDbContext context) : base(context)
        {
        }


        public (List<BlogForListDto> list, int totalCount) GetWithPagination(BlogForPaginationRequest req, bool tracking = true,
            Expression<Func<BlogForListDto, bool>>? filter = null)
        {

            var query = GetAll().Include(b => b.Categories)
                .Include(b => b.Comments)
                .Include(b => b.Writer).AsQueryable();


            query = CheckIfRequestParams(query, req);

            if (req.CategoryIds != null)
                query = query.Where(blog => blog.Categories!.Any(category => req.CategoryIds.Contains(category.Id)));

            if (req.WriterIds != null)
                query = query.Where(blog => req.WriterIds.Contains((int)blog.WriterId));


            var result = query.Select(b => new BlogForListDto()
            {
                Id = b.Id,
                ImagePath = b.ImagePath,
                Title = b.Title,
                CreatedDate = b.CreatedDate,
                CommentCount = b.Comments!.Count,
                WriterNickName = b.Writer!.NickName,
                WriterId = b.Writer.Id,
                Status = b.Status
            }).AsQueryable();
            var list = result.Skip(req.Index * req.Size).Take(req.Size);
            int count = result.Count();

            if (filter == null) return (list.ToList(), count);

            list = list.Where(filter);
            count = list.Count();
            return (list.ToList(), count);
        }

        public List<Blog>? GetAllByCategoryId(int categoryId)
        {
            var result = GetAll().Include(b => b.Categories).ToList();
            var list = new List<Blog>();
            foreach (var blog in result)
                foreach (var category in blog.Categories)
                    if (category.Id == categoryId)
                        list.Add(blog);
            return list;
        }
    }
}
