using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Services;
using ReportingSystem.Utils;

namespace ReportingSystem.Controllers.Functions
{
    public class EmployeesController : Controller
    {

        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetEmployees(string idCu, string idCo)
        {
            await Task.Delay(10);
            var employees = _employeesService.GetEmployees(idCu, idCo);
            return Json(employees);
        }

        //[HttpPost]
        //// Видалення співробітників
        //public async Task<IActionResult> DeleteEmployee(string idCu, string idCo, string idEm)
        //{
        //    await Task.Delay(10);
        //    var result = _employeesService.DeleteEmployee(idCu, idCo, idEm);
        //    return result != null ? Ok(result) : NotFound();
        //}

        [HttpPost]
        // Архівування співробітників
        public async Task<IActionResult> ArchiveEmployee(string idCu, string idCo, string idEm)
        {
            await Task.Delay(10);
            var result = _employeesService.ArchiveEmployee(idCu, idCo, idEm);
            return result != null ? Ok(result) : NotFound();
        }

        //[HttpPost]
        //// Відновлення співробітників з архіву
        //public async Task<IActionResult> FromArchiveEmployee(string idCu, string idCo, string idEm)
        //{
        //    await Task.Delay(10);
        //    var result = _employeesService.FromArchiveEmployee(idCu, idCo, idEm);
        //    return result != null ? Ok(result) : NotFound();
        //}

        //[HttpPost]
        //// Додавання нового співробітника
        //public async Task<IActionResult> CreateEmployee(string idCu, string idCo)
        //{
        //    await Task.Delay(10);
        //    var result = _employeesService.CreateEmployee(idCu, idCo);
        //    return result != null ? Ok(result) : NotFound();
        //}

        [HttpPost]
        public async Task<IActionResult> EditEmployee([FromBody] object employee)
        {
            await Task.Delay(10);
            var result = _employeesService.EditEmployee(employee);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
