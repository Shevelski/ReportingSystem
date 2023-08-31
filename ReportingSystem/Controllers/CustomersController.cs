using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
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

        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();


        [HttpGet]
        public async Task<IActionResult> GetAllLicence()
        {
            await Task.Delay(10);
            var result = _customersService.GetCustomers();
            return Json(result);
        }


        [HttpPost]
        public IActionResult CreateCustomer([FromBody] string[] ar)
        {
            var result = _customersService.CreateCustomer(ar[0]);
            return result != null ? Ok(result) : NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> RenewalLicence([FromBody] string[] ar)
        {
            var result = await _customersService.RenewalLicense(ar);
            return result != null ? Ok(result) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> ArchivingLicence([FromBody] string[] ar)
        {
            var result = await _customersService.ArchivingLicence(ar);
            return result != null ? Ok(result) : NotFound();

        }


        [HttpPost]
        public async Task<IActionResult> CancellationLicence([FromBody] string[] ar)
        {
            var result = await _customersService.CancellationLicence(ar);
            return result != null ? Ok(result) : NotFound();
        }

    }
}
