using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

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
            var result = _companiesService.GetCompanies();
            return Json(result);
        }

        
        [HttpGet]
        public async Task<IActionResult> CheckSave()
        {
            await Task.Delay(10);
            var result = _companiesService.CheckSave();
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetActualCompanies()
        {
            await Task.Delay(10);
            var result = _companiesService.GetActualCompanies();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermanentCompany([FromBody] string id)
        {
            await Task.Delay(10);
            var result = _companiesService.SavePermanentCompany(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _companiesService.EditCompany(ar);
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> ArchiveCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _companiesService.ArchiveCompany(ar);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _companiesService.DeleteCompany(ar);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task PostCheckCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            _companiesService.PostCheckCompany(ar);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckCompany(string id)
        {
            await Task.Delay(10);
            var result = _companiesService.GetCheckCompany(id);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _companiesService.CreateCompany(ar);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
