using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Models.News
{
    public class CategoryNewsModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public List<NewsModel> news { get; set; }
    }
}
