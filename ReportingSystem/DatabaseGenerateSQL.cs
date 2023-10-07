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
using ReportingSystem.Data;
using ReportingSystem.Utils;

namespace ReportingSystem
{
    public class DatabaseGenerateSQL
    {

        public async Task GenerateAdministrators()
        {
            if (!new DataIsExist().Administrators()) 
            {
                List<EmployeeModel> devList = new List<EmployeeModel>();

                EmployeeModel dev = new EmployeeModel();
                dev.birthDate = DateTime.Parse("10.04.1991");
                dev.firstName = "Сергій";
                dev.secondName = "Наку";
                dev.thirdName = "Олегович";
                dev.phoneWork = "+380666666666";
                dev.emailWork = "serhii@gmail.ua";
                dev.login = "serhii";
                dev.password = "12345";
                devList.Add(dev);

                dev = new EmployeeModel();
                dev.birthDate = DateTime.Parse("01.01.1990");
                dev.firstName = "Олександр";
                dev.secondName = "Шевельський";
                dev.thirdName = "------------";
                dev.phoneWork = "+380666666666";
                dev.emailWork = "alex@gmail.ua";
                dev.login = "alex";
                dev.password = "12345";
                devList.Add(dev);

                foreach (EmployeeModel e in devList)
                {
                    dev = new EmployeeModel()
                    {
                        birthDate = e.birthDate,
                        customerId = Guid.Empty,
                        companyId = Guid.Empty,
                        firstName = e.firstName,
                        secondName = e.secondName,
                        thirdName = e.thirdName,
                        phoneWork = e.phoneWork,
                        emailWork = e.emailWork,
                        login = e.login,
                        password = e.password,
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
                    };
                    Debug.WriteLine($"Admins added");
                    await new InsertData().Developer(dev);
                }
            }
        }

        public async Task GenerateCustomers()
        {
            if (!new DataIsExist().Customers())
            {
                for (int i = 0; i < 20; i++)
                {
                    var faker = new Faker();
                    CustomerModel customer = new CustomerModel();
                    customer.id = Guid.NewGuid();
                    customer.firstName = faker.Name.FirstName();
                    customer.secondName = faker.Name.LastName();
                    customer.thirdName = faker.Name.Suffix();
                    customer.statusLicence = GenerateCustomer.Status();
                    customer.phone = GenerateInfo.MobilePhoneNumber();
                    customer.email = (customer.secondName.ToLower() + "@gmail.com.ua").Replace(" ", "");
                    customer.password = GenerateInfo.Password();
                    customer.endTimeLicense = GenerateCustomer.LicenceDate(customer.statusLicence);
                    customer.dateRegistration = GenerateDate.BetweenDates(new DateTime(2020, 01, 01), new DateTime(2021, 06, 01));
                    customer.configure = new CustomerConfigModel() { };
                    await new InsertData().Customer(customer);
                    await GenerateRandomCompany(customer);
                    Debug.WriteLine($"Customer added");
                }

                async Task GenerateRandomCompany(CustomerModel customer)
                {
                    Random random = new Random();
                    var countCompany = random.Next(15, 22);
                    for (int i = 0; i < countCompany; i++)
                    {
                        if (customer != null)
                        {
                            var company = await new GenerateCompanySQL().RandomCompany(customer);
                            //if (company != null)
                            //{
                            //    await new InsertData().Company(company, customer.id);
                            Debug.WriteLine($"Company {i} added. All is {countCompany}");
                            //}
                        }
                    }
                }
            }
        }
    }
}
