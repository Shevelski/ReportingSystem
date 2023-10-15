using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Project.Step
{
    public class ProjectStepModel
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime planDate { get; set; }
        public DateTime endDate { get; set; }
        public ProjectStatusModel? status { get; set; }
        public List<EmployeePositionModel>? positions { get; set; }
        public List<EmployeeModel>? members { get; set; }
    }
}
