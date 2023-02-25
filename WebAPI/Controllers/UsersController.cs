using Business.Abstract;
using Business.Concrete;
using Entities.DTOs.Roles;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AssignRole(int userId, string roleName)
        {
            var result = await _userService.AssignRole(userId,roleName);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RevokeRole(int userId, string roleName)
        {
            var result = await _userService.AssignRole(userId, roleName);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _userService.DeleteAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(UserForUpdateDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(UserForChangePasswordDto dto)
        {
            var result = await _userService.ChangePasswordAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public ActionResult GetAll()
        {
            var result = _userService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public ActionResult GetAllNonAdmin()
        {
            var result = _userService.GetAllNonAdmin();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetById([FromQuery]int id)
        {
            var result =await _userService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetUserByMailAsync([FromQuery] string email)
        {
            var result = await _userService.GetUserByMailAsync(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetUserByMailWithRolesAsync([FromQuery] string email)
        {
            var result = await _userService.GetUserByMailWithRolesAsync(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
