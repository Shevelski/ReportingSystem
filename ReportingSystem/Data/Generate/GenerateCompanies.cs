using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ReportingSystem.Data.Generate
{
    public class GenerateCompanies
    {
        public List<CompanyModel> GenerateRandomCompanies(CustomerModel customer)
        {
            Random random = new Random();
            List<CompanyModel> companies = new List<CompanyModel>();
            var countCompany = random.Next(15, 22);
            for (int i = 0; i < countCompany; i++)
            {

                if (customer != null)
                {
                    var company = RandomCompany(customer);
                    if (company != null)
                    {
                        companies.Add(company);
                        Debug.WriteLine($"Company {i} added. All is {countCompany}");
                    }
                }
            };
            return companies;
        }

        private CompanyModel? RandomCompany(CustomerModel customer)
        {
            var faker = new Faker();
            CompanyModel company = new CompanyModel();
            Random random = new Random();
            if (company != null)
            {
                company.id = Guid.NewGuid();
                company.idCustomer = customer.id;
                company.name = faker.Company.CompanyName();
                company.address = faker.Address.FullAddress();
                company.code = GenerateInfo.Code();
                company.actions = faker.Commerce.ProductAdjective();

                List<string> statusCompanyFromWeb = new List<string> { "Зареєстровано", "На реєстрації", "Актуальна", "На перегляді" };
                company.statusWeb = statusCompanyFromWeb[random.Next(statusCompanyFromWeb.Count)];

                CompanyStatusModel resultStatus = new CompanyStatusModel();
                CompanyStatus[] values = { CompanyStatus.Project, CompanyStatus.Actual, CompanyStatus.Archive };
                CompanyStatus status = values[random.Next(values.Length)];
                resultStatus.companyStatusType = status;
                resultStatus.companyStatusName = status.GetDisplayName();
                company.status = resultStatus;

                company.phone = GenerateInfo.PhoneNumber();
                company.email = (Regex.Replace(company.name, "[^0-9a-zA-Z]+", "") + ".com.ua").Replace(" ", "").ToLower();
                company.statutCapital = random.Next(1000, 300000).ToString() + " UAH";
                company.registrationDate = GenerateDate.BetweenDates(new DateTime(2000, 01, 01), new DateTime(2010, 01, 01));
                company.positions = new GeneratePositions().Positions();
                company.rolls = DefaultEmployeeRolls.GetForEmployee();

                company.employees = new GenerateEmployees().Employees(company, customer.id);
                company.chief = company.employees.First(u => u.position != null && u.position.namePosition != null && u.position.namePosition.Equals("Директор"));

                company.categories = new GenerateCategories().Categories();
                //company.projects = GenerateProjects();
                return company;
            }
            return null;
        }
    }
}
