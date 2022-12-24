using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Drawing;
using WebUI.Models;
using X.PagedList;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;

        public HomeController(ILogger<HomeController> logger, IBlogService blogService, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _blogService = blogService;
            _localizer = localizer;
        }

        public IActionResult Index(int page = 1, int size = 6)
        {
            //var result = _blogService.GetWithPaginate(page - 1, size);
            //List<int> pageList = new();
            //if (page - 3 <= 0)
            //    for (var i = 1; i <= 7; i++)
            //        pageList.Add(i);

            //else if (page + 3 >= result.Pages)
            //    for (var i = result.Pages - 6; i <= result.Pages; i++)
            //        pageList.Add(i);

            //else
            //    for (var i = page - 3; i <= page + 3; i++)
            //        pageList.Add(i);
            ////ViewBag.pageList = pageList;
            //return View((result,pageList));
            IPagedList<Blog> pagedlist = null;
            var result = _blogService.GetAll();
            if(result.Data!= null) 
           pagedlist = result.Data.ToPagedList(page, size);
            return View(pagedlist);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}