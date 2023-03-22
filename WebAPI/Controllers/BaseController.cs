using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var writer = _writerService.GetByUserId(Convert.ToInt32(userId)).Result.Data;
            return writer!.Id;
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
