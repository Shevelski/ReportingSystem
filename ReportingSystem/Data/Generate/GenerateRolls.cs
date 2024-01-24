using ReportingSystem.Enums;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using ReportingSystem.Enums.Extensions;

namespace ReportingSystem.Data.Generate
{
    public class GenerateRolls
    {
        public EmployeeRolModel GenerateRol(EmployeePositionModel position)
        {
            EmployeeRolModel rol = new EmployeeRolModel();
            List<string> positions = new List<string>
            {
                "Директор",
                "Адміністратор",
                "Заступник директора",
                "Економіст",
                "Юрист",
                "HR",
                "Проект-менеджер",
                "Розробник",
                "Тестувальник",
                "Графічний дизайнер"
            };

            if (position.NamePosition == positions[0] || position.NamePosition == positions[2])
            {
                rol.RolType = EmployeeRolStatus.CEO;
                rol.RolName = EmployeeRolStatus.CEO.GetDisplayName();
                return rol;
            }
            if (position.NamePosition == positions[1])
            {
                rol.RolType = EmployeeRolStatus.Administrator;
                rol.RolName = EmployeeRolStatus.Administrator.GetDisplayName();
            }
            if (position.NamePosition == positions[3] || position.NamePosition == positions[4] || position.NamePosition == positions[5] || position.NamePosition == positions[6])
            {
                rol.RolType = EmployeeRolStatus.ProjectManager;
                rol.RolName = EmployeeRolStatus.ProjectManager.GetDisplayName();
            }
            if (position.NamePosition == positions[7] || position.NamePosition == positions[8] || position.NamePosition == positions[9])
            {
                rol.RolType = EmployeeRolStatus.User;
                rol.RolName = EmployeeRolStatus.User.GetDisplayName();
            }
            return rol;
        }
    }
}
