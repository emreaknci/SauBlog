﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities.Results;
using Entities.DTOs.Users;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<int>> AddAsync(User user);
        Task<IDataResult<User>> DeleteAsync(int id);
        Task<IDataResult<User>> UpdateAsync(UserForUpdateDto dto);
        Task<IDataResult<User>> GetUserByMailWithRolesAsync(string mail);
        Task<IDataResult<User>> GetUserByMailAsync(string mail);
        IDataResult<List<User>> GetAll();
        Task<IDataResult<User>> GetById(int id);
        Task<IDataResult<User>> GetWithRolesById(int id);

    }
}