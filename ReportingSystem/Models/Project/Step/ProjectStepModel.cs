﻿using ReportingSystem.Models.Employee;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Project.Step
{
    public class ProjectStepModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatusModel? Status { get; set; }
        public List<EmployeePositionModel>? Positions { get; set; }
        public List<EmployeesPositionsModel>? EmpPositions { get; set; }
        public List<EmployeeModel>? Members { get; set; }
    }
}
