namespace ReportingSystem.Models.Report
{
    public class ReportModel
    {
        public Guid Id { get; set; }
        public Guid IdCustomer { get; set; }
        public Guid IdCompany { get; set; }
        public Guid IdEmployee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid IdCategory0 { get; set; }
        public Guid IdCategory1 { get; set; }
        public Guid IdCategory2 { get; set; }
        public Guid IdCategory3 { get; set; }
        public Guid IdGroup { get; set; }
        public Guid IdProject { get; set; }
        public string? Comment { get; set; }
    }
}
