using Business.Abstract;
using Entities.DTOs.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddTask(RoleForCreateDto dto)
        {
            var result = await _roleService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
