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
    public class CategoryDal : EfBaseRepository<Category, BlogDbContext>, ICategoryDal
    {
        public CategoryDal(BlogDbContext context) : base(context)
        {
        }
    }
}
