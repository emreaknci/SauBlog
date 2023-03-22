using Business.Abstract;
using Business.Concrete;
using Entities.DTOs.Comment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : BaseController
    {
        private ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var result = _commentService.GetAll();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllByBlogId(int id)
        {
            var result = _commentService.GetAllByBlogId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllByWriterId(int id)
        {
            var result = _commentService.GetAllByWriterId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllForListing()
        {
            var result = _commentService.GetAllForListing();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllWithWriterByBlogId(int id)
        {
            var result = _commentService.GetAllWithWriterByBlogId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetWithPagination([FromQuery]CommentForPaginationRequest request)
        {
            var result = _commentService.GetWithPaginate(request);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetCurrentUserComments([FromQuery] CommentForPaginationRequest request)
        {
            request.WriterId = GetCurrentWriterId();
            var result = _commentService.GetWithPaginate(request);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Writer")]
        public async Task<IActionResult> AddAsync(CommentForCreateDto dto)
        {
            var result =await _commentService.AddAsync(dto);

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
            var result = await WriterService!.DoesCommentBelongToThisWriter(id, GetCurrentWriterId());
            if (!IsCurrentUserAdmin() && !result.Success) return Forbid();

            result = await _commentService.RemoveAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
