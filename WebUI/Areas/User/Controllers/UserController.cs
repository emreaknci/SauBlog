using Business.Abstract;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        
        private Core.Entities.User CurrentUser()
        {
            var email = HttpContext.User.Claims.ToList()[1].Value;
            var user = _userService.GetUserByMailAsync(email).Result.Data;
            return user;
        }

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Edit()
        {
            ViewBag.email = CurrentUser().Email;

            var user = CurrentUser();
            UserForUpdateDto dto = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(dto);
        }
        [HttpPost]
        public IActionResult Edit(UserForUpdateDto dto)
        {
            ViewBag.email = CurrentUser().Email;

            if (dto.Id == null || dto.Id != CurrentUser().Id) //Id degistirmeye izin vermiyoruz. Manipule edilirse diye aktif kullanicinin idsini aliyoruz.
                dto.Id = CurrentUser().Id;

            if (!ModelState.IsValid)
            {

                return View(dto);
            }
            var email = HttpContext.User.Claims.ToList()[1].Value;
            var user = _userService.UpdateAsync(dto).Result.Data;
            return View(dto);
        }
    }
}
