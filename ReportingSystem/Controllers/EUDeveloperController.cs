using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Controllers
{
    public class EUDeveloperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Адміністративні функції

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
