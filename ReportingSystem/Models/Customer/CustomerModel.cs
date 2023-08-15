using ReportingSystem.Enum;
using ReportingSystem.Models.Company;

namespace ReportingSystem.Models.Customer
{
    public class CustomerModel
    {
        public Guid id { get; set; }
        public string? firstName { get; set; }
        public string? secondName { get; set; }
        public string? thirdName { get; set; }
        public CustomerLicenceStatusModel? statusLicence { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? status { get; set; }
        public string? password { get; set; }
        public DateTime endTimeLicense { get; set; }
        public DateTime dateRegistration { get; set; }
        public List<CompanyModel>? companies { get; set; }
        public List<CustomerLicenseOperationModel>? historyOperations { get; set; } = new List<CustomerLicenseOperationModel>();
    }
}


