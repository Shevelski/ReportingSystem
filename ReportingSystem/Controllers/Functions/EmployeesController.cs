﻿using Microsoft.AspNetCore.Mvc;
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
            await Task.Delay(0);
            using (_employeesService as IDisposable)
            {
                var employees = _employeesService.GetEmployees(idCu, idCo);
                return Json(employees);
            }
        }


        [HttpPost]
        // Архівування співробітників
        public async Task<IActionResult> ArchiveEmployee([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _employeesService.ArchiveEmployee(ar[0], ar[1], ar[2]);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpPost]
        // Відновлення співробітників з архіву
        public async Task<IActionResult> FromArchiveEmployee([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _employeesService.FromArchiveEmployee(ar[0], ar[1], ar[2]);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        // Видалення співробітників з системи
        public async Task<IActionResult> DeleteEmployee([FromBody] string[] ar)
        {
            await Task.Delay(10);
            _employeesService.DeleteEmployee(ar[0], ar[1], ar[2]);
            //return result != null ? Ok(result) : NotFound();
            return Ok();
        }

        [HttpPost]
        // Додавання нового співробітника
        public async Task<IActionResult> CreateEmployee([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _employeesService.CreateEmployee(ar);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet]
        // Перевірка вільності email
        public async Task<IActionResult> IsBusyEmail(string email)
        {
            await Task.Delay(10);
            var result = _employeesService.IsBusyEmail(email);
            return Json(result);
            //return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        //Редагування співробітника
        public async Task<IActionResult> EditEmployee([FromBody] object employee)
        {
            await Task.Delay(10);
            var result = _employeesService.EditEmployee(employee);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
