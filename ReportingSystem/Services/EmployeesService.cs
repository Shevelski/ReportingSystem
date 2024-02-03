using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class EmployeesService
    {
        bool mode = Settings.Source().Equals("json");
        public async Task<List<EmployeeModel>?> GetEmployees(Guid idCu, Guid idCo)
        {
            return mode ? await new JsonRead().GetEmployees(idCu, idCo) : await new SQLRead().GetEmployees(idCu, idCo);
        }

        public async Task<List<EmployeeModel>?> GetAdministrators()
        {
            return mode ? await new JsonRead().GetAdministrators() : await new SQLRead().GetAdministrators();
        }

        public async Task<EmployeeModel?> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            return mode ? await new JsonRead().GetEmployee(idCu, idCo, idEm) : await new SQLRead().GetEmployee(idCu, idCo, idEm);
        }

        //Редагування співробітника
        public async Task EditEmployee(Object employeeInput)
        {
            await (mode ? new JsonWrite().EditEmployee(employeeInput) : new SQLWrite().EditEmployee(employeeInput));
        }

        //Редагування співробітника
        public async Task EditAdministrator(Object employeeInput)
        {
            await (mode ? new JsonWrite().EditAdministrator(employeeInput) : new SQLWrite().EditAdministrator(employeeInput));
        }

        // Архівування співробітників
        public async Task ArchiveEmployee(string[] ar)
        {
            await (mode ? new JsonWrite().ArchiveEmployee(ar) : new SQLWrite().ArchiveEmployee(ar));
        }

        // Архівування співробітників
        public async Task ArchiveAdministrator(string[] ar)
        {
            await (mode ? new JsonWrite().ArchiveAdministrator(ar) : new SQLWrite().ArchiveAdministrator(ar));
        }

        // Перевірка на доступність email
        public async Task<bool> IsBusyEmail(string email)
        {
            return mode ? await new JsonRead().IsBusyEmail(email) : await new SQLRead().IsBusyEmail(email);   
        }

        // Додавання співробітників
        public async Task CreateEmployee(string[] ar)
        {
            await (mode ? new JsonWrite().CreateEmployee(ar) : new SQLWrite().CreateEmployee(ar));
        }

        // Додавання співробітників
        public async Task CreateAdministrator(string[] ar)
        {
            await (mode ? new JsonWrite().CreateAdministrator(ar) : new SQLWrite().CreateAdministrator(ar));
        }

        // Відновлення співробітників з архіву
        public async Task FromArchiveEmployee(string[] ar)
        {
            await (mode ? new JsonWrite().FromArchiveEmployee(ar) : new SQLWrite().FromArchiveEmployee(ar));
        }

        // Відновлення співробітників з архіву
        public async Task FromArchiveAdministrator(string id)
        {
            await (mode ? new JsonWrite().FromArchiveAdministrator(id) : new SQLWrite().FromArchiveAdministrator(id));
        }

        // Видалення співробітників з системи
        public async Task DeleteEmployee(string[] ar)
        {
            await (mode ? new JsonWrite().DeleteEmployee(ar) : new SQLWrite().DeleteEmployee(ar));
        }

        // Видалення співробітників з системи
        public async Task DeleteAdministrator(string id)
        {
            await (mode ? new JsonWrite().DeleteAdministrator(id) : new SQLWrite().DeleteAdministrator(id));
        }
    }
}
