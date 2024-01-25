using Newtonsoft.Json;

namespace ReportingSystem.Models.Settings
{
    public class LogLevel
    {
        public string Default { get; set; }

        [JsonProperty("Microsoft.AspNetCore")] // Додайте атрибут для вказівки імені в JSON
        public string MicrosoftAspNetCore { get; set; }
    }

    public class Logging
    {
        [JsonProperty("LogLevel")] // Додайте атрибут для вказівки імені в JSON
        public LogLevel LogLevel { get; set; }
    }

}
