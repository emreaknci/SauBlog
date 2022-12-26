using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities;
using Core.Extensions;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IDataResult<int>> AddAsync(User user)
        {
            var result= BusinessRules.Run(CheckIfEmailExist(user.Email));
            if (result != null)
            {
                return new ErrorDataResult<int>(result.Message);
            }
            await _userDal.AddAsync(user);
            await _userDal.SaveAsync();

            return new SuccessDataResult<int>(user.Id);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var user = await _userDal.GetByIdAsync(id);
            if (user != null)
            {
                _userDal.Remove(user);
                await _userDal.SaveAsync();
                return new SuccessResult("User Silindi");
            }
            return new ErrorResult("User Bulunamadı");

        }

        public async Task<IDataResult<User>> UpdateAsync(UserForUpdateDto dto)
        {
            var user = await _userDal.GetByIdAsync(dto.Id);

            if (user != null)
            {
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                _userDal.Update(user);
                await _userDal.SaveAsync();
                return new SuccessDataResult<User>(user, "User Güncellendi");
            }
            return new ErrorDataResult<User>("User Bulunamadı");
        }

        public async Task<IResult> ChangePasswordAsync(UserForChangePasswordDto dto)
        {
            var userToCheck = await _userDal.GetByIdAsync(dto.UserId);
            if (userToCheck != null)
            {
                if (!HashingHelper.VerifyPasswordHash(dto.OldPassword!, userToCheck.PasswordHash!,
                        userToCheck.PasswordSalt!))
                    return new ErrorResult("Hatalı Şifre!");
                return new SuccessResult("Şifre değişikliği tamamlandı!");
            }
            return new ErrorResult("Kullanıcı bulunamadı");
        }

        public async Task<IDataResult<User>> GetUserByMailWithRolesAsync(string? mail)
        {
            var user = await _userDal.Table.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == mail);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>("User Bulunamadı");
        }


        public async Task<IDataResult<User>> GetUserByMailAsync(string mail)
        {
            var user = await _userDal.Table.FirstOrDefaultAsync(x => x.Email == mail);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>("User Bulunamadı");
        }


        public async Task<IDataResult<User>> GetUserByIdWithRolesAsync(int id)
        {
            var user = await _userDal.Table.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>("User Bulunamadı");

        }

        public IDataResult<List<User>> GetAll()
        {
            var list = _userDal.GetAll().ToList();
            if (list.Count > 0)
            {
                return new SuccessDataResult<List<User>>(list);
            }
            return new ErrorDataResult<List<User>>("User Bulunamadı");
        }

        public async Task<IDataResult<User>> GetById(int id)
        {
            var user = await _userDal.GetByIdAsync(id);

            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>("User Bulunamadı");
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



        public async Task<IResult> AddResetPasswordToken(User user, string resetPasswordToken)
        {
            user.ResetPasswordToken = resetPasswordToken;
            _userDal.Update(user);
            await _userDal.SaveAsync();
            return new SuccessResult();
        }
        public async Task<IResult> ResetPasswordAsync(int userId, string newPassword)
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



        private IResult CheckIfEmailExist(string email)
        {
            var result = _userDal.GetAll(p => p.Email == email).Any();
            if (result)
            {
                return new ErrorResult("Bu email daha önce kullanılmış");
            }
            return new SuccessResult();
        }
    }
}
