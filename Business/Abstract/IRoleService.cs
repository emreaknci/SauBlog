using Core.Entities;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs.Roles;

namespace Business.Abstract
{
    public interface IRoleService
    {
        Task<IDataResult<Role>> AddAsync(RoleForCreateDto dto);
        Task<IDataResult<Role>> DeleteAsync(int id);
        Task<IDataResult<Role>> UpdateAsync(RoleForUpdateDto dto);

        IDataResult<List<Role>> GetAll();
        Task<IDataResult<Role>> GetById(int id);
        Task<IDataResult<Role>> GetByName(string name);
        Task<IDataResult<Role>> GetWithUsersById(int id);


    }
}
