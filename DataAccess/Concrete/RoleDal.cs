using Core.DataAccess;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class RoleDal : EfBaseRepository<Role, BlogDbContext>, IRoleDal
    {
        public RoleDal(BlogDbContext context) : base(context)
        {
        }
    }
}
