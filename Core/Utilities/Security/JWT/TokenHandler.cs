using Core.Entities;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration Configuration;
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = new()
            {
                Issuer = Configuration["TokenOptions:Issuer"],
                Audience = Configuration["TokenOptions:Audience"],
                SecurityKey = Configuration["TokenOptions:SecurityKey"],
                AccessTokenExpiration = Convert.ToInt16(Configuration["TokenOptions:AccessTokenExpiration"])
            };
        }

        public AccessToken CreateToken(User user, List<Role> roles)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, roles);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new()
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        private IEnumerable<Claim> SetClaims(User user, List<Role> roles)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email!);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(roles);
            return claims;
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials,
            List<Role> roles)
        {
            JwtSecurityToken jwt = new(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, roles),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        public string GenerateResetPasswordToken(string userId, string resetPasswordToken)
        {
            // Token oluşturma işlemini gerçekleştirin.
            // Örneğin, tarih ve saat bilgilerini kullanarak bir token oluşturun.
            // Token, verilen userId ve resetPasswordToken değerlerini içerecektir.
            var currentDate = DateTime.UtcNow;
            var expirationDate = currentDate.AddDays(1);

            var token = $"{userId}:{resetPasswordToken}:{currentDate.Ticks}:{expirationDate.Ticks}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
        }
        public ResetPasswordToken ValidateResetPasswordToken(string token)
        {
            // Token doğrulama işlemini gerçekleştirin.
            // Örneğin, token'ın oluşturulduğu tarih ve saat bilgilerine bakarak token'ın geçerliliğini doğrulayabilirsiniz.
            // Token geçerli ise, token içindeki bilgileri döndürün.
            // Token geçersiz ise, null değerini döndürün.
            var tokenBytes = Convert.FromBase64String(token);
            var tokenString = Encoding.UTF8.GetString(tokenBytes);
            var tokenValues = tokenString.Split(':');

            if (tokenValues.Length != 4)
            {
                return null;
            }

            var userId = tokenValues[0];
            var resetPasswordToken = tokenValues[1];
            var currentTicks = long.Parse(tokenValues[2]);
            var expirationTicks = long.Parse(tokenValues[3]);

            var currentDate = new DateTime(currentTicks);
            var expirationDate = new DateTime(expirationTicks);

            if (currentDate > DateTime.UtcNow || expirationDate < DateTime.UtcNow)
            {
                return null;
            }

            // Token geçerli. Token içindeki bilgileri döndürün.
            ResetPasswordToken resetToken = new()
            {
                Token = resetPasswordToken,
                UserId = Convert.ToInt32(userId),
                ExpirationDate = expirationDate
            };


            return resetToken;
        }
    }
}
