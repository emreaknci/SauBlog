using Business.Abstract;
using Core.Utilities.Results;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        private Core.Entities.User GetCurrentUser()
        {
            var user = _userService!.GetById(Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value)).Result.Data;
            return user!;
        }

        public UserController(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }
        [HttpGet]
        public IActionResult Edit()
        {
            ViewBag.email = GetCurrentUser().Email!;

            var user = GetCurrentUser();
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
            ViewBag.email = GetCurrentUser().Email!;

            if (dto.Id == null || dto.Id != GetCurrentUser().Id) //Id degistirmeye izin vermiyoruz. Manipule edilirse diye aktif kullanicinin idsini aliyoruz.
                dto.Id = GetCurrentUser().Id;

            if (!ModelState.IsValid)
            {

                return View(dto);
            }
            var email = HttpContext.User.Claims.ToList()[1].Value;
            var user = _userService.UpdateAsync(dto).Result.Data;
            return View(dto);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AssignRole(int userId)
        {
            var roleName = (string)TempData["roleName"]!;
           var result= _userService.AssignRole(userId, roleName).Result;
           
           if (result.Success)
               _toastNotification.AddSuccessToastMessage(result.Message);
           else
               _toastNotification.AddErrorToastMessage(result.Message);

            return Redirect(TempData["url"]!.ToString()!);
        }

        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RevokeRole(int userId, string previousUrl)
        {
            var roleName = (string)TempData["roleName"]!;
            var result = await _userService.RevokeRole(userId, roleName);

            if (result.Success)
                _toastNotification.AddSuccessToastMessage(result.Message);
            else
                _toastNotification.AddErrorToastMessage(result.Message);
            return Redirect(TempData["url"]!.ToString()!);
        }
    }
}
