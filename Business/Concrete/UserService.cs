using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.DTOs.Users;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IDataResult<int>> AddAsync(User user)
        {

            await _userDal.AddAsync(user);
            await _userDal.SaveAsync();

            return new SuccessDataResult<int>(user.Id);
        }

        public async Task<IDataResult<User>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<User>> UpdateAsync(UserForUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<User>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<User>> GetWithRolesById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
