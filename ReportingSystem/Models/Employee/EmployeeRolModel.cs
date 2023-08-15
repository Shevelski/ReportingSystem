using ReportingSystem.Models.User;
using ReportingSystem.Enum;

namespace ReportingSystem.Models
{
    public class EmployeeRolModel
    {
        public EmployeeRolStatus rolType { get; set; }
        public string? rolName { get; set; }
        public List<EmployeeModel>? rolsEmployeeStatus { get; set; }
        public List<string>? rolPermissions { get; set; }
    }
}
