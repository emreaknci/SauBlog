using Business.Abstract;
using Core.Utilities.Results;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Add(CategoryForCreateDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update(CategoryForUpdateDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            Thread.Sleep(2000);
            var result = _categoryService.GetAll();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllWithBlogs()
        {
            var result = _categoryService.GetAllWithBlogs();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetListWithBlogCount()
        {
            var result = await _categoryService.GetListWithBlogCount();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetById(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByIdWithBlogs(int id)
        {
            var result = await _categoryService.GetByIdWithBlogs(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByList([FromQuery] List<int> ids)
        {
            var result = await _categoryService.GetByList(ids);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }

}
