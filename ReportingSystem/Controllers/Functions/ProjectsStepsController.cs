using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ProjectsStepsController(ProjectsStepsService projectsStepsService) : Controller
    {

        private readonly ProjectsStepsService _projectsStepsService = projectsStepsService;

        [HttpGet]
        public async Task<IActionResult> GetStepsProjects(string idCu, string idCo, string idPr)
        {
            var result = await _projectsStepsService.GetStepsProjects(idCu, idCo, idPr);
            return Json(result);
        }

        //[HttpPost]
        //public async Task EditNameCategory([FromBody] string[] ar)
        //{
        //    await _projectsCategoriesService.EditNameCategory(ar);
        //}

        //[HttpPost]
        //public async Task CreateCategory([FromBody] string[] ar)
        //{
        //    await _projectsCategoriesService.CreateCategory(ar);
        //}

        //[HttpPost]
        //public async Task DeleteCategory([FromBody] string[] ar)
        //{
        //    await _projectsCategoriesService.DeleteCategory(ar);
        //}

    }
}
