using ReportingSystem.Models.Company;

namespace ReportingSystem.Models.Customer
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public CustomerLicenceStatusModel? StatusLicence { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime EndTimeLicense { get; set; }
        public DateTime DateRegistration { get; set; }
        public List<CompanyModel>? Companies { get; set; }
        public List<CustomerLicenseOperationModel>? HistoryOperations { get; set; } = [];
        public CustomerConfigModel? Configure{ get; set; }
    }
}


