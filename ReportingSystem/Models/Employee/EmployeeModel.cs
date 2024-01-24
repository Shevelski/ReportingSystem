using ReportingSystem.Enums;

namespace ReportingSystem.Models.User
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? PhoneWork { get; set; }
        public string? PhoneSelf { get; set; }
        public string? EmailWork { get; set; }
        public string? EmailSelf { get; set; }
        public string? TaxNumber { get; set; }
        public string? AddressReg { get; set; }
        public string? AddressFact { get; set; }
        public string? Photo { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public double Salary { get; set; }
        public double AddSalary { get; set; }
        public EmployeeStatusModel? Status { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkEndDate { get; set; }
        public List<DateTime>? HolidayDate { get; set; }
        public List<DateTime>? HospitalDate { get; set; }
        public List<DateTime>? AssignmentDate { get; set; }
        public List<DateTime>? TaketimeoffDate { get; set; }
        public EmployeePositionModel? Position { get; set; }
        public EmployeeRolModel? Rol { get; set; }

    }
}
