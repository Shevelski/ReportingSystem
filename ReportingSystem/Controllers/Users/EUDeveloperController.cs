using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Controllers.Functions;

namespace ReportingSystem.Controllers.Users
{
    public class EUDeveloperController : Controller
    {
        public IActionResult StartPage()
        {
            return SessionHelper.ViewDataFullSession(HttpContext);
        }

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

        public IActionResult Administrators()
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

        public IActionResult Rolls()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }
        public IActionResult Structure()
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
            return RedirectToAction("StartPage", "Home");
        }

    }
}
