using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

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

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetAdministrators()
        {
            await Task.Delay(0);
            using (_employeesService as IDisposable)
            {
                var employees = _employeesService.GetAdministrators();
                return Json(employees);
            }
        }

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetEmployee(string idCu, string idCo, string idEm)
        {
            await Task.Delay(0);
            using (_employeesService as IDisposable)
            {
                var employee = _employeesService.GetEmployee(idCu, idCo, idEm);
                return Json(employee);
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

        [HttpPost]
        // Додавання нового співробітника
        public async Task<IActionResult> CreateAdministrator([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _employeesService.CreateAdministrator(ar);
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

        [HttpPost]
        //Редагування співробітника
        public async Task<IActionResult> EditAdministrator([FromBody] object employee)
        {
            await Task.Delay(10);
            var result = _employeesService.EditAdministrator(employee);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
