using Newtonsoft.Json;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Company
{
    public class CompanyModel
    {
        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Code { get; set; }
        public string? Actions { get; set; }
        public string? StatusWeb { get; set; }
        public CompanyStatusModel? Status { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? StatutCapital { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<EmployeeRolModel>? Rolls { get; set; }
        public EmployeeModel? Chief { get; set; }
        public List<EmployeePositionModel>? Positions { get; set; }
        public List<ProjectModel>? Projects { get; set; }
        public List<EmployeeModel>? Employees { get; set; }
        public List<ProjectCategoryModel>? Categories { get; set; }
    }
}
