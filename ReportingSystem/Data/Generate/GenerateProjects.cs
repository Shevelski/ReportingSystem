using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.User;

namespace ReportingSystem.Data.Generate
{
    public class GenerateProjects
    {

        public List<ProjectModel> RandomProjects(CompanyModel company)
        {
            List<ProjectModel> projects = [];

            List<EmployeePositionModel> teamP = company.Positions ?? ([]);
            List<EmployeeModel> teamE = company.Employees ?? ([]);

            List<EmployeePositionModel> managersP = teamP.Where(e => e.NamePosition == "Проект-менеджер").ToList();
            List<EmployeeModel> managersE = teamE.Where(e => e.Position != null && e.Position.NamePosition == "Проект-менеджер").ToList();

            List<EmployeePositionModel> developersP = teamP.Where(e => e.NamePosition == "Розробник").ToList();
            List<EmployeeModel> developersE = teamE.Where(e => e.Position!= null && e.Position.NamePosition == "Розробник").ToList();

            List<EmployeePositionModel> designersP = teamP.Where(e => e.NamePosition == "Графічний дизайнер").ToList();
            List<EmployeeModel> designersE = teamE.Where(e => e.Position != null && e.Position.NamePosition == "Графічний дизайнер").ToList();

            List<EmployeePositionModel> testersP = teamP.Where(e => e.NamePosition == "Тестувальник").ToList();
            List<EmployeeModel> testersE = teamE.Where(e => e.Position != null &&  e.Position.NamePosition == "Тестувальник").ToList();

            int numberPosOfManagers = managersP.Count;
            int numberPosOfDevelopers = developersP.Count / numberPosOfManagers;

            int numberEmpOfManagers = managersE.Count;
            int numberEmpOfDevelopers = developersE.Count / numberEmpOfManagers;

            int numberPosOfDesigners = designersP.Count / numberPosOfManagers;
            int numberEmpOfDesigners = designersE.Count / numberEmpOfManagers;

            int numberPosOfTesters = testersP.Count / numberPosOfManagers;
            int numberEmpOfTesters = testersE.Count / numberEmpOfManagers;


            for (int m = 0; m < numberPosOfManagers; m++)
            {
                Random random = new();
                Faker faker = new();
                ProjectModel project = new()
                {
                    CompanyId = company.Id,
                    Id = Guid.NewGuid(),
                    Head = managersE[m],
                    Name = faker.Commerce.ProductMaterial(),
                    Description = faker.Commerce.ProductDescription(),
                    StartDate = DateTime.Now.AddDays(random.Next(-180, -5)),
                    PlanDate = DateTime.Now.AddDays(random.Next(20, 35)),
                    EndDate = DateTime.Now.AddDays(random.Next(30, 35)),
                    Positions = []
                };
                project.Positions.Add(managersP[m]);
                project.Members = [managersE[m]];
                ProjectStatus[] values = [ProjectStatus.Project, ProjectStatus.InProcess, ProjectStatus.Support, ProjectStatus.InImprove, ProjectStatus.Archive];
                project.Status = new ProjectStatusModel
                {
                    Type = values[random.Next(values.Length)]
                };
                project.Status.Name = project.Status.Type.GetDisplayName();

                if (company.Categories != null)
                {
                    project.Category = new ProjectChCategoryModel
                    {
                        Level0CatId = company.Categories[0].Id,
                        Level0CatName = company.Categories[0].Name,
                        Level1CatId = company.Categories[0].CategoriesLevel1[0].Id,
                        Level1CatName = company.Categories[0].CategoriesLevel1[0].Name
                    };
                }

                for (int d = 0; d < numberPosOfDevelopers; d++)
                {
                    project.Positions.Add(developersP[d]);
                    project.Members.Add(developersE[d]);
                }
                for (int de = 0; de < numberPosOfDesigners; de++)
                {
                    project.Positions.Add(designersP[de]);
                    project.Members.Add(designersE[de]);
                }
                for (int t = 0; t < numberPosOfTesters; t++)
                {
                    project.Positions.Add(testersP[t]);
                    project.Members.Add(testersE[t]);
                }

                project.Steps = [];
                ProjectStepModel stepModel = new()
                {
                    Name = "Step 1",
                    Description = "Description 1",
                    Positions = project.Positions,
                    Members = project.Members,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate
                };
                project.Steps.Add(stepModel);

                stepModel = new ProjectStepModel
                {
                    Name = "Step 2",
                    Description = "Description 1",
                    Positions = project.Positions,
                    Members = project.Members,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate
                };
                project.Steps.Add(stepModel);

                projects.Add(project);
            }


            //List<ProjectModel> projects = new List<ProjectModel>();
            //List<List<EmployeePositionModel>> teams = new List<List<EmployeePositionModel>>();
            //List<EmployeePositionModel> team = new List<EmployeePositionModel>();

            //List<EmployeePositionModel> posHeader = new List<EmployeePositionModel>();
            //List<EmployeeModel> empHeader = new List<EmployeeModel>();

            //if (company.positions != null)
            //{
            //    foreach (var pos in company.positions)
            //    {
            //        if (pos.namePosition != null && pos.namePosition.Equals("Проект-менеджер"))
            //        {
            //            posHeader.Add(pos);
            //        }
                    
            //    }
            //}
            //if (company.employees != null)
            //{
            //    foreach (var emp in company.employees)
            //    {
            //        if (emp.position != null && emp.position.namePosition != null && emp.position.namePosition.Equals("Проект-менеджер"))
            //        {
            //            empHeader.Add(emp);
            //        }
                    
            //    }
            //}

            //for (int i = 0; i < posHeader.Count; i++)
            //{
            //    ProjectModel project = RandomProject(company, posHeader[i], empHeader[i]);
            //}

            return projects;
        }

        //public ProjectModel RandomProject(CompanyModel company, EmployeePositionModel posHead, EmployeeModel headEm)
        //{
        //    Random random = new Random();
        //    Faker faker = new Faker();
        //    ProjectModel project = new ProjectModel();
        //    project.id = Guid.NewGuid();
        //    project.companyId = company.id;
        //    project.name = faker.Commerce.ProductMaterial();
        //    project.description = faker.Commerce.ProductDescription();
        //    project.startDate = DateTime.Now.AddDays(random.Next(-5, -180));
        //    project.planDate = DateTime.Now.AddDays(random.Next(20, 35));
        //    project.endDate = DateTime.Now.AddDays(random.Next(30, 35));
        //    if (company.categories != null)
        //    {
        //        project.categoryModel = company.categories[random.Next(0, company.categories.Count)];
        //        if (project.categoryModel.categoriesLevel1 != null)
        //        {
        //            var categoryModel1 = project.categoryModel.categoriesLevel1[random.Next(0, project.categoryModel.categoriesLevel1.Count)];
        //            project.categoryModel1 = categoryModel1;
        //            if (categoryModel1.categoriesLevel2 != null)
        //            {
        //                var categoryModel2 = categoryModel1.categoriesLevel2[random.Next(0, categoryModel1.categoriesLevel2.Count)];
        //                project.categoryModel2 = categoryModel2;
        //                if (categoryModel2.categoriesLevel3 != null)
        //                {
        //                    var categoryModel3 = categoryModel2.categoriesLevel3[random.Next(0, categoryModel2.categoriesLevel3.Count)];
        //                    project.categoryModel3 = categoryModel3;

        //                }
        //            }
        //        }
        //    }
            
        //    ProjectStatus[] values = { ProjectStatus.Project, ProjectStatus.InProcess, ProjectStatus.Support, ProjectStatus.InImprove, ProjectStatus.Archive };
        //    ProjectStatus status = values[random.Next(values.Length)];

        //    project.status = new ProjectStatusModel()
        //    {
        //        type = status,
        //        name = status.GetDisplayName(),
        //    };

        //    return project;
        //}

        private List<EmployeePositionModel>? GeneratePositionForProject(CompanyModel company)
        {
            throw new NotImplementedException();
        }
    }
}
