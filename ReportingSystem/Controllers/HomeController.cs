﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Models;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ReportingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthorizeService _authorizeService;

        public HomeController(ILogger<HomeController> logger, AuthorizeService authorizeService)
        {
            _logger = logger;
            _authorizeService = authorizeService;
        }


        public IActionResult Authorize(bool authorizeOK)
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn([FromBody] AuthorizeModel authorizeModel)
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var result = _authorizeService.CheckEmail(email);
            return Json(result);
        }

        [HttpGet]
        public IActionResult CheckPassword(string email, string password)
        {
            var result = _authorizeService.CheckPassword(email, password);

            if (result != null && result.AuthorizeStatusModel != null)
            {
                var check = result.AuthorizeStatusModel.authorizeStatusType;
                if (check.Equals(AuthorizeStatus.PasswordOk))
                {
                    var controller = _authorizeService.GetRolController(result);

                    if (!string.IsNullOrEmpty(controller))
                    {
                        if (result.Employee != null)
                        {
                            var custId = result.Employee.customerId;
                            var compId = result.Employee.companyId;
                            var emplId = result.Employee.id;
                            Guid[] ids = {custId, compId, emplId };
                            HttpContext.Session.Set("ids", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ids)));
                            return RedirectToAction("Index", controller, new { ids = ids });
                        }
                        
                    }
                }
                else
                {
                    return Json(result);
                }
            }
            return Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}