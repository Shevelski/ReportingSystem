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
using ReportingSystem.Data.JSON;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace ReportingSystem.Controllers.Users
{
    public class HomeController(ILogger<HomeController> logger, AuthorizeService authorizeService, IHubContext<StatusHub> hubContext) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly AuthorizeService _authorizeService = authorizeService;
        private readonly IHubContext<StatusHub> _hubContext = hubContext;
            

        public IActionResult Authorize()
        {
            string cookieValue = Request.Cookies["culture"];
            if ( cookieValue == null)
            {
                CultureInfo currentCulture = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
                string languageCode = currentCulture.TwoLetterISOLanguageName;
                ViewBag.CookieValue = languageCode;
            }
            else
            {
                ViewBag.CookieValue = cookieValue;
            }            
            
            
            return View();
        }
        public void GenerateData()
        {
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
                var check = result.AuthorizeStatusModel.AuthorizeStatusType;
                if (check.Equals(AuthorizeStatus.PasswordOk))
                {
                    var controller = _authorizeService.GetRolController();

                    if (!string.IsNullOrEmpty(controller))
                    {
                        if (result.Employee != null)
                        {
                            var custId = result.Employee.CustomerId;
                            var compId = result.Employee.CompanyId;
                            var emplId = result.Employee.Id;
                            var rol = "";
                            if (result.Employee.Rol != null && result.Employee.Rol.RolName != null)
                            {
                                rol = result.Employee.Rol.RolType.ToString();
                            }
                            string[] ids = [custId.ToString(), compId.ToString(), emplId.ToString(), rol.ToString()];
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
        [HttpGet]
        public async Task<bool> ConfigEnter(string username, string password)
        {
            await Task.Delay(10);
            if (username == "life" && password == "isgood" + DateTime.Now.Minute)
            {
                return true;
            } else
            {
                return false;
            }
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
            bool result = Database.IsExist(Context.serverName, Context.dbName);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsDatabaseAvailable1(string serverName, string databaseName)
        {
            await Task.Delay(10);
            bool result = Database.IsDatabaseAvailable(serverName, databaseName);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsDatabaseAvailable2(string serverName, string databaseName, string login, string password)
        {
            await Task.Delay(10);
            bool result = Database.IsDatabaseAvailable(serverName, databaseName, login, password);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsTablesAvailable1(string serverName, string databaseName)
        {
            bool result = await new Database().IsTablesAvailable(serverName, databaseName);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsTablesAvailable2(string serverName, string databaseName, string login, string password)
        {
            return await new Database().IsTablesAvailable(serverName, databaseName, login, password);
        }

        [HttpGet]
        public async Task<bool> IsServerAvailable1(string serverName)
        {
            await Task.Delay(10);
            var result = Database.IsServerAvailable(serverName);
            return result;
        }

        [HttpGet]
        public async Task<bool> IsServerAvailable2(string serverName, string login, string password)
        {
            await Task.Delay(10);
            return Database.IsServerAvailable(serverName, login, password);
        }

        [HttpGet]
        public async Task<string[]> GetConnectionString()
        {
            await Task.Delay(10);
            string[] result = [Context.serverName.Replace("\\\\", "\\"), Context.dbName];
            return result;
        }

        [HttpPost]
        public async Task SetConnectionString([FromBody] string[] ar)
        {
            await Task.Delay(10);
            new JsonWrite().UpdateJsonAppsettings(ar);
        }

        [HttpPost]
        public async Task ClearTables([FromBody] string[] ar)
        {
            await ClearDatabase.ClearTables(ar);
        }

        public async Task<IActionResult> ChangeLocalization(string culture)
        {
            await Task.Delay(10);
            HttpContext.Request.Cookies.TryGetValue("previous", out string? previous);
            //string cookieValue = culture;
            //ViewBag.CookieValue = cookieValue;

            return Redirect(previous ?? "/");
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}