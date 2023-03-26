using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Utilities.Results;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IWriterService? _writerService;
        protected IWriterService? WriterService => _writerService ??= HttpContext.RequestServices.GetService<IWriterService>();

        protected int GetCurrentWriterId()
        {
            var writer = WriterService.GetByUserId(GetCurrentUserId()).Result.Data;
            return writer!.Id;
        }
        protected int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Int32.Parse(userId);
        }
        protected bool IsCurrentUserAdmin()
        {
            var roles = User.FindAll(ClaimTypes.Role).ToList();
            foreach (var role in roles)
                if (role.Value.Contains("Admin"))
                    return true;
            return false;
        }
    }
}
