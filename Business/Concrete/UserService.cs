using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities;
using Core.Extensions;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.DTOs.Users;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IResult> AddResetPasswordToken(User user, string resetPasswordToken)
        {
            user.ResetPasswordToken = resetPasswordToken;
            _userDal.Update(user);
            await _userDal.SaveAsync();
            return new SuccessResult();
        }

        public async Task<IDataResult<User>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<User>> UpdateAsync(UserForUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userDal.GetByIdAsync(userId);
            if (user == null)
                return new ErrorResult("Kullanıcı bulunamadı");

            HashingHelper.CreatePasswordHash(newPassword, out var hash, out var salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.ResetPasswordToken = null;
            _userDal.Update(user);
            await _userDal.SaveAsync();
            return new SuccessResult("Şifre değişikliği tamamlandı!");
        }

        public async Task<IDataResult<User>> GetUserByMailWithRolesAsync(string? mail)
        {
            var user = await _userDal.Table.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == mail);
            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<User>> GetUserByMailAsync(string mail)
        {
            var user = await _userDal.Table.FirstOrDefaultAsync(x => x.Email == mail);
            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<User>> GetUserByIdWithRolesAsync(int id)
        {
            var user = await _userDal.Table.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);
            return new SuccessDataResult<User>(user);
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

        public List<Claim> GetClaims(User user, List<Role> roles)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email!);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(roles);
            return claims;
        }
    }
}
