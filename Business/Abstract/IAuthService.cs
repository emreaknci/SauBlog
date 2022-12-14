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
        IDataResult<AccessToken> CreateAccessTokenAsync(User user);
        Task<IResult> LoginAsync(UserForLoginDto dto);
        Task<IDataResult<int>> RegisterForUserAsync(UserForRegisterDto dto);
        Task<IResult> RegisterForWriterAsync(WriterForRegisterDto dto);
    }
}
