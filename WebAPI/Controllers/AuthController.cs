using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Entities.DTOs.Users;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private IWriterDal _writerDal;
        public AuthController(IAuthService authService, IWriterDal writerDal)
        {
            _authService = authService;
            _writerDal = writerDal;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterForUser(UserForRegisterDto dto)
        {
            var result = await _authService.RegisterForUserAsync(dto);
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
                var token = _authService.CreateAccessToken(result.Data).Data;
                return Ok(token);
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


        [HttpPost("[action]")]
        public IActionResult SendResetPassword(string email)
        {
            var result = _authService.PasswordResetAsync(email).Result;

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
