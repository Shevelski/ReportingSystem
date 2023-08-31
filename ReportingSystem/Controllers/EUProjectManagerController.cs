using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.User;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers
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

        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult Customers()
        {
            return View();
        }
        public IActionResult Companies()
        {
            return View();

        }
    }
}
