using Business.Abstract;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterForUser(UserForRegisterDto dto)
        {
          var result= await _authService.RegisterForUserAsync(dto);
          if (result.Success)
          {
              return Ok(result);
          }
          return BadRequest();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result.Success)
            {
                var token = _authService.CreateAccessToken(result.Data);
                return Ok(token.Data);
            }
            return BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterForWriter(WriterForRegisterDto dto)
        {
            var result = await _authService.RegisterForWriterAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "User")]
        public IActionResult denemeYetki()
        {
            return Ok();
        }
    }
}
