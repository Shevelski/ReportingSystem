using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Employee
{
    public class EmployeesPositionsModel
    {
        public string? NamePosition { get; set; }
        public List<EmployeeModel>? Employees { get; set; }
    }
}
