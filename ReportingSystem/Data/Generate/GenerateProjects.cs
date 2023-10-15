using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;

namespace ReportingSystem.Data.Generate
{
    public class GenerateProjects
    {
        public List<ProjectModel> RandomProjects(CompanyModel company)
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            List<List<EmployeePositionModel>> teams = new List<List<EmployeePositionModel>>();
            List<EmployeePositionModel> team = new List<EmployeePositionModel>();

            List<EmployeePositionModel> posHeader = new List<EmployeePositionModel>();
            List<EmployeeModel> empHeader = new List<EmployeeModel>();
                //Тестувальник
                //Розробник
                //Графічний дизайнер
            if (company.positions != null)
            {
                foreach (var pos in company.positions)
                {
                    if (pos.namePosition != null && pos.namePosition.Equals("Проект-менеджер"))
                    {
                        posHeader.Add(pos);
                    }
                    
                }
            }




            if (company.employees != null)
            {
                foreach (var emp in company.employees)
                {
                    if (emp.position != null && emp.position.namePosition != null && emp.position.namePosition.Equals("Проект-менеджер"))
                    {
                        empHeader.Add(emp);
                    }
                    
                }
            }

            for (int i = 0; i < posHeader.Count; i++)
            {
                ProjectModel project = RandomProject(company, posHeader[i], empHeader[i]);
            }

            return new List<ProjectModel>();
        }

        public ProjectModel RandomProject(CompanyModel company, EmployeePositionModel posHead, EmployeeModel headEm)
        {
            Random random = new Random();
            Faker faker = new Faker();
            ProjectModel project = new ProjectModel();
            project.id = Guid.NewGuid();
            project.companyId = company.id;
            project.name = faker.Commerce.ProductMaterial();
            project.description = faker.Commerce.ProductDescription();
            project.startDate = DateTime.Now.AddDays(random.Next(-5, -180));
            project.planDate = DateTime.Now.AddDays(random.Next(20, 35));
            project.endDate = DateTime.Now.AddDays(random.Next(30, 35));
            if (company.categories != null)
            {
                project.categoryModel = company.categories[random.Next(0, company.categories.Count)];
                if (project.categoryModel.categoriesLevel1 != null)
                {
                    var categoryModel1 = project.categoryModel.categoriesLevel1[random.Next(0, project.categoryModel.categoriesLevel1.Count)];
                    project.categoryModel1 = categoryModel1;
                    if (categoryModel1.categoriesLevel2 != null)
                    {
                        var categoryModel2 = categoryModel1.categoriesLevel2[random.Next(0, categoryModel1.categoriesLevel2.Count)];
                        project.categoryModel2 = categoryModel2;
                        if (categoryModel2.categoriesLevel3 != null)
                        {
                            var categoryModel3 = categoryModel2.categoriesLevel3[random.Next(0, categoryModel2.categoriesLevel3.Count)];
                            project.categoryModel3 = categoryModel3;

                        }
                    }
                }
            }
            
            //project.positions = GeneratePositionForProject(company);
            //project.members = GenerateMembersForProject(company);
            //project.head = GenerateHeaderForProject(company);


            //project
            //project.steps
            ProjectStatus[] values = { ProjectStatus.Project, ProjectStatus.InProcess, ProjectStatus.Support, ProjectStatus.InImprove, ProjectStatus.Archive };
            ProjectStatus status = values[random.Next(values.Length)];

            project.status = new ProjectStatusModel()
            {
                type = status,
                name = status.GetDisplayName(),
            };

            return project;
        }

        private List<EmployeePositionModel>? GeneratePositionForProject(CompanyModel company)
        {
            throw new NotImplementedException();
        }
    }
}
