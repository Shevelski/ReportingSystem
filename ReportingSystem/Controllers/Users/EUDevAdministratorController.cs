using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class EUDevAdministratorController : Controller
    {

        public IActionResult Index()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Companies()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }

        public IActionResult Categories()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
        public IActionResult Projects()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
        public IActionResult Employees()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
        public IActionResult Info()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
        public IActionResult Positions()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
        public IActionResult Report()
        {
            string actionName = this.ControllerContext.ActionDescriptor.ActionName;
            return SessionHelper.ViewWithIdsFromSession(HttpContext, actionName);
        }
    }
}
