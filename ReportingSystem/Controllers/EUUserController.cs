using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ReportingSystem.Controllers
{
    public class EUUserController : Controller
    {

        private IActionResult GetIdsFromSession()
        {
            if (HttpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
            {
                var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));
                return View(ids);
            }
            else
            {
                return RedirectToAction("Home", "Authorize");
            }
        }
        public IActionResult Index()
        {
            return GetIdsFromSession();
        }

        public IActionResult Companies()
        {
            return GetIdsFromSession();
        }

        public IActionResult Categories()
        {
            return GetIdsFromSession();
        }
        public IActionResult Projects()
        {
            return GetIdsFromSession();
        }
        public IActionResult Employees()
        {
            return GetIdsFromSession();
        }
        public IActionResult Info()
        {
            return GetIdsFromSession();
        }
        public IActionResult Positions()
        {
            return GetIdsFromSession();
        }
        public IActionResult Report()
        {
            return GetIdsFromSession();
        }
    }
}
