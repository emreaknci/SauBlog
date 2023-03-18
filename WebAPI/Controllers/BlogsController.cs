using Business.Abstract;
using Business.Concrete;
using Entities.DTOs.Blog;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private IBlogService _blogService;

    public BlogsController(IBlogService blogService)
    {
        _blogService = blogService;
    }
    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Writer")]
    public async Task<IActionResult> Add(BlogForCreateDto dto)
    {
        var result = await _blogService.AddAsync(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Writer")]
    public async Task<IActionResult> Update(BlogForUpdateDto dto)
    {
        var result = await _blogService.UpdateAsync(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpDelete("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Writer,Admin")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _blogService.RemoveAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public IActionResult GetWithPagination(int index, int size, string? filter)
    {
        var result = _blogService.GetWithPaginate(index, size,filter);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        var result = _blogService.GetAll();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public IActionResult GetAllByWriterId(int id)
    {
        var result = _blogService.GetAllByWriterId(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public IActionResult GetAllByCategoryId(int id)
    {
        var result = _blogService.GetAllByCategoryId(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _blogService.GetById(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public IActionResult GetLastBlogs(int count = 3)
    {
        var result = _blogService.GetLastBlogs(count);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetByIdWithCommentsAsync(int id)
    {
        var result = await _blogService.GetByIdWithCommentsAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByIdWithCategories(int id)
    {
        var result = await _blogService.GetByIdWithCategories(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByIdWithWriter(int id)
    {
        var result = await _blogService.GetByIdWithWriter(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}

