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
            var employees = _employeesService.GetEmployees(id);
            return Json(employees);
        }

        //редагування/додавання посад з попередженнями про наявних співробітників на цих посадах
        //редагування

        //[HttpGet]
        //public async Task<IActionResult> GetAllPositionsFromCompany(string id)
        //{
        //    await Task.Delay(10);
        //    var employees = _employeesService.GetPositions(id);
        //    return Json(employees);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetAllRols(string id)
        //{
        //    await Task.Delay(10);
        //    var employees = _employeesService.GetRol(id);
        //    return Json(employees);
        //}

        //[HttpPost]
        //public async Task<IActionResult> GeneratePassword(string id)
        //{
        //    await Task.Delay(10);
        //    var employees = _employeesService.GeneratePassword(id);
        //    return Json(employees);
        //}

        //групове надсилання листа на пошту
        //групове надсилання паролів

        //[HttpGet]
        //public async Task<IActionResult> GetEmployeesByOwnId(string id)
        //{
        //    await Task.Delay(10);
        //    var employees = _employeesService.GetEmployeesByOwnId(id);
        //    return Json(employees);
        //}

        [HttpPost]
        public async Task<IActionResult> EditEmployee([FromBody] Object employee)
        {
            await Task.Delay(10);
            var result = _employeesService.EditEmployee(employee);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
