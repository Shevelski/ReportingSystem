using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using System;
using ReportingSystem.Models.Company;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers
{

    public class CustomersController : Controller
    {
        private readonly CustomersService _customersService;

        public CustomersController(CustomersService customersService)
        {
            _customersService = customersService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLicence()
        {
            await Task.Delay(10);
            var customers = _customersService.GetCustomers();
            return Json(customers);
        }


        [HttpPost]
        public IActionResult CreateCustomer([FromBody] string[] ar)
        {
            var result = _customersService.CreateCustomer(ar[0]);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> RenewalLicence([FromBody] string[] ar)
        {
            var result = await _customersService.RenewalLicense(ar);
            return result != null ? (IActionResult)Ok(result) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> ArchivingLicence([FromBody] string[] ar)
        {
            var result = await _customersService.ArchivingLicence(ar);
            return result != null ? (IActionResult)Ok(result) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> CancellationLicence([FromBody] string[] ar)
        {
            var result = await _customersService.CancellationLicence(ar);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

    }
}
