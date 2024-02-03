using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Controllers
{
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
