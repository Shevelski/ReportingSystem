using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Project
{
    public class ProjectModel
    {
        public Guid id { get; set; }
        public Guid companyId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public double projectCostsForCompany { get; set; }
        public double projectCostsForCustomer { get; set; }
        public DateTime startDate { get; set; }
        public DateTime planDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusModel? status { get; set; }
        public List<EmployeePositionModel>? positions { get; set; }
        public List<EmployeeModel>? members { get; set; }
        public EmployeeModel? head { get; set; }
        public ProjectChCategoryModel? category { get; set; }
        public List<ProjectStepModel>? steps { get; set; }
    }
}
