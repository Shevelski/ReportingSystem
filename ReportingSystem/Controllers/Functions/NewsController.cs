using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{
    public class NewsController(NewsService newsService) : Controller
    {
        private readonly NewsService _newsService = newsService;

        [HttpGet]
        // отримання новин
        public async Task<IActionResult> GetNews(string category)
        {
            var news = await _newsService.GetNews(category);
            return Json(news);
        }

        [HttpGet]
        // отримання категорій новин
        public async Task<IActionResult> GetCategoriesNews()
        {
            var categories = await _newsService.GetCategoriesNews();
            return Json(categories);
        }
    }
}
