using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Company;
using ReportingSystem.Services;

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
        public async Task<IActionResult> GetCustomer(string idCu)
        {
            //using (_customersService as IDisposable)
            //{
                await Task.Delay(10);
                var result = _customersService.GetCustomer(idCu);
                return Json(result);
            //}
        }

        [HttpPost]
        //створити замовника - використовується з лендінга для створення, треба прикріпити до кнопки
        public IActionResult RegistrationCustomer([FromBody] string[] ar)
        {
            var result = _customersService.RegistrationCustomer(ar);
            return result != null ? Ok(result) : NotFound();
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
