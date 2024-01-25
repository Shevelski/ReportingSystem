namespace ReportingSystem.Models.Settings
{
    public class RootObject
    {
        public Logging? Logging { get; set; }
        public string? AllowedHosts { get; set; }
        public Source? Source { get; set; }
        public ConnectionSettings? ConnectionSettings { get; set; }
    }
}
