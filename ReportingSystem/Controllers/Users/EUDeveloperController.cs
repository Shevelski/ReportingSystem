using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using ReportingSystem.Models.User;
using System.Reflection;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class EUDeveloperController : Controller
    {

        private readonly ILogger<EUDeveloperController> _logger;

        public EUDeveloperController(ILogger<EUDeveloperController> logger)
        {
            _logger = logger;
        }

        //private IActionResult GetIdsFromSession()
        //{
        //    if (HttpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
        //    {
                
        //        var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));
        //        return View(ids);
        //    }
        //    else
        //    {
        //        HttpContext.SignOutAsync();
        //        return RedirectToAction("Authorize", "Home");
        //    }
        //}

        //public string[]? GetIds()
        //{
        //    if (HttpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
        //    {
        //        var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));
        //        return ids;
        //    }
        //    return null;
        //}

        public IActionResult Index()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Companies()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Steps()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Customers()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Categories()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Projects()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Employees()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Info()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Positions()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }


        public IActionResult Report()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Exit()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Authorize", "Home");
        }

    }
}
