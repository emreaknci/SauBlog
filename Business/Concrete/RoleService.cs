using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.DTOs.Roles;

namespace Business.Concrete
{
    public class RoleService : IRoleService
    {
        private IRoleDal _roleDal;

        public RoleService(IRoleDal roleDal)
        {
            _roleDal = roleDal;
        }

        public async Task<IDataResult<Role>> AddAsync(RoleForCreateDto dto)
        {
            var newRole = new Role()
            {
                Name = dto.Name,
            };
            await _roleDal.AddAsync(newRole);
            await _roleDal.SaveAsync();
            return new SuccessDataResult<Role>(newRole);
        }

        public async Task<IDataResult<Role>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Role>> UpdateAsync(RoleForUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Role>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Role>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Role>> GetWithUsersById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
