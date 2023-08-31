using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using ReportingSystem.Enums;
using ReportingSystem.Models;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ReportingSystem.Controllers
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


        public IActionResult Authorize(bool authorizeOK)
        {

            //return RedirectToAction("Index", "EUProjectManager");
            return View();
            //if (authorizeOK)
            //{
            //    return RedirectToAction("Index", "EUProjectManager");
            //}
            //return View();
        }

        [HttpPost]
        public IActionResult SignIn([FromBody] AuthorizeModel authorizeModel)
        {
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
                    var result1 = _authorizeService.GetRolController(result);

                    if (!string.IsNullOrEmpty(result1))
                    {
                        return RedirectToAction("Index", "EUProjectManager");
                        //return RedirectToAction("Index", result1);
                        //return Redirect();
                    }
                }
                else
                {
                    return Json(result);
                }
            }
            return Json(result);
        }

        public IActionResult GoToRolController(AuthorizeModel authorizeModel)
        {
            var result = _authorizeService.GetRolController(authorizeModel);

            if (!string.IsNullOrEmpty(result))
            {
                return RedirectToAction("Index", result, authorizeModel.Employee);
            }

            return View("ErrorView");
        }

        public IActionResult Redirect()
        {
            return Authorize(true);   
        }



        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //public IActionResult Employee()
        //{
        //    return View();
        //}

        //public IActionResult Customers()
        //{
        //    return View();
        //}
        //public IActionResult Companies()
        //{
        //    return View();

        //}

        //public IActionResult Employees()
        //{
        //    return View();

        //}
        //public IActionResult Projects()
        //{
        //    return View();

        //}

        //public IActionResult ProjectsCategories()
        //{
        //    return View();

        //}

        //public IActionResult StepProjects()
        //{
        //    return View();
        //}


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