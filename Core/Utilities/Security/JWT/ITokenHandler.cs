using Core.Entities;
using System.Security.Claims;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHandler
    {
        AccessToken CreateToken(User user, List<Role> roles);
        string GenerateResetPasswordToken(string userId, string resetPasswordToken);
        ResetPasswordToken ValidateResetPasswordToken(string token);
    }
}
