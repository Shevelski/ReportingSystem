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
            List<EmployeeModel> managersE = teamE.Where(e => e.position != null && e.position.NamePosition == "Проект-менеджер").ToList();

            List<EmployeePositionModel> developersP = teamP.Where(e => e.NamePosition == "Розробник").ToList();
            List<EmployeeModel> developersE = teamE.Where(e => e.position!= null && e.position.NamePosition == "Розробник").ToList();

            List<EmployeePositionModel> designersP = teamP.Where(e => e.NamePosition == "Графічний дизайнер").ToList();
            List<EmployeeModel> designersE = teamE.Where(e => e.position != null && e.position.NamePosition == "Графічний дизайнер").ToList();

            List<EmployeePositionModel> testersP = teamP.Where(e => e.NamePosition == "Тестувальник").ToList();
            List<EmployeeModel> testersE = teamE.Where(e => e.position != null &&  e.position.NamePosition == "Тестувальник").ToList();

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
                    companyId = company.Id,
                    id = Guid.NewGuid(),
                    head = managersE[m],
                    name = faker.Commerce.ProductMaterial(),
                    description = faker.Commerce.ProductDescription(),
                    startDate = DateTime.Now.AddDays(random.Next(-180, -5)),
                    planDate = DateTime.Now.AddDays(random.Next(20, 35)),
                    endDate = DateTime.Now.AddDays(random.Next(30, 35)),
                    positions = []
                };
                project.positions.Add(managersP[m]);
                project.members = [managersE[m]];
                ProjectStatus[] values = [ProjectStatus.Project, ProjectStatus.InProcess, ProjectStatus.Support, ProjectStatus.InImprove, ProjectStatus.Archive];
                project.status = new ProjectStatusModel
                {
                    type = values[random.Next(values.Length)]
                };
                project.status.name = project.status.type.GetDisplayName();

                if (company.Categories != null)
                {
                    project.category = new ProjectChCategoryModel
                    {
                        level0CatId = company.Categories[0].id,
                        level0CatName = company.Categories[0].name,
                        level1CatId = company.Categories[0].categoriesLevel1[0].id,
                        level1CatName = company.Categories[0].categoriesLevel1[0].name
                    };
                }

                for (int d = 0; d < numberPosOfDevelopers; d++)
                {
                    project.positions.Add(developersP[d]);
                    project.members.Add(developersE[d]);
                }
                for (int de = 0; de < numberPosOfDesigners; de++)
                {
                    project.positions.Add(designersP[de]);
                    project.members.Add(designersE[de]);
                }
                for (int t = 0; t < numberPosOfTesters; t++)
                {
                    project.positions.Add(testersP[t]);
                    project.members.Add(testersE[t]);
                }

                project.steps = [];
                ProjectStepModel stepModel = new()
                {
                    name = "Step 1",
                    description = "Description 1",
                    positions = project.positions,
                    members = project.members,
                    status = project.status,
                    startDate = project.startDate,
                    endDate = project.endDate
                };
                project.steps.Add(stepModel);

                stepModel = new ProjectStepModel
                {
                    name = "Step 2",
                    description = "Description 1",
                    positions = project.positions,
                    members = project.members,
                    status = project.status,
                    startDate = project.startDate,
                    endDate = project.endDate
                };
                project.steps.Add(stepModel);

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
