﻿using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IToastNotification _toastNotification;

        public ErrorController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int? statusCode)
        {
            switch (statusCode.Value)
            {
                case 404:
                    return RedirectToAction("Error404");
                    break;
                case 405:
                    return RedirectToAction("Error405");
                    break;
            }
            return View("Error");
        }

        [HttpGet("404")]
        public IActionResult Error404()
        {
            return View();
        }
        [HttpGet("405")]
        public IActionResult Error405()
        {
            return View();
        }
    }
}
