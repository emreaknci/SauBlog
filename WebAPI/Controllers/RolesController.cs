using Business.Abstract;
using Business.Concrete;
using Entities.DTOs.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(RoleForCreateDto dto)
        {
            var result = await _roleService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(RoleForUpdateDto dto)
        {
            var result = await _roleService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var result = _roleService.GetAll();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roleService.GetById(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _roleService.GetByName(name);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWithUsersById(int id)
        {
            var result = await _roleService.GetWithUsersById(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWithUsersByName(string name)
        {
            var result = await _roleService.GetWithUsersByName(name);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
