using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class EmployeesService
    {
        public async Task<List<EmployeeModel>?> GetEmployees(Guid idCu, Guid idCo)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetEmployees(idCu, idCo) :
                      await new SQLRead().GetEmployees(idCu, idCo);
            return result;
        }

        public async Task<List<EmployeeModel>?> GetAdministrators()
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetAdministrators() :
                      await new SQLRead().GetAdministrators();
            return result;
        }

        public async Task<EmployeeModel?> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetEmployee(idCu, idCo, idEm) :
                      await new SQLRead().GetEmployee(idCu, idCo, idEm);
            return result;
        }

        //Редагування співробітника
        public async Task EditEmployee(Object employeeInput)
        {
            await new JsonWrite().EditEmployee(employeeInput);
            await new SQLWrite().EditEmployee(employeeInput);
        }

        //Редагування співробітника
        public async Task EditAdministrator(Object employeeInput)
        {
            await new JsonWrite().EditAdministrator(employeeInput);
            await new SQLWrite().EditAdministrator(employeeInput);
        }

        // Архівування співробітників
        public async Task ArchiveEmployee(string[] ar)
        {
            await new JsonWrite().ArchiveEmployee(ar);
            await new SQLWrite().ArchiveEmployee(ar);
        }

        // Архівування співробітників
        public async Task ArchiveAdministrator(string[] ar)
        {
            await new JsonWrite().ArchiveAdministrator(ar);
            await new SQLWrite().ArchiveAdministrator(ar);
        }

        // Перевірка на доступність email
        public async Task<bool> IsBusyEmail(string email)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().IsBusyEmail(email) :
                      await new SQLRead().IsBusyEmail(email);
            return result;   
        }

        // Додавання співробітників
        public async Task CreateEmployee(string[] ar)
        {
            await new JsonWrite().CreateEmployee(ar);
            await new SQLWrite().CreateEmployee(ar);
        }

        // Додавання співробітників
        public async Task CreateAdministrator(string[] ar)
        {
            await new JsonWrite().CreateAdministrator(ar);
            await new SQLWrite().CreateAdministrator(ar);
        }

        // Відновлення співробітників з архіву
        public async Task FromArchiveEmployee(string[] ar)
        {
            await new JsonWrite().FromArchiveEmployee(ar);
            await new SQLWrite().FromArchiveEmployee(ar);
        }

        // Відновлення співробітників з архіву
        public async Task FromArchiveAdministrator(string id)
        {
            await new JsonWrite().FromArchiveAdministrator(id);
            await new SQLWrite().FromArchiveAdministrator(id);
        }

        // Видалення співробітників з системи
        public async Task DeleteEmployee(string[] ar)
        {
            await new JsonWrite().DeleteEmployee(ar);
            await new SQLWrite().DeleteEmployee(ar);
        }

        // Видалення співробітників з системи
        public async Task DeleteAdministrator(string id)
        {
            await new JsonWrite().DeleteAdministrator(id);
            await new SQLWrite().DeleteAdministrator(id);
        }
    }
}
