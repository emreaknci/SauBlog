using Core.Entities;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHandler
    {
        AccessToken CreateToken (User user, List<Role> roles);
    }
}
