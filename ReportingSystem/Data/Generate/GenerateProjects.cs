using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Employee;
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
                    CustomerId = company.IdCustomer,
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

                project.EmpPositions = new List<EmployeesPositionsModel>();
                for (int d = 0; d < numberPosOfDevelopers; d++)
                {
                    project.Positions.Add(developersP[d]);
                    project.Members.Add(developersE[d]);
                    EmployeesPositionsModel empo = new();
                    empo.NamePosition = developersP[d].NamePosition;
                    empo.Employees = new List<EmployeeModel>();
                    empo.Employees.Add(developersE[d]);
                    project.EmpPositions.Add(empo);
                }
                for (int de = 0; de < numberPosOfDesigners; de++)
                {
                    project.Positions.Add(designersP[de]);
                    project.Members.Add(designersE[de]);
                    EmployeesPositionsModel empo = new();
                    empo.NamePosition = designersP[de].NamePosition;
                    empo.Employees = new List<EmployeeModel>();
                    empo.Employees.Add(designersE[de]);
                    project.EmpPositions.Add(empo);
                }
                for (int t = 0; t < numberPosOfTesters; t++)
                {
                    project.Positions.Add(testersP[t]);
                    project.Members.Add(testersE[t]);
                    EmployeesPositionsModel empo = new();
                    empo.NamePosition = testersP[t].NamePosition;
                    empo.Employees = new List<EmployeeModel>();
                    empo.Employees.Add(testersE[t]);
                    project.EmpPositions.Add(empo);
                }

                project.Steps = [];
                ProjectStepModel stepModel = new()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = project.CustomerId,
                    CompanyId = project.CompanyId,
                    ProjectId = project.Id,
                    Name = "Step 1",
                    Description = "Description 1",
                    Positions = project.Positions,
                    Members = project.Members,
                    EmpPositions = project.EmpPositions,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    PlanDate = project.EndDate,
                    EndDate = project.EndDate
                };
                project.Steps.Add(stepModel);

                stepModel = new ProjectStepModel
                {
                    Id = Guid.NewGuid(),
                    CustomerId = project.CustomerId,
                    CompanyId = project.CompanyId,
                    ProjectId = project.Id,
                    Name = "Step 2",
                    Description = "Description 1",
                    Positions = project.Positions,
                    Members = project.Members,
                    EmpPositions = project.EmpPositions,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    PlanDate = project.EndDate,
                    EndDate = project.EndDate
                };
                project.Steps.Add(stepModel);

                projects.Add(project);
            }
            return projects;
        }
    }
}
