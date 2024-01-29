using Bogus;
using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Hubs;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ReportingSystem.Data.Generate
{
    public partial class GenerateCompanies(IHubContext<StatusHub> hubContext)
    {

        public async Task<List<CompanyModel>> GenerateRandomCompanies(CustomerModel customer)
        {
            try
            {
                Random random = new();
                List<CompanyModel> companies = [];
                var countCompany = random.Next(15, 22);
                for (int i = 0; i < countCompany; i++)
                {

                    if (customer != null)
                    {
                        var company = await RandomCompany(customer);
                        if (company != null)
                        {
                            companies.Add(company);
                            Debug.WriteLine($"Company {i} added. All is {countCompany}");
                            await new StatusSignalR(hubContext).UpdateStatusGenerateData($"Company {i} added. All is {countCompany}");
                        }
                    }
                };
                return companies;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task<CompanyModel?> RandomCompany(CustomerModel customer)
        {
            try
            {
                var faker = new Faker();
                CompanyModel company = new();
                Random random = new();
                if (company != null)
                {
                    company.Id = Guid.NewGuid();
                    company.IdCustomer = customer.Id;
                    company.Name = faker.Company.CompanyName();
                    company.Address = faker.Address.FullAddress();
                    company.Code = GenerateInfo.Code();
                    company.Actions = faker.Commerce.ProductAdjective();

                    List<string> statusCompanyFromWeb = ["Зареєстровано", "На реєстрації", "Актуальна", "На перегляді"];
                    company.StatusWeb = statusCompanyFromWeb[random.Next(statusCompanyFromWeb.Count)];

                    CompanyStatusModel resultStatus = new();
                    CompanyStatus[] values = [CompanyStatus.Project, CompanyStatus.Actual, CompanyStatus.Archive];
                    CompanyStatus status = values[random.Next(values.Length)];
                    resultStatus.CompanyStatusType = status;
                    resultStatus.CompanyStatusName = status.GetDisplayName();
                    company.Status = resultStatus;

                    company.Phone = GenerateInfo.PhoneNumber();
                    company.Email = (MyRegex().Replace(company.Name, "") + ".com.ua").Replace(" ", "").ToLower();
                    company.StatutCapital = random.Next(1000, 300000).ToString() + " UAH";
                    company.RegistrationDate = GenerateDate.BetweenDates(new DateTime(2000, 01, 01), new DateTime(2010, 01, 01));

                    company.Positions = new GeneratePositions().Positions();
                    company.Rolls = DefaultEmployeeRolls.GetForEmployee();

                    company.Employees = await new GenerateEmployees(hubContext).Employees(company, customer.Id);
                    company.Chief = company.Employees.First(u => u.Position != null && u.Position.NamePosition != null && u.Position.NamePosition.Equals("Директор"));

                    company.Categories = new GenerateCategories().Categories();
                    company.Projects = new GenerateProjects().RandomProjects(company);
                    return company;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [GeneratedRegex("[^0-9a-zA-Z]+")]
        private static partial Regex MyRegex();
    }
}
