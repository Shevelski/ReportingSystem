using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Company
{
    public class TeamModel
    {
        public Guid id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public string? name { get; set; }
        public Dictionary<EmployeePositionModel,EmployeeModel>? members { get; set; }
    }
}
