using ReportingSystem.Enum;

namespace ReportingSystem.Models.User
{
    public class EmployeeModel
    {
        public Guid id { get; set; }
        public string? firstName { get; set; }
        public string? secondName { get; set; }
        public string? thirdName { get; set; }
        public string? phoneWork { get; set; }
        public string? phoneSelf { get; set; }
        public string? emailWork { get; set; }
        public string? emailSelf { get; set; }
        public string? taxNumber { get; set; }
        public string? addressReg { get; set; }
        public string? addressFact { get; set; }
        public string? photo { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        public double salary { get; set; }
        public double addSalary { get; set; }
        public EmployeeStatusModel? status { get; set; }
        public DateTime birthDate { get; set; }
        public DateTime workStartDate { get; set; }
        public DateTime workEndDate { get; set; }
        public List<DateTime>? holidayDate { get; set; }
        public List<DateTime>? hospitalDate { get; set; }
        public List<DateTime>? assignmentDate { get; set; }
        public List<DateTime>? taketimeoffDate { get; set; }
        public EmployeePositionModel? position { get; set; }
        public EmployeeRolModel? rol { get; set; }

    }
}
