﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class EUUserController : Controller
    {
        public IActionResult StartPage()
        {
            return SessionHelper.ViewDataFullSession(HttpContext);
        }

        public IActionResult Index()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Projects()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Info()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Report()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Employees()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }
        public IActionResult Structure()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }
        public IActionResult Exit()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("StartPage", "Home");
        }
    }
}
