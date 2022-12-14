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
using Entities.DTOs.Writers;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IWriterService _writerService;
        public AuthService(IUserService userService, IRoleService roleService, ITokenHandler tokenHandler, IWriterService writerService)
        {
            _userService = userService;
            _roleService = roleService;
            _tokenHandler = tokenHandler;
            _writerService = writerService;
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

        public async Task<IDataResult<int>> RegisterForUserAsync(UserForRegisterDto dto)
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

        public async Task<IResult> RegisterForWriterAsync(WriterForRegisterDto dto)
        {
            var result = await RegisterForUserAsync(new UserForRegisterDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
            });
            await _writerService.AddAsync(new WriterForCreateDto
            {
                NickName = dto.NickName,
                UserId = result.Data
            });
            return new SuccessResult();
        }
    }
}
