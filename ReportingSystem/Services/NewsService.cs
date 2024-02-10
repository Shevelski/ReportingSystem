using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.News;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class NewsService
    {
        public async Task<List<NewsModel>> GetNews(string category)
        {
            await Task.Delay(10);
            return new News().GetNews(category);
        }
        public async Task<List<string>> GetCategoriesNews()
        {
            await Task.Delay(10);
            return new News().GetCategoriesNews();
        }


        
    }
}
