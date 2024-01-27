using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Controllers.Functions
{
    public class ConfigurationController : Controller
    {

        public IActionResult Configuration()
        {
            return View();
        }
    }
}
