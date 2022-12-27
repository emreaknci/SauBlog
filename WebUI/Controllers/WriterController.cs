using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class WriterController : Controller
    {
        private readonly IWriterService _writerService;

        public WriterController(IWriterService writerService)
        {
            _writerService = writerService;
        }

        public IActionResult Detail(int id)
        {
            var result = _writerService.GetByIdWithAllInfo(id).Result;
            
            return View(result);
        }
    }
}
