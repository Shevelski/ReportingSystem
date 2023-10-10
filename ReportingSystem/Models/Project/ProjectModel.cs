using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Project
{
    public class ProjectModel
    {
        public Guid id { get; set; }
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
        public ProjectCategoryModel? categoryModel { get; set; }
        public ProjectCategoryModel1? categoryModel1{ get; set; }
        public ProjectCategoryModel2? categoryModel2 { get; set; }
        public ProjectCategoryModel3? categoryModel3 { get; set; }
        public List<ProjectStepModel>? steps { get; set; }
    }
}
