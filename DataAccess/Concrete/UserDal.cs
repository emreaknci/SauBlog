using Core.DataAccess;
using Core.Entities;
using DataAccess.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;

namespace DataAccess.Concrete
{
    public class UserDal : EfBaseRepository<User, BlogDbContext>, IUserDal
    {
        public UserDal(BlogDbContext context) : base(context)
        {
        }
    }
}
