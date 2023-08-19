using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using ReportingSystem.Utils;

namespace ReportingSystem.Controllers
{

    public class CompaniesController : Controller
    {
        private readonly CompaniesService _companiesService;

        public CompaniesController(CompaniesService companiesService)
        {
            _companiesService = companiesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {

            await Task.Delay(10);
            var companies = _companiesService.GetCompanies();
            return Json(companies);
        }

        [HttpGet]
        public async Task<IActionResult> GetActualCompanies()
        {
            await Task.Delay(10);
            var companies = _companiesService.GetActualCompanies();
            return Json(companies);
        }


        [HttpPost]
        public async Task<IActionResult> EditCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var company = _companiesService.EditCompany(ar);
            return Json(company);
        }


        [HttpPost]
        public async Task<IActionResult> ArchiveCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var company = _companiesService.ArchiveCompany(ar);
            return Json(company);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var company = _companiesService.DeleteCompany(ar);
            return Json(company);
        }

        //--------------------------------------------------------------------------------------------продовжити розділяти

        [HttpPost]
        public async Task<IActionResult> PostCheckCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var company = _companiesService.PostCheckCompany(ar);
            return Json(company);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckCompany(string id)
        {
            await Task.Delay(10);
            var company = _companiesService.GetCheckCompany(id);
            return Json(company);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var company = _companiesService.CreateCompany(ar);
            return Json(company);
        }
    }
}
