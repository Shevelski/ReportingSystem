using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers
{

    public class ProjectsCategoriesController : Controller
    {

        private readonly ProjectsCategoriesService _projectsCategoriesService;

        public ProjectsCategoriesController(ProjectsCategoriesService projectsCategoriesService)
        {
            _projectsCategoriesService = projectsCategoriesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {

            await Task.Delay(10);
            var result = _projectsCategoriesService.GetCategories();
            return Json(result);
        }



        [HttpPost]
        public async Task<IActionResult> EditNameCategory([FromBody] int[] levels, string newName, Guid idLevel1)
        {
            await Task.Delay(10);
            ProjectCategoryModel? categoryLevel1 = new ProjectCategoryModel();
            if (DatabaseMoq.ProjectsCategories != null)
            {
                categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(idLevel1));
            }
            
            if (categoryLevel1 != null)
            {
                if (levels[1] != -1 && levels[2] != -1)
                {
                    categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3[levels[2]].name = newName;
                    return Json(categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3[levels[2]]);
                }
                else if (levels[1] != -1 && levels[2] == -1 && categoryLevel1.categoriesLevel2 != null)
                {
                    categoryLevel1.categoriesLevel2[levels[1]].name = newName;
                    return Json(categoryLevel1.categoriesLevel2[levels[1]]);
                }
                else
                {
                    categoryLevel1.name = newName;
                    DatabaseMoq.UpdateJson();
                    return Json(categoryLevel1);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] int[] levels, string newName, Guid idLevel1)
        {

            await Task.Delay(10);
            if (levels[0] == -1)
            {
                ProjectCategoryModel projectCategoryModel = new ProjectCategoryModel();
                projectCategoryModel.id = Guid.NewGuid();
                projectCategoryModel.name = newName;
                DatabaseMoq.ProjectsCategories.Add(projectCategoryModel);
                DatabaseMoq.UpdateJson();
                return Json(DatabaseMoq.ProjectsCategories);
            }
            else
            {
                ProjectCategoryModel? categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(idLevel1));
                if (levels[1] == -1)
                {
                    ProjectCategoryModel2 projectCategoryModel2 = new ProjectCategoryModel2();
                    projectCategoryModel2.id = Guid.NewGuid();
                    projectCategoryModel2.name = newName;
                    categoryLevel1.categoriesLevel2.Add(projectCategoryModel2);
                    DatabaseMoq.UpdateJson();
                    return Json(categoryLevel1.categoriesLevel2);
                }
                if (levels[2] == -1)
                {
                    ProjectCategoryModel3 projectCategoryModel3 = new ProjectCategoryModel3();
                    projectCategoryModel3.id = Guid.NewGuid();
                    projectCategoryModel3.name = newName;
                    categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3.Add(projectCategoryModel3);
                    DatabaseMoq.UpdateJson();
                    return Json(categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3);
                }

            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> DeleteCategory([FromBody] string[] ids0)
        {
            await Task.Delay(10);
            Guid[] ids = new Guid[3];
            for (int i = 0; i < ids0.Length; i++)
            {
                if (Guid.TryParse(ids0[i], out Guid result))
                {
                    ids[i] = result;
                }
            }

            ProjectCategoryModel? categoryLevel1 = new ProjectCategoryModel();
            ProjectCategoryModel2? categoryLevel2 = new ProjectCategoryModel2();
            ProjectCategoryModel3? categoryLevel3 = new ProjectCategoryModel3();

            if (ids[0] != Guid.Empty && DatabaseMoq.ProjectsCategories != null)
            {
                categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(ids[0]));

                if (categoryLevel1 != null)
                {
                    if (ids[1] != Guid.Empty && categoryLevel1.categoriesLevel2 != null)
                    {
                        categoryLevel2 = categoryLevel1.categoriesLevel2.FirstOrDefault(c => c.id.Equals(ids[1]));

                        if (categoryLevel2 != null)
                        {
                            if (ids[2] != Guid.Empty && categoryLevel2.categoriesLevel3 != null)
                            {
                                categoryLevel3 = categoryLevel2.categoriesLevel3.FirstOrDefault(c => c.id.Equals(ids[2]));

                                if (categoryLevel3 != null)
                                {
                                    categoryLevel2.categoriesLevel3.Remove(categoryLevel3);
                                }
                            }
                            else
                            {
                                categoryLevel1.categoriesLevel2.Remove(categoryLevel2);
                            }
                        }
                    }
                    else
                    {
                        DatabaseMoq.ProjectsCategories.Remove(categoryLevel1);
                    }
                }
            }
            DatabaseMoq.UpdateJson();

            return NotFound();
        }

    }
}
