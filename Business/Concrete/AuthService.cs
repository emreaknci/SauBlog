using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs.Users;

namespace Business.Concrete
{
    internal class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ITokenHandler _tokenHandler;

        public AuthService(IUserService userService, IRoleService roleService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _roleService = roleService;
            _tokenHandler = tokenHandler;
        }

        public IDataResult<AccessToken> CreateAccessTokenAsync(User user)
        {
            var token = _tokenHandler.CreateToken(user, user.Roles!.ToList());
            return new SuccessDataResult<AccessToken>(token);
            throw new NotImplementedException();
        }

        public async Task<IResult> LoginAsync(UserForLoginDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<int>> RegisterAsync(UserForRegisterDto dto)
        {
            HashingHelper.CreatePasswordHash(dto.Password, out var hash, out var salt);
            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            var result = await _userService.AddAsync(user);
            return new SuccessDataResult<int>(result.Data);
        }
    }
}
