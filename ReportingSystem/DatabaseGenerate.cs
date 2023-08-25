using Bogus;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models;
using ReportingSystem.Test.GenerateData;
using ReportingSystem.Test.Generate;
using System.Diagnostics;

namespace ReportingSystem
{
    public static class DatabaseMoqGenerate
    {
        public static List<CustomerModel>? Customers { get; set; }
        public static CustomerModel? customer { get; set; }
        public static List<List<EmployeeModel>>? AllUsers { get; set; }
        public static List<EmployeeModel>? Users { get; set; }
        public static List<ProjectCategoryModel>? AllProjectsCategories { get; set; }
        public static List<ProjectCategoryModel>? ProjectsCategories { get; set; }
        public static List<List<ProjectModel>>? AllProjects { get; set; }
        public static List<ProjectModel>? Projects{ get; set; }
        public static List<List<CompanyModel>>? AllCompanies { get; set; }
        public static List<CompanyModel>? Companies { get; set; }
        public static List<ProjectStatusModel>? ProjectStatus { get; set; }
        public static List<EmployeePositionModel>? UserPositions { get; set; }
        public static List<EmployeeRolModel>? UserRolls { get; set; }
        public static List<CompanyStatusModel>? CompanyStatus { get; set; }
        //public static CustomerConfigModel? CustomerConfig { get; set; }
        public static Random random = new Random();

        static DatabaseMoqGenerate()
        {

            Customers = new List<CustomerModel>();
            Companies = new List<CompanyModel>();
            Users = new List<EmployeeModel>();
            var faker = new Faker();

            static CustomerModel GenerateRandomCustomer()
            {
                var faker = new Faker();
                customer = new CustomerModel();
                customer.id = Guid.NewGuid();
                customer.firstName = faker.Name.FirstName();
                customer.secondName = faker.Name.LastName();
                customer.thirdName = faker.Name.FirstName();
                customer.statusLicence = GenerateCustomer.Status();
                customer.phone = GenerateInfo.MobilePhoneNumber();
                customer.email = (customer.secondName + "@gmail.com.ua").Replace(" ", "").ToLower();
                customer.password = GenerateInfo.Password();
                customer.endTimeLicense = GenerateCustomer.LicenceDate(customer.statusLicence);
                customer.dateRegistration = GenerateDate.BetweenDates(new DateTime(2020, 01, 01), new DateTime(2021, 06, 01));
                customer.companies = GenerateRandomCompanies(customer);
                customer.configure = new CustomerConfigModel()
                {
                    IsSaveCompany = false,
                    IdSavedCompany = Guid.Empty,
                };
                return customer;
            }

            static List<CompanyModel> GenerateRandomCompanies(CustomerModel customer)
            {
                List<CompanyModel> companies = new List<CompanyModel>();
                var countCompany = random.Next(15, 22);
                for (int i = 0; i < countCompany; i++)
                {
                    companies.Add(GenerateCompany.RandomCompany(customer));
                    Debug.WriteLine($"Company {i} added. All is {countCompany}");
                };
                return companies;
            }

            int countCustomer = random.Next(2, 15);
            for (int i = 0; i < countCustomer; i++)
            {
                var customer = GenerateRandomCustomer();
                Customers.Add(customer);
                Debug.WriteLine($"Customer {i} added. All is 10");
            }
        }
    }
}
