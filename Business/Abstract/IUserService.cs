﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities.Results;
using Entities.DTOs.Users;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IResult> AddResetPasswordToken(User user, string resetPasswordToken);
        Task<IDataResult<int>> AddAsync(User user);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<User>> UpdateAsync(UserForUpdateDto dto);
        Task<IResult> ResetPasswordAsync(int userId,string newPassword);
        Task<IResult> ChangePasswordAsync(UserForChangePasswordDto dto);
        Task<IDataResult<User>> GetUserByMailWithRolesAsync(string? mail);
        Task<IDataResult<User>> GetUserByMailAsync(string mail);
        IDataResult<List<User>> GetAll();
        Task<IDataResult<User>> GetById(int id);
        List<Claim> GetClaims(User user, List<Role> roles);


    }
}
