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
            await Task.Delay(10);
            var result = _projectsCategoriesService.GetCategories(idCu, idCo);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditNameCategory([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsCategoriesService.EditNameCategory(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsCategoriesService.CreateCategory(ar);
            return result != null ? Ok(result) : NotFound();

        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteCategory([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsCategoriesService.DeleteCategory(ar);
            return result != null ? Ok(result) : NotFound();

        }

    }
}
