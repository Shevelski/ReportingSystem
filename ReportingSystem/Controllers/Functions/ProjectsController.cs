using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ProjectsController(ProjectsService projectsService) : Controller
    {

        private readonly ProjectsService _projectsService = projectsService;

        [HttpGet]
        public async Task<IActionResult> GetProjects(string idCu, string idCo)
        {
            var result = await _projectsService.GetProjects(idCu, idCo);
            return Json(result);
        }

        [HttpPost]
        public async Task EditProject([FromBody] string[] ar)
        {
            await _projectsService.EditProject(ar);
        }

        [HttpPost]
        public async Task CreateProject([FromBody] string[] ar)
        {
            await _projectsService.CreateProject(ar);
        }

        [HttpPost]
        public async Task DeleteProject([FromBody] string[] ar)
        {
            await _projectsService.DeleteProject(ar);
        }

        [HttpPost]
        public async Task ArchiveProject([FromBody] string[] ar)
        {
            await _projectsService.DeleteProject(ar);

        }

    }

    
}
