using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class EUProjectManagerController : Controller
    {

        private readonly ILogger<EUProjectManagerController> _logger;

        public EUProjectManagerController(ILogger<EUProjectManagerController> logger)
        {
            _logger = logger;
        }

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
    }
}
