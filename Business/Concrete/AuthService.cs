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
        private readonly IMailService _mailService;
        public AuthService(IUserService userService, IRoleService roleService, ITokenHandler tokenHandler, IWriterService writerService, IMailService mailService)
        {
            _userService = userService;
            _roleService = roleService;
            _tokenHandler = tokenHandler;
            _writerService = writerService;
            _mailService = mailService;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var token = _tokenHandler.CreateToken(user, user.Roles!.ToList());
            return new SuccessDataResult<AccessToken>(token);
        }

        public async Task<IDataResult<User>> LoginAsync(UserForLoginDto dto)
        {
            var userToCheck = await _userService.GetUserByMailWithRolesAsync(dto.Email);
            if (userToCheck.Data != null)
            {
                if (!HashingHelper.VerifyPasswordHash(dto.Password!, userToCheck.Data.PasswordHash!,
                        userToCheck.Data.PasswordSalt!))
                    return new ErrorDataResult<User>();
                return new SuccessDataResult<User>(userToCheck.Data, "Giriş başarılı");
            }
            return new ErrorDataResult<User>();
        }

        public async Task<IDataResult<int>> RegisterForUserAsync(UserForRegisterDto dto)
        {
            HashingHelper.CreatePasswordHash(dto.Password!, out var hash, out var salt);
            var userRoleResult = await _roleService.GetByName("User");
           
            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = hash,
                PasswordSalt = salt,
            };
            user.Roles!.Add(userRoleResult.Data!);
            var result = await _userService.AddAsync(user);

            if (!result.Success) return new ErrorDataResult<int>();
            await _mailService.SendRegistrationCompletedMailAsync(user.Email!, $"{user.FirstName} {user.LastName}");
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
            if (!result.Success) return new ErrorResult();
            await _writerService.AddAsync(new WriterForCreateDto
            {
                NickName = dto.NickName,
                UserId = result.Data
            });
            return new SuccessResult();
        }
    }
}
