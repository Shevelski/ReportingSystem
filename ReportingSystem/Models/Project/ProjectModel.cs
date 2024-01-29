using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Project
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double ProjectCostsForCompany { get; set; }
        public double ProjectCostsForCustomer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatusModel? Status { get; set; }
        public List<EmployeePositionModel>? Positions { get; set; }
        public List<EmployeeModel>? Members { get; set; }
        public EmployeeModel? Head { get; set; }
        public ProjectChCategoryModel? Category { get; set; }
        public List<ProjectStepModel>? Steps { get; set; }
    }
}
