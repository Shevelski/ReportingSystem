using Newtonsoft.Json;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Company
{
    public class CompanyModel
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? code { get; set; }
        public string? actions { get; set; }
        public string? statusWeb { get; set; }
        public CompanyStatusModel? status { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? statutCapital { get; set; }
        public DateTime registrationDate { get; set; }
        public List<EmployeeRolModel>? rolls { get; set; }
        public EmployeeModel? chief { get; set; }
        public List<EmployeePositionModel>? positions { get; set; }
        public List<ProjectModel>? projects { get; set; }
        public List<EmployeeModel>? employees { get; set; }
        public List<ProjectCategoryModel>? categories { get; set; }
    }
}
