using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ProjectsController : Controller
    {

        private readonly ProjectsService _projectsService;

        public ProjectsController(ProjectsService projectsService)
        {
            _projectsService = projectsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects(string idCu, string idCo)
        {
            var result = await _projectsService.GetProjects(idCu, idCo);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditProject([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsService.EditProject(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsService.CreateProject(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsService.DeleteProject(ar);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ArchiveProject([FromBody] string[] ar)
        {

            await Task.Delay(10);
            var result = _projectsService.DeleteProject(ar);
            return result != null ? Ok(result) : NotFound();

        }

    }

    
}
