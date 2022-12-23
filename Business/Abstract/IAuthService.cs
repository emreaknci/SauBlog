using Core.Entities;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<AccessToken> CreateAccessToken(User user);
        Task<IDataResult<User>> LoginAsync(UserForLoginDto dto);
        Task<IDataResult<int>> RegisterForUserAsync(UserForRegisterDto dto);
        Task<IResult> PasswordResetAsync(string email);
        Task<IDataResult<ResetPasswordToken>> VerifyResetTokenAsync(string resetToken);
        Task<IResult> RegisterForWriterAsync(WriterForRegisterDto dto);
    }
}
