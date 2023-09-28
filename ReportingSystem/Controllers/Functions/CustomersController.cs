using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Company;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
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
        //отримати замовників
        public async Task<IActionResult> GetCustomers()
        {
            using (_customersService as IDisposable)
            {
                await Task.Delay(10);
                var result = _customersService.GetCustomers();
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

        //[HttpPost]
        ////створити замовника - використовується з лендінга для створення, треба прикріпити до кнопки
        //public IActionResult CreateCustomer([FromBody] string[] ar)
        //{
        //    var result = _customersService.CreateCustomer(ar[0]);
        //    return result != null ? Ok(result) : NotFound();
        //}

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
        //анулювання ліцензії замовника
        public async Task<IActionResult> CancellationLicence([FromBody] string[] ar)
        {
            var result = await _customersService.CancellationLicence(ar);
            return result != null ? Ok(result) : NotFound();
        }

        //[HttpGet]
        ////перевірка збережених компаній
        //public async Task<IActionResult> GetCustomerInfo(string idCu)
        //{
        //    await Task.Delay(10);
        //    var result = _customersService.GetCustomerInfo(idCu);
        //    return Json(result);
        //}

        //[HttpPost]
        ////перевірка збережених компаній
        //public async Task<IActionResult> SetCustomerInfo(string idCu)
        //{
        //    await Task.Delay(10);
        //    var result = _customersService.SetCustomerInfo(idCu);
        //    return Json(result);
        //}

    }
}
