using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class BlogDal : EfBaseRepository<Blog, BlogDbContext>, IBlogDal
    {
        public BlogDal(BlogDbContext context) : base(context)
        {
        }
    }
}
