using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Controllers.Functions;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class EUUserController : Controller
    {

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
