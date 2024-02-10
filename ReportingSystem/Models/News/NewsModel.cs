using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Models.News
{
    public class NewsModel
    {
        public Guid Id { get; set; }
        public string? Header { get; set; }
        public string? Url { get; set; }
    }
}
