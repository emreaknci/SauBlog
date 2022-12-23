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
using Microsoft.EntityFrameworkCore;

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

        public async Task<IResult> DeleteAsync(int id)
        {
            var role = await _roleDal.GetByIdAsync(id);
            if (role!=null)
            {
                _roleDal.Remove(role);
                await _roleDal.SaveAsync();
                return new SuccessResult("Rol Silindi");
            }
            return new ErrorResult("Rol Bulunamadı");
        }

        public async Task<IDataResult<Role>> UpdateAsync(RoleForUpdateDto dto)
        {
           var role = await _roleDal.GetByIdAsync(dto.Id);
            if (role!=null)
            {
                role.Name = dto.Name;
                _roleDal.Update(role);
                await _roleDal.SaveAsync();
                return new SuccessDataResult<Role>(role, "Rol Bilgileri Güncellendi");
            }
            return new ErrorDataResult<Role>("Rol Bulunamadı");
        }

        public IDataResult<List<Role>> GetAll()
        {
            var list = _roleDal.GetAll().ToList();
            if (list.Count>0)
            {
                return new SuccessDataResult<List<Role>>(list);
            }
            return new ErrorDataResult<List<Role>>("Rol Bulunamadı");
        }

        public async Task<IDataResult<Role>> GetById(int id)
        {
            var role = await _roleDal.GetByIdAsync(id);

            if (role!=null)
            {
                return new SuccessDataResult<Role>(role);
            }
            return new ErrorDataResult<Role>("Rol bulunamadı");
        }

        public async Task<IDataResult<Role>> GetByName(string name)
        {
            var role = await _roleDal.GetAsync(r => r.Name == name);

            if (role != null)
            {
                return new SuccessDataResult<Role>(role);
            }
            return new ErrorDataResult<Role>("Rol bulunamadı");
        }

        public async Task<IDataResult<Role>> GetWithUsersById(int id)
        {
            var role = await _roleDal.Table.Include(r=>r.Users).FirstOrDefaultAsync(r=>r.Id==id);
            if (role!=null)
            {
                return new SuccessDataResult<Role>(role);
            }
            return new ErrorDataResult<Role>("Rol Bulunamadı");
        }
    }
}
