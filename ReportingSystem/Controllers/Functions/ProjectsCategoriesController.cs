using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ProjectsCategoriesController(ProjectsCategoriesService projectsCategoriesService) : Controller
    {

        private readonly ProjectsCategoriesService _projectsCategoriesService = projectsCategoriesService;

        [HttpGet]
        public async Task<IActionResult> GetCategories(string idCu, string idCo)
        {
            var result = await _projectsCategoriesService.GetCategories(idCu, idCo);
            return Json(result);
        }

        [HttpPost]
        public async Task EditNameCategory([FromBody] string[] ar)
        {
            await _projectsCategoriesService.EditNameCategory(ar);
        }

        [HttpPost]
        public async Task CreateCategory([FromBody] string[] ar)
        {
            await _projectsCategoriesService.CreateCategory(ar);
        }
        
        [HttpPost]
        public async Task DeleteCategory([FromBody] string[] ar)
        {
            await _projectsCategoriesService.DeleteCategory(ar);
        }

    }
}
