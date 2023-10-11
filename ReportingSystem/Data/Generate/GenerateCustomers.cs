using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Test.Generate;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GenerateCustomers
    {
        public List<CustomerModel> Customers()
        {
            List<CustomerModel> Customers = new List<CustomerModel>();
            CustomerModel Customer = new CustomerModel();
            List<CompanyModel> Companies = new List<CompanyModel>();
            List<EmployeeModel> Users = new List<EmployeeModel>();
            Random random = new Random();

            try
            {
                int countCustomer = random.Next(2, 3);
                for (int i = 0; i < countCustomer; i++)
                {
                    Customer = RandomCustomer();
                    Customers.Add(Customer);
                    Debug.WriteLine($"Customer {i} added. All is " + countCustomer);
                }
                return Customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
            return Customers;
        }

        private CustomerModel RandomCustomer()
        {
            var faker = new Faker();
            CustomerModel customer = new CustomerModel();
            customer.id = Guid.NewGuid();
            customer.firstName = faker.Name.FirstName();
            customer.secondName = faker.Name.LastName();
            customer.thirdName = faker.Name.FirstName();
            customer.statusLicence = GenerateCustomer.Status();
            customer.phone = GenerateInfo.MobilePhoneNumber();
            customer.email = (customer.secondName.ToLower() + "@gmail.com.ua").Replace(" ", "");
            customer.password = GenerateInfo.Password();//EncryptionHelper.Encrypt(GenerateInfo.Password());
            customer.endTimeLicense = LicenseCustomer.LicenceDate(customer.statusLicence);
            customer.dateRegistration = GenerateDate.BetweenDates(new DateTime(2020, 01, 01), new DateTime(2021, 06, 01));
            customer.companies = new GenerateCompanies().GenerateRandomCompanies(customer);
            customer.configure = new CustomerConfigModel();
            return customer;
        }

        private class LicenseCustomer
        {
            static Random random = new Random();
            public static DateTime LicenceDate(CustomerLicenceStatusModel status)
            {
                DateTime generateDate = DateTime.Now;

                if (status.licenceType == LicenceType.Archive)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(-200, -100));
                };
                if (status.licenceType == LicenceType.Main)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(100, 1000));
                };
                if (status.licenceType == LicenceType.Nulled)
                {
                    generateDate = DateTime.Now.AddDays(-1000);
                };
                if (status.licenceType == LicenceType.Test)
                {
                    generateDate = DateTime.Now.AddDays(30);
                };
                if (status.licenceType == LicenceType.Expired)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(-30, -5));
                };

                return generateDate;
            }

            private CustomerLicenceStatusModel Status()
            {
                CustomerLicenceStatusModel result = new CustomerLicenceStatusModel();
                LicenceType[] values = { LicenceType.Archive, LicenceType.Test, LicenceType.Main, LicenceType.Expired, LicenceType.Nulled };
                LicenceType status = values[random.Next(values.Length)];

                result.licenceType = status;
                result.licenceName = status.GetDisplayName();

                return result;
            }
        }
    }
}
