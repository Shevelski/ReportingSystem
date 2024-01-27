using Bogus;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GenerateCustomers
    {
        public List<CustomerModel> Customers()
        {
            List<CustomerModel> Customers = [];
            Random random = new();

            try
            {
                int countCustomer = random.Next(5, 10);
                for (int i = 0; i < countCustomer; i++)
                {
                    CustomerModel Customer = RandomCustomer();
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

        private static CustomerModel RandomCustomer()
        {
            var faker = new Faker();
            CustomerModel customer = new()
            {
                Id = Guid.NewGuid(),
                FirstName = faker.Name.FirstName(),
                SecondName = faker.Name.LastName(),
                ThirdName = faker.Name.FirstName(),
                StatusLicence = CustomerLicenceStatus(),
                Phone = GenerateInfo.MobilePhoneNumber()
            };
            customer.Email = (customer.SecondName.ToLower() + "@gmail.com.ua").Replace(" ", "");
            customer.Password = EncryptionHelper.Encrypt(GenerateInfo.Password());
            customer.EndTimeLicense = LicenseCustomer.LicenceDate(customer.StatusLicence);
            customer.DateRegistration = GenerateDate.BetweenDates(new DateTime(2020, 01, 01), new DateTime(2021, 06, 01));
            customer.Companies = new GenerateCompanies().GenerateRandomCompanies(customer);
            customer.Configure = new CustomerConfigModel();
            return customer;
        }

        public static CustomerLicenceStatusModel CustomerLicenceStatus()
        {
            Random random = new();
            CustomerLicenceStatusModel result = new();
            LicenceType[] values = [LicenceType.Archive, LicenceType.Test, LicenceType.Main, LicenceType.Expired, LicenceType.Nulled];
            LicenceType status = values[random.Next(values.Length)];

            result.LicenceType = status;
            result.LicenceName = status.GetDisplayName();

            return result;
        }

        private class LicenseCustomer
        {
            static readonly Random random = new();
            public static DateTime LicenceDate(CustomerLicenceStatusModel status)
            {
                DateTime generateDate = DateTime.Now;

                if (status.LicenceType == LicenceType.Archive)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(-200, -100));
                };
                if (status.LicenceType == LicenceType.Main)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(100, 1000));
                };
                if (status.LicenceType == LicenceType.Nulled)
                {
                    generateDate = DateTime.Now.AddDays(-1000);
                };
                if (status.LicenceType == LicenceType.Test)
                {
                    generateDate = DateTime.Now.AddDays(30);
                };
                if (status.LicenceType == LicenceType.Expired)
                {
                    generateDate = DateTime.Now.AddDays(random.Next(-30, -5));
                };

                return generateDate;
            }

            private CustomerLicenceStatusModel Status()
            {
                CustomerLicenceStatusModel result = new();
                LicenceType[] values = [LicenceType.Archive, LicenceType.Test, LicenceType.Main, LicenceType.Expired, LicenceType.Nulled];
                LicenceType status = values[random.Next(values.Length)];

                result.LicenceType = status;
                result.LicenceName = status.GetDisplayName();

                return result;
            }
        }
    }
}
