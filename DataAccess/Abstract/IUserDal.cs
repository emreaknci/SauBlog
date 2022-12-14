using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Core.Entities;
using DataAccess.Concrete.Context;

namespace DataAccess.Abstract
{
    public interface IUserDal : IRepository<User>
    {
    }
}
