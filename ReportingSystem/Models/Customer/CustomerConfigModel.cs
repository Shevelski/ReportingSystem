namespace ReportingSystem.Models.Customer
{
    public class CustomerConfigModel
    {
        public bool IsSaveCustomer { get; set; }
        public Guid IdSavedCustomer { get; set; }
        public bool IsSaveCompany {  get; set; }
        public Guid IdSavedCompany { get; set; }
    }
}