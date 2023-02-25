using Business.Abstract;
using Business.Concrete;
using Entities.DTOs.Roles;
using Entities.DTOs.Writers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritersController : ControllerBase
    {
        private IWriterService _writerService;

        public WritersController(IWriterService writerService)
        {
            _writerService = writerService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(WriterForCreateDto dto)
        {
            var result = await _writerService.AddAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(WriterForUpdateDto dto)
        {
            var result = await _writerService.UpdateAsync(dto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _writerService.DeleteByUserIdAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult GetAll()
        {
            var result = _writerService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _writerService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public IActionResult GetAllWithUserInfo()
        {
            var result = _writerService.GetAllWithUserInfo();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var result =await _writerService.GetByUserId(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByIdWithUserInfoAsync(int id)
        {
            var result = await _writerService.GetByIdWithUserInfoAsync(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByIdWithAllInfo(int id)
        {
            var result = await _writerService.GetByIdWithAllInfo(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> ChangeNickNameAsync([FromBody] ChangeNickNameDto dto)
        {
            var result = await _writerService.ChangeNickNameAsync(dto.UserId,dto.NewNickName);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
