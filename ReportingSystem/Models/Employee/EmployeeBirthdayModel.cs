namespace ReportingSystem.Models.Employee
{
    public class EmployeeBirthdayModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? PhoneWork { get; set; }
        public string? EmailWork { get; set; }
    }
}
