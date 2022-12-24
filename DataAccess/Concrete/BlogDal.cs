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

namespace DataAccess.Concrete
{
    public class BlogDal : EfBaseRepository<Blog, BlogDbContext>, IBlogDal
    {
        public BlogDal(BlogDbContext context) : base(context)
        {
        }

        public (List<BlogForListDto> list, int totalCount) GetWithPagination(int index, int size, bool tracking = true, Expression<Func<BlogForListDto, bool>>? filter = null)
        {
            var result = GetAll().Include(b => b.Categories)
                .Include(b => b.Comments)
                .Include(b => b.Writer)
                .Select(b => new BlogForListDto()
                {
                    Id = b.Id,
                    ImagePath = b.ImagePath,
                    Title = b.Title,
                    CreatedDate = b.CreatedDate,
                    CommentCount = b.Comments!.Count,
                    WriterNickName = b.Writer.NickName,
                });

            var list = result.Skip(index * size).Take(size);
            int count = result.Count();

            if (filter == null) return (list.ToList(), count);

            list = list.Where(filter);
            count = list.Count();
            return (list.ToList(), count);
        }
    }
}
