using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Configuration;

namespace ReportingSystem.Data.JSON
{
    public class JsonWrite
    {
        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }
       

        public async Task RegistrationCustomer(string[] ar)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new ();
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                Email = ar[0],
                FirstName = ar[1],
                SecondName = ar[2],
                ThirdName = ar[3],
                Phone = ar[4],
                DateRegistration = DateTime.Today,
                //дилема з паролем, ввід при реєстрації чи відправка на пошту
                Password = EncryptionHelper.Encrypt(ar[5]),/*new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray()),*/
                StatusLicence = new CustomerLicenceStatusModel
                {
                    licenceType = LicenceType.Test,
                    licenceName = LicenceType.Test.GetDisplayName()
                },
                Companies = [],
                EndTimeLicense = DateTime.Today.AddDays(30),
                HistoryOperations = []
            };

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = customer.Id,
                oldEndDateLicence = DateTime.Today,
                newEndDateLicence = customer.EndTimeLicense,
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = customer.StatusLicence
            };

            customer.HistoryOperations.Add(history);

            var customers = await new JsonRead().GetCustomers();
            if (customers != null)
            {
                customers.Add(customer);
                UpdateJsonCustomers(customers);
            }
        }

        public async Task EditCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    company.Name = ar[1];
                    company.Address = ar[2];
                    company.Actions = ar[3];
                    company.Phone = ar[4];
                    company.Email = ar[5];
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task EditCompanyJson(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    company.Name = ar[1];
                    company.Address = ar[2];
                    company.Actions = ar[3];
                    company.Phone = ar[4];
                    company.Email = ar[5];
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task ArchiveCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    company.Status = new CompanyStatusModel
                    {
                        companyStatusType = CompanyStatus.Archive,
                        companyStatusName = CompanyStatus.Archive.GetDisplayName()
                    };
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task DeleteCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    customer.Companies.Remove(company);
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task<CompanyModel?> CreateCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return null;
            }

            var company = new CompanyModel
            {
                Name = ar[0],
                Code = ar[1],
                Address = ar[2],
                Actions = ar[3],
                Phone = ar[4],
                Email = ar[5],
                RegistrationDate = DateTime.Today,
                Rolls = DefaultEmployeeRolls.Get(),
                Positions = [],
                Employees = [],
                Status = new CompanyStatusModel
                {
                    companyStatusType = CompanyStatus.Project,
                    companyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (customers != null)
            {
                var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

                if (customer != null && customer.Companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        firstName = customer.FirstName,
                        secondName = customer.SecondName,
                        thirdName = customer.ThirdName,
                        emailWork = customer.Email
                    };

                    company.Chief = chief;
                    customer.Companies.Add(company);
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }


        private void UpdateJsonCustomers(List<CustomerModel> customers)
        {
            var data = new DatabaseData
            {
                Customers = customers,
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        private static void UpdateJsonAdministrators(List<EmployeeModel> administrators)
        {
            var data = new DatabaseData
            {
                Administrators = administrators
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        private void UpdateJsonConfiguration(ConfigurationModel configuration)
        {
            var data = new DatabaseData
            {
                Configuration = configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        private static void UpdateJsonAll(DatabaseData DatabaseData)
        {
            var data = new DatabaseData
            {
                Customers = DatabaseData.Customers,
                Administrators = DatabaseData.Administrators,
                Configuration = DatabaseData.Configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
    }
}
