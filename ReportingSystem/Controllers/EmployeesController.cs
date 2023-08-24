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
    public class EmployeesController : Controller
    {

        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;
        }


        [HttpGet]
        public async Task<IActionResult> CheckEmployeeEmail(string email)
        {
            await Task.Delay(10);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmployeePassword(string password)
        {
            await Task.Delay(10);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(string id)
        {
            await Task.Delay(10);
            var employees = _employeesService.GetEmployees( id);
            return Json(employees);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee([FromBody] Object employee)
        {
            await Task.Delay(10);
            var employees = _employeesService.EditEmployee(employee);
            return Json(employees);
        }
    }
}
