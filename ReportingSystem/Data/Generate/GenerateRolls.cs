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

            if (position.namePosition == positions[0] || position.namePosition == positions[2])
            {
                rol.rolType = EmployeeRolStatus.CEO;
                rol.rolName = EmployeeRolStatus.CEO.GetDisplayName();
                return rol;
            }
            if (position.namePosition == positions[1])
            {
                rol.rolType = EmployeeRolStatus.Administrator;
                rol.rolName = EmployeeRolStatus.Administrator.GetDisplayName();
            }
            if (position.namePosition == positions[3] || position.namePosition == positions[4] || position.namePosition == positions[5] || position.namePosition == positions[6])
            {
                rol.rolType = EmployeeRolStatus.ProjectManager;
                rol.rolName = EmployeeRolStatus.ProjectManager.GetDisplayName();
            }
            if (position.namePosition == positions[7] || position.namePosition == positions[8] || position.namePosition == positions[9])
            {
                rol.rolType = EmployeeRolStatus.User;
                rol.rolName = EmployeeRolStatus.User.GetDisplayName();
            }
            return rol;
        }
    }
}
