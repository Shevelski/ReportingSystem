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
            return View();
        }

        public IActionResult Companies()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult Projects()
        {
            return View();
        }
        public IActionResult Employees()
        {
            return View();
        }
        public IActionResult Info()
        {
            return View();
        }
        public IActionResult Positions()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
    }
}
