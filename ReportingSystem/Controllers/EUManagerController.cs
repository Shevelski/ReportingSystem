using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Controllers
{
    public class EUManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
