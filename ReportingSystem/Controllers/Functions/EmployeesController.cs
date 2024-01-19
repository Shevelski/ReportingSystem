using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{
    public class EmployeesController(EmployeesService employeesService) : Controller
    {

        private readonly EmployeesService _employeesService = employeesService;

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetEmployees(Guid idCu, Guid idCo)
        {
            var employees = await _employeesService.GetEmployees(idCu, idCo);
            return Json(employees);
        }

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetAdministrators()
        {
            var employees = await _employeesService.GetAdministrators();
            return Json(employees);
        }

        [HttpGet]
        // отримання співробітників
        public async Task<IActionResult> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            var employee = await _employeesService.GetEmployee(idCu, idCo, idEm);
            return Json(employee);
        }

        [HttpPost]
        // Архівування співробітників
        public async Task ArchiveEmployee([FromBody] string[] ar)
        {
            await _employeesService.ArchiveEmployee(ar);
        }

        [HttpPost]
        // Архівування співробітників
        public async Task ArchiveAdministrator([FromBody] string[] ar)
        {
            await _employeesService.ArchiveAdministrator(ar);
        }


        [HttpPost]
        // Відновлення співробітників з архіву
        public async Task FromArchiveEmployee([FromBody] string[] ar)
        {
           await _employeesService.FromArchiveEmployee(ar);
        }


        [HttpPost]
        // Відновлення співробітників з архіву
        public async Task FromArchiveAdministrator([FromBody] string[] ar)
        {
            await _employeesService.FromArchiveAdministrator(ar[0]);
        }

        [HttpPost]
        // Видалення співробітників з системи
        public async Task DeleteEmployee([FromBody] string[] ar)
        {
            await _employeesService.DeleteEmployee(ar);
        }

        [HttpPost]
        // Видалення співробітників з системи
        public async Task DeleteAdministrator([FromBody] string[] ar)
        {
            await _employeesService.DeleteAdministrator(ar[0]);
        }

        [HttpPost]
        // Додавання нового співробітника
        public async Task CreateEmployee([FromBody] string[] ar)
        {
            await _employeesService.CreateEmployee(ar);
        }

        [HttpPost]
        // Додавання нового співробітника
        public async Task CreateAdministrator([FromBody] string[] ar)
        {
            await _employeesService.CreateAdministrator(ar);
        }

        [HttpGet]
        // Перевірка вільності email
        public async Task IsBusyEmail(string email)
        {
            await _employeesService.IsBusyEmail(email);
        }

        [HttpPost]
        //Редагування співробітника
        public async Task EditEmployee([FromBody] object employee)
        {
            await _employeesService.EditEmployee(employee);
        }

        [HttpPost]
        //Редагування співробітника
        public async Task EditAdministrator([FromBody] object employee)
        {
            await _employeesService.EditAdministrator(employee);
        }
    }
}
