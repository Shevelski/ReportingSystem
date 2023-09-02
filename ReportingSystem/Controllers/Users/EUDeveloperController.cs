using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using ReportingSystem.Models.User;
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

        public IActionResult Index()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Companies()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Steps()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Customers()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Categories()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Projects()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Employees()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Info()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Positions()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }


        public IActionResult Report()
        {
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Exit()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Authorize", "Home");
        }

    }
}
