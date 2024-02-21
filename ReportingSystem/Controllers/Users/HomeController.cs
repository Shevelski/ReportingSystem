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
using ReportingSystem.Enums.Extensions;

namespace ReportingSystem.Controllers.Users
{
    public class HomeController(ILogger<HomeController> logger, AuthorizeService authorizeService, IHubContext<StatusHub> hubContext) : BaseController
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly AuthorizeService _authorizeService = authorizeService;
        private readonly IHubContext<StatusHub> _hubContext = hubContext;
            

        public IActionResult Authorize()
        {            
            return PartialView();
        }
        public async Task<bool> GenerateData()
        {
            try
            {
                return await new GenerateMain(_hubContext).Data();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
                return false;
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
                            var rol = "";
                            if (result.Employee.Rol != null && result.Employee.Rol.RolName != null)
                            {
                                rol = result.Employee.Rol.RolType.ToString();
                            }

                            Guid custId = result.Employee.CustomerId;
                            Guid compId = result.Employee.CompanyId;
                            Guid emplId = result.Employee.Id;

                            if (result.Employee.Rol.RolType == EmployeeRolStatus.Customer)
                            {
                                custId = result.Employee.Id;
                                compId = Guid.Empty;
                                emplId = Guid.Empty;
                            } else
                            {
                                custId = result.Employee.CustomerId;
                                compId = result.Employee.CompanyId;
                                emplId = result.Employee.Id;
                            }

                            string[] ids = [custId.ToString(), compId.ToString(), emplId.ToString(), rol.ToString()];
                            HttpContext.Session.Set("ids", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ids)));
                            return RedirectToAction("StartPage", controller);

                        }
                    }
                }
                else
                {
                    return RedirectToAction("StartPage", "Home", new { result });
                }
            }
            return Json(result);
        }

        public async Task<List<string>> CheckCustomer(string email, string password)
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
                            var rol = "";
                            if (result.Employee.Rol != null && result.Employee.Rol.RolName != null)
                            {
                                rol = result.Employee.Rol.RolType.ToString();
                            }

                            Guid custId = result.Employee.CustomerId;
                            Guid compId = result.Employee.CompanyId;
                            Guid emplId = result.Employee.Id;

                            custId = result.Employee.Id;
                            compId = Guid.Empty;
                            emplId = Guid.Empty;

                            string[] ids = [custId.ToString(), compId.ToString(), emplId.ToString(), rol.ToString()];
                            //HttpContext.Session.Set("ids", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ids)));
                            return [controller.ToString(), ids[0], ids[1],ids[2]];

                        }
                    }
                }
                else
                {
                    //return Json(result);
                    return null;
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> EnterToSystem(string email, string password)
        {
            List<string> list = await CheckCustomer(email, password);
            string[] ids = [list[1], list[2], list[3]];
            if (!string.IsNullOrEmpty(list[0]))
            {
                HttpContext.Session.Set("ids", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ids)));
                return RedirectToAction("StartPage", list[0]);
            }
            else
            {
                return RedirectToAction("StartPage", "Home");
            }
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
            return PartialView();
        }
        public async Task<IActionResult> StartPage()
        {
            await Task.Delay(10);
            return View();
            //return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogIn()
        {
            await Task.Delay(10);
            return View();
        }
        public async Task<IActionResult> Index()
        {
            await Task.Delay(10);
            return PartialView();
        }
         public async Task<IActionResult>  AboutUs()
        {
            await Task.Delay(10);
            return PartialView();
        }
         public async Task<IActionResult>  Contacts()
         {
            await Task.Delay(10);
            return PartialView();
         }
         public async Task<IActionResult>  Configuration()
         {
            await Task.Delay(10);
            return PartialView();
         }
       
        [HttpGet]
        public async Task<bool> HasDatabase()
        {
            await Task.Delay(10);
            bool result = Database.IsExist(Context.serverName, Context.dbName, Context.isUseDatabaseCredential,Context.login,Context.password);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsDatabaseAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            await Task.Delay(10);
            bool result = Database.IsDatabaseAvailable(serverName, databaseName, isUseDatabaseCredential, login, password);
            return result;
        }
        
        [HttpGet]
        public async Task<bool> IsTablesAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            bool result = await new Database().IsTablesAvailable(serverName, databaseName, isUseDatabaseCredential, login, password);
            return result;
        }

        [HttpGet]
        public async Task<bool> IsServerAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            await Task.Delay(10);
            return Database.IsServerAvailable(serverName, databaseName, isUseDatabaseCredential, login, password);
        }

        [HttpGet]
        public async Task<string[]> GetConnectionString()
        {
            await Task.Delay(10);
            string[] result = [Context.serverName.Replace("\\\\", "\\"), Context.dbName, Context.isUseDatabaseCredential, Context.login, Context.password];
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