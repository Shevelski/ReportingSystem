using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class CompaniesController(CompaniesService companiesService) : Controller
    {
        private readonly CompaniesService _companiesService = companiesService;

        [HttpGet]
        //отримати компанії вказаного замовника
        public async Task<IActionResult> GetCompanies(string idCu)
        {
            var result = await _companiesService.GetCompanies(idCu);
            return Json(result);   
        }

        [HttpGet]
        //отримати компанії з статусом актуальні вказаного замовника
        public async Task<IActionResult> GetActualCompanies(string idCu)
        {
            var result = await _companiesService.GetActualCompanies(idCu);
            return Json(result);
        }

        [HttpGet]
        //отримати ролі вибраної компанії вибраного замовника 
        public async Task<IActionResult> GetRolls(string idCu, string idCo)
        {
            var result = await _companiesService.GetRolls(idCu, idCo);
            return Json(result);
        }

        [HttpGet]
        //отримати ролі вибраної компанії вибраного замовника 
        public async Task<IActionResult> GetDevRolls()
        {
            var result = await _companiesService.GetDevRolls();
            return Json(result);
        }

        [HttpPost]
        //змінити компанію
        public async Task EditCompany([FromBody] string[] ar)
        {
            await _companiesService.EditCompany(ar);
        }


        [HttpPost]
        //архівація компанії
        public async Task ArchiveCompany([FromBody] string[] ar)
        {
            await _companiesService.ArchiveCompany(ar);
        }


        [HttpPost]
        //видалення компанії
        public async Task DeleteCompany([FromBody] string[] ar)
        {
            await _companiesService.DeleteCompany(ar);
        }

        [HttpPost]
        //перевірка єдрпу компанії при створенні - повернення даних про компанію
        public async Task PostCheckCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);
            _companiesService.PostCheckCompany(ar);
        }

        [HttpGet]
        //перевірка єдрпу компанії при створенні
        public async Task<IActionResult> GetCheckCompany(string id)
        {
            await Task.Delay(10);
            var result = _companiesService.GetCheckCompany(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        //створення компанії
        public async Task<IActionResult> CreateCompany([FromBody] string[] ar)
        {
            var result = await _companiesService.CreateCompany(ar);
            return Json(result);
        }
    }
}
