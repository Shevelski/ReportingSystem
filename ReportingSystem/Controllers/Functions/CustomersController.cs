using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Company;
using ReportingSystem.Services;
using ReportingSystem.Controllers.Users;
using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Models.User;
using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ReportingSystem.Controllers.Functions
{

    public class CustomersController(CustomersService customersService) : Controller
    {
        private readonly CustomersService _customersService = customersService;

        [HttpGet]
        //отримати замовників
        public async Task<IActionResult> GetCustomers()
        {
            //using (_customersService as IDisposable)
            //{
                await Task.Delay(10);
                var result = await _customersService.GetCustomers();
                return Json(result);
            //}
        }

        [HttpGet]
        //отримати замовника
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            return await _customersService.GetCustomer(idCu);
        }

        [HttpPost]
        //створити замовника - використовується з лендінга для створення, треба прикріпити до кнопки
        public Task<bool> RegistrationCustomer([FromBody] string[] ar)
        {
            return _customersService.RegistrationCustomer(ar);
        }


        [HttpPost]
        //продовження ліцензії замовника
        public async Task RenewalLicence([FromBody] string[] ar)
        {
            await _customersService.RenewalLicense(ar);
        }

        [HttpPost]
        //ліцензія замовника - статус архів
        public async Task ArchivingLicence([FromBody] string[] ar)
        {
            await _customersService.ArchivingLicence(ar);
        }

        [HttpPost]
        //видалення замовника
        public async Task DeleteLicence([FromBody] string[] ar)
        {
            await _customersService.DeleteLicence(ar);
        }

        [HttpPost]
        //анулювання ліцензії замовника
        public async Task CancellationLicence([FromBody] string[] ar)
        {
            await _customersService.CancellationLicence(ar);
        }

        [HttpPost]
        //анулювання ліцензії замовника
        public async Task EditCustomer([FromBody] string[] ar)
        {
            await _customersService.EditCustomer(ar);
        }

    }
}
