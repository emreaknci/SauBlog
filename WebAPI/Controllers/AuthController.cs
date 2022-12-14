using Business.Abstract;
using Entities.DTOs.Users;
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
        public async Task<IActionResult> Register(UserForRegisterDto dto)
        {
          var result= await _authService.RegisterAsync(dto);
          if (result.Success)
          {
              return Ok(result);
          }
          return BadRequest();
        }
    }
}
