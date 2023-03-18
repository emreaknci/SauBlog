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
                    return new ErrorDataResult<User>("Doğru şifreyi girdiğinizden emin olun");
                return new SuccessDataResult<User>(userToCheck.Data, "Giriş başarılı");
            }
            return new ErrorDataResult<User>("Kullanıcı bulunamadı");
        }

        public async Task<IDataResult<int>> RegisterForUserAsync(UserForRegisterDto dto)
        {
            HashingHelper.CreatePasswordHash(dto.Password!, out var hash, out var salt);
            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = hash,
                PasswordSalt = salt,
            };
            if (CheckIfEmailPrivate(dto.Email).Success)
            {
                var adminRole = await _roleService.GetByName("Admin");
                user.Roles!.Add(adminRole.Data!);
            }
            var userRole = await _roleService.GetByName("User");
            user.Roles!.Add(userRole.Data!);
            var addResult = await _userService.AddAsync(user);

            if (!addResult.Success) return new ErrorDataResult<int>();
            await _mailService.SendRegistrationCompletedMailAsync(user.Email!, $"{user.FirstName} {user.LastName}");
            return new SuccessDataResult<int>(addResult.Data);
        }

        public async Task<IResult> RegisterForWriterAsync(WriterForRegisterDto dto)
        {
            var writerResult = await _writerService.GetByNickName(dto.NickName);
            if (writerResult.Success)
                return new ErrorResult("Bu kullanıcı adı daha önce kullanılmış!");


            var result = await RegisterForUserAsync(new UserForRegisterDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
            });
            if (!result.Success) return new ErrorResult();
            var user = _userService.GetById(result.Data).Result.Data;
            var writerRole = _roleService.GetByName("Writer").Result.Data;
            user.Roles!.Add(writerRole);
            await _writerService.AddAsync(new WriterForCreateDto
            {
                NickName = dto.NickName,
                UserId = result.Data
            });


            return new SuccessResult("Yazar Kaydı Oluşturuldu");
        }

        public async Task<IResult> DeleteAsync(int userId)
        {
            //once varsa yazari ve iliskili oldugu tablolarda veriler varsa siliyoruz
            await _writerService.DeleteByUserIdAsync(userId);
            var userDeleteResult = await _userService.DeleteAsync(userId);
            if (!userDeleteResult.Success)
                return new ErrorResult(userDeleteResult.Message);
            return new SuccessResult(userDeleteResult.Message);
        }

        public async Task<IResult> ResetPasswordAsync(UserForResetPasswordDto dto)
        {
            return await _userService.ResetPasswordAsync(dto.UserId, dto.NewPassword);
        }

        public async Task<IResult> SendPasswordResetEmailAsync(string email)
        {
            var user = _userService.GetUserByMailAsync(email).Result.Data;
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı!");

            var resetPasswordToken = Guid.NewGuid().ToString();
            var token = _tokenHandler.GenerateResetPasswordToken(user.Id.ToString(), resetPasswordToken);

            var result = await _userService.AddResetPasswordToken(user, token);
            if (!result.Success)
                return new ErrorResult();

            await _mailService.SendResetPasswordMailAsync(email, $"{user.FirstName} {user.LastName}", token);
            return new SuccessResult("Şifre sıfırlama maili gönderildi.");
        }

        public async Task<IDataResult<ResetPasswordToken>> VerifyResetTokenAsync(string resetToken)
        {
            var token = _tokenHandler.ValidateResetPasswordToken(resetToken);
            if (token == null)
                return new ErrorDataResult<ResetPasswordToken>("Token doğrulanamadı.Süresi dolmuş olabilir.");
            var userCurrentToken = _userService.GetById(token.UserId).Result.Data.ResetPasswordToken;
            if (string.IsNullOrEmpty(userCurrentToken) || userCurrentToken != resetToken)
                return new ErrorDataResult<ResetPasswordToken>("Bu link üzerinden şifre daha önce değiştirilmiş.");

            return new SuccessDataResult<ResetPasswordToken>(token);
        }

        private IResult CheckIfEmailPrivate(string email)
        {
            if (email == "yunus.akinci1@ogr.sakarya.edu.tr" || email == "emirhan.usta@ogr.sakarya.edu.tr")
                return new SuccessResult();
            return new ErrorResult();
        }
    }
}
