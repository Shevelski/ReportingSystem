using ReportingSystem.Enum;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Customer
{
    public class CustomerLicenseOperationModel
    {
        public Guid id { get; set; }
        public Guid idCustomer { get; set; }
        public DateTime dateChange { get; set; }
        public DateTime oldEndDateLicence { get; set; }
        public DateTime newEndDateLicence { get; set; }
        public CustomerLicenceStatusModel? oldStatus { get; set; }
        public CustomerLicenceStatusModel? newStatus { get; set; }
        public Double price { get; set; }
        public string? period { get; set; }
        public string? nameOperation { get; set; }
    }
}
