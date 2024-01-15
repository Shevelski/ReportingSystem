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
            using (_customersService as IDisposable)
            {
                await Task.Delay(10);
                var result = await _customersService.GetCustomers();
                return Json(result);
            }
        }

        [HttpGet]
        //отримати замовників
        public async Task<IActionResult> GetCustomer(string idCu)
        {
            using (_customersService as IDisposable)
            {
                await Task.Delay(10);
                var result = _customersService.GetCustomer(idCu);
                return Json(result);
            }
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
        public async Task<IActionResult> RenewalLicence([FromBody] string[] ar)
        {
            var result = await _customersService.RenewalLicense(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        //ліцензія замовника - статус архів
        public async Task<IActionResult> ArchivingLicence([FromBody] string[] ar)
        {
            var result = await _customersService.ArchivingLicence(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        //видалення замовника
        public async Task<IActionResult> DeleteLicence([FromBody] string[] ar)
        {
            var result = await _customersService.DeleteLicence(ar);
            return Ok(result);

        }

        [HttpPost]
        //анулювання ліцензії замовника
        public async Task<IActionResult> CancellationLicence([FromBody] string[] ar)
        {
            var result = await _customersService.CancellationLicence(ar);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        //анулювання ліцензії замовника
        public async Task<IActionResult> EditCustomer([FromBody] string[] ar)
        {
            var result = await _customersService.EditCustomer(ar);
            return result != null ? Ok(result) : NotFound();
        }

    }
}
