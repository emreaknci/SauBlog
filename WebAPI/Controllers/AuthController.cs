using Business.Abstract;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterForUser(UserForRegisterDto dto)
        {
            var result = await _authService.RegisterForUserAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyResetToken( string resetToken)
        {
            var result = await _authService.VerifyResetTokenAsync(resetToken);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int id)//id->userId
        {
            var result = await _authService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result.Success)
            {
                var token = _authService.CreateAccessToken(result.Data).Data;
                return Ok(token);
            }
            return BadRequest(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterForWriter(WriterForRegisterDto dto)
        {
            var result = await _authService.RegisterForWriterAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(UserForResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("[action]")]
        public IActionResult SendResetPassword(string email)
        {
            var result = _authService.SendPasswordResetEmailAsync(email).Result;

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
