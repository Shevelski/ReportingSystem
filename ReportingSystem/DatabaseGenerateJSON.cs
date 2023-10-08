using Bogus;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models;
using ReportingSystem.Test.GenerateData;
using ReportingSystem.Test.Generate;
using System.Diagnostics;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;

namespace ReportingSystem
{
    public static class DatabaseMoqGenerate
    {
        public static List<CustomerModel>? Customers { get; set; }
        public static CompanyModel? Configuration { get; set; }
        public static List<EmployeeModel>? Administrators { get; set; }
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
        
        public static Random random = new Random();

        static DatabaseMoqGenerate()
        {
            Administrators = new List<EmployeeModel>()
            {
                new EmployeeModel()
                {
                    id = Guid.NewGuid(),
                    firstName = "Сергій",
                    secondName = "Наку",
                    thirdName = "------",
                    phoneWork = "+380666666666",
                    emailWork = "serhii@gmail.ua",
                    photo = "",
                    login = "serhii",
                    password = "12345",//EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = Enums.EmployeeRolStatus.Developer,
                        rolName = Enums.EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = Enums.EmployeeStatus.Actual,
                        employeeStatusName = Enums.EmployeeStatus.Actual.GetDisplayName()
                    }
                },
                new EmployeeModel()
                {
                    id = Guid.NewGuid(),
                    firstName = "Олександр",
                    secondName = "Шевельський",
                    thirdName = "------------",
                    phoneWork = "+380666666666",
                    emailWork = "alex@gmail.ua",
                    photo = "",
                    login = "alex",
                    password = "12345",//EncryptionHelper.Encrypt("12345"),
                    rol = new EmployeeRolModel()
                    {
                        rolType = Enums.EmployeeRolStatus.Developer,
                        rolName = Enums.EmployeeRolStatus.Developer.GetDisplayName()
                    },
                    status = new EmployeeStatusModel()
                    {
                        employeeStatusType = Enums.EmployeeStatus.Actual,
                        employeeStatusName = Enums.EmployeeStatus.Actual.GetDisplayName()
                    }
                }
            };
            Debug.WriteLine($"Admins added. All is 2");

            CompanyModel configuration = new CompanyModel();

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
                customer.email = (customer.secondName.ToLower() + "@gmail.com.ua").Replace(" ", "");
                customer.password = GenerateInfo.Password();//EncryptionHelper.Encrypt(GenerateInfo.Password());
                customer.endTimeLicense = GenerateCustomer.LicenceDate(customer.statusLicence);
                customer.dateRegistration = GenerateDate.BetweenDates(new DateTime(2020, 01, 01), new DateTime(2021, 06, 01));
                customer.companies = GenerateRandomCompanies(customer);
                customer.configure = new CustomerConfigModel();
                return customer;
            }

            static List<CompanyModel> GenerateRandomCompanies(CustomerModel customer)
            {
                List<CompanyModel> companies = new List<CompanyModel>();
                var countCompany = random.Next(15, 22);
                for (int i = 0; i < countCompany; i++)
                {
                    
                    if (customer != null)
                    {
                        var company = GenerateCompany.RandomCompany(customer);
                        if (company != null)
                        {
                            companies.Add(company);
                            Debug.WriteLine($"Company {i} added. All is {countCompany}");
                        }  
                    }
                };
                return companies;
            }

            int countCustomer = random.Next(2, 3);
            for (int i = 0; i < countCustomer; i++)
            {
                var customer = GenerateRandomCustomer();
                Customers.Add(customer);
                Debug.WriteLine($"Customer {i} added. All is 10");
            }
        }
    }
}
