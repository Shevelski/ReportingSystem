using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.News;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class NewsService
    {
        public async Task<List<NewsModel>> GetNews(string url)
        {

            await Task.Delay(10);
            return new News().GetNews(url);
        }
        public async Task<List<CategoryNewsModel>> GetCategoriesNews()
        {
            await Task.Delay(10);
            return new News().GetCategoriesNews();
        }


        
    }
}
