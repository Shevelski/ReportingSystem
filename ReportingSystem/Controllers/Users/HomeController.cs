using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Data;
using ReportingSystem.Data.Generate;
using ReportingSystem.Enums;
using ReportingSystem.Models;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using System.Diagnostics;
using System.Text;
using ReportingSystem.Data.SQL;
using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Hubs;

namespace ReportingSystem.Controllers.Users
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthorizeService _authorizeService;
        private readonly IHubContext<StatusHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, AuthorizeService authorizeService, IHubContext<StatusHub> hubContext)
        {
            _logger = logger;
            _authorizeService = authorizeService;
            _hubContext = hubContext;  // Added this line
        }

        public IActionResult Authorize()
        {
            //генерація даних, залежить від режиму в Settings
            return View();
        }

        public void GenerateData()
        {
            //if (Utils.Settings.Mode().Equals("write"))
            //{

            //}
            try
            {
                var a = new GenerateMain(_hubContext).Data();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            AuthorizeModel? result;

            bool mode = Utils.Settings.Source().Equals("json");
            result = mode ? _authorizeService.CheckEmailJson(email) :
                      await _authorizeService.CheckEmailSQL(email);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CheckPassword(string email, string password)
        {

            AuthorizeModel? result;

            bool mode = Utils.Settings.Source().Equals("json");
            result = mode ? _authorizeService.CheckPasswordJson(email, password) :
                      await _authorizeService.CheckPasswordSQL(email, password);

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
                    return RedirectToAction("Authorize", "Home", new { result });
                }
            }
            return Json(result);
        }
        public async Task<IActionResult>  Registration()
        {   
            await Task.Delay(10);
            return View();
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



        [HttpGet]
        public async Task<bool> HasDatabase()
        {
            await Task.Delay(10);
            bool result = Database.IsExist(Context.connectionDB, Context.dbName);
            return result;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}