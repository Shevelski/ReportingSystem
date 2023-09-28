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
            return SessionHelper.ViewDataSession(HttpContext);
        }

        public IActionResult Companies()
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
        public IActionResult Structure()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }
        public IActionResult Report()
        {
            return SessionHelper.ViewDataSession(HttpContext);
        }
    }
}
