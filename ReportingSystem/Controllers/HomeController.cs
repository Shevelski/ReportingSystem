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
        //public IActionResult Projects()
        //{
        //    return View();

        //}

        //public IActionResult ProjectsCategories()
        //{
        //    return View();

        //}

        public IActionResult StepProjects()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[ApiController]
        //[Route("api/[controller]")]
        //public class DataController : ControllerBase
        //{
        //    [HttpPost]
        //    public IActionResult PostData([FromBody] ProjectModel project)
        //    {
        //        return Ok("Дані успішно збережено на сервері!");
        //    }
        //}

    }
}