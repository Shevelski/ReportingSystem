using ReportingSystem.Enums;
using ReportingSystem.Models.User;

namespace ReportingSystem.Models.Customer
{
    public class CustomerLicenseOperationModel
    {
        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public DateTime DateChange { get; set; }
        public DateTime OldEndDateLicence { get; set; }
        public DateTime NewEndDateLicence { get; set; }
        public CustomerLicenceStatusModel? OldStatus { get; set; }
        public CustomerLicenceStatusModel? NewStatus { get; set; }
        public Double Price { get; set; }
        public string? Period { get; set; }
        public string? NameOperation { get; set; }
    }
}
