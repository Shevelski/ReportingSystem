using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Models;
using ReportingSystem.Services;
using System.Diagnostics;
using System.Text;

namespace ReportingSystem.Controllers.Users
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthorizeService _authorizeService;

        public HomeController(ILogger<HomeController> logger, AuthorizeService authorizeService)
        {
            _logger = logger;
            _authorizeService = authorizeService;
        }


        public IActionResult Authorize()
        {
            var a = DatabaseMoq.Customers;
            var b = DatabaseSQL.Init;
            return View();
        }

        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var result = _authorizeService.CheckEmail(email);
            return Json(result);
        }

        [HttpGet]
        public IActionResult CheckPassword(string email, string password)
        {
            var result = _authorizeService.CheckPassword(email, password);

            if (result != null && result.AuthorizeStatusModel != null)
            {
                var check = result.AuthorizeStatusModel.authorizeStatusType;
                if (check.Equals(AuthorizeStatus.PasswordOk))
                {
                    var controller = _authorizeService.GetRolController(result);

                    if (!string.IsNullOrEmpty(controller))
                    {
                        if (result.Employee != null)
                        {
                            var custId = result.Employee.customerId;
                            var compId = result.Employee.companyId;
                            var emplId = result.Employee.id;
                            var rol = "";
                            if (result.Employee.rol != null && result.Employee.rol.rolName != null)
                            {
                                rol = result.Employee.rol.rolType.ToString();
                            }
                            string[] ids = { custId.ToString(), compId.ToString(), emplId.ToString(), rol.ToString()};
                            HttpContext.Session.Set("ids", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ids)));
                            return RedirectToAction("Index", controller, new { ids });
                        }
                    }
                }
                else
                {
                    //return Json(result);
                    return RedirectToAction("Authorize", "Home", new { result });
                }
            }
            return Json(result);
        }

         public async Task<IActionResult> LogIn()
        {
            await Task.Delay(10);
            return View();
        }
        public async Task<IActionResult> Index()
        {
            await Task.Delay(10);
            return View();
        }
         public async Task<IActionResult>  AboutUs()
        {
            await Task.Delay(10);
            return View();
        }
        public async Task<IActionResult>  Contacts()
        {
            await Task.Delay(10);
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}