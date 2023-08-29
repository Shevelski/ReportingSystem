using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models;
using System.Diagnostics;

namespace ReportingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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

        public IActionResult Employees()
        {
            return View();

        }
        public IActionResult Projects()
        {
            return View();

        }

        public IActionResult ProjectsCategories()
        {
            return View();

        }

        public IActionResult StepProjects()
        {
            return View();
        }


        //public IActionResult Index()
        //{
        //    if (User.IsInRole("EUAdministrator"))
        //    {
        //        return RedirectToAction("ActionName", "EUAdministrator");
        //    }
        //    else if (User.IsInRole("EUDeveloper"))
        //    {
        //        return RedirectToAction("ActionName", "EUDeveloper");
        //    }
        //    else if (User.IsInRole("EUUser"))
        //    {
        //        return RedirectToAction("ActionName", "EUUser");
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}