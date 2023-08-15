using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Controllers
{

    public class ProjectController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> GetProject()
        {
            await Task.Delay(10);
            return Json(DatabaseMoq.Projects);
        }

        [HttpPost]
        public async Task<IActionResult> EditNameCategory([FromBody] int[] levels, string newName, Guid idLevel1)
        {
            await Task.Delay(10);
            ProjectCategoryModel? categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(idLevel1));

            if (levels[1] != -1 && levels[2] != -1)
            {
                categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3[levels[2]].name = newName;
                return Json(categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3[levels[2]]);
            } 
            else if (levels[1] != -1 && levels[2] == -1)
            {
                categoryLevel1.categoriesLevel2[levels[1]].name = newName;
                return Json(categoryLevel1.categoriesLevel2[levels[1]]);
            }
            else
            {
                categoryLevel1.name = newName;
                return Json(categoryLevel1);
            }
            
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
                    return Json(categoryLevel1.categoriesLevel2);
                }
                if (levels[2] == -1)
                {
                    ProjectCategoryModel3 projectCategoryModel3 = new ProjectCategoryModel3();
                    projectCategoryModel3.id = Guid.NewGuid();
                    projectCategoryModel3.name = newName;
                    categoryLevel1.categoriesLevel2[levels[1]].categoriesLevel3.Add(projectCategoryModel3);
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

            ProjectCategoryModel? categoryLevel1 = null;
            ProjectCategoryModel2? categoryLevel2 = null;
            ProjectCategoryModel3? categoryLevel3 = null;
            
            if (ids[0] != Guid.Empty && DatabaseMoq.ProjectsCategories != null)
            {
                categoryLevel1 = DatabaseMoq.ProjectsCategories.FirstOrDefault(c => c.id.Equals(ids[0]));
                if (ids[1] != Guid.Empty && categoryLevel1!=null && categoryLevel1.categoriesLevel2 != null)
                {
                    categoryLevel2 = categoryLevel1.categoriesLevel2.FirstOrDefault(c => c.id.Equals(ids[1]));
                    if (ids[2] != Guid.Empty && categoryLevel2 != null && categoryLevel2.categoriesLevel3 != null)
                    {
                        categoryLevel3 = categoryLevel2.categoriesLevel3.FirstOrDefault(c => c.id.Equals(ids[2]));
                    }
                }
            }

            if (ids[0] != Guid.Empty && ids[1] == Guid.Empty && ids[2] == Guid.Empty && categoryLevel1 != null && DatabaseMoq.ProjectsCategories != null)
            {
                DatabaseMoq.ProjectsCategories.Remove(categoryLevel1);
            }
            if (ids[0] != Guid.Empty && ids[1] != Guid.Empty && ids[2] == Guid.Empty && categoryLevel2 != null && categoryLevel1.categoriesLevel2 != null)
            {
                categoryLevel1.categoriesLevel2.Remove(categoryLevel2);
            }
            if (ids[0] != Guid.Empty && ids[1] != Guid.Empty && ids[2] != Guid.Empty && categoryLevel3 != null && categoryLevel2.categoriesLevel3 != null)
            {
                categoryLevel2.categoriesLevel3.Remove(categoryLevel3);
            }

            return NotFound();
        }

    }
}
