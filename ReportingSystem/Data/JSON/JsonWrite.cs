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
       
        public async Task EditCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    company.name = ar[1];
                    company.address = ar[2];
                    company.actions = ar[3];
                    company.phone = ar[4];
                    company.email = ar[5];
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

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    company.name = ar[1];
                    company.address = ar[2];
                    company.actions = ar[3];
                    company.phone = ar[4];
                    company.email = ar[5];
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

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    company.status = new CompanyStatusModel
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

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    customer.companies.Remove(company);
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
                name = ar[0],
                code = ar[1],
                address = ar[2],
                actions = ar[3],
                phone = ar[4],
                email = ar[5],
                registrationDate = DateTime.Today,
                rolls = DefaultEmployeeRolls.Get(),
                positions = new List<EmployeePositionModel>(),
                employees = new List<EmployeeModel>(),
                status = new CompanyStatusModel
                {
                    companyStatusType = CompanyStatus.Project,
                    companyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (customers != null)
            {
                var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

                if (customer != null && customer.companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        firstName = customer.firstName,
                        secondName = customer.secondName,
                        thirdName = customer.thirdName,
                        emailWork = customer.email
                    };

                    company.chief = chief;
                    customer.companies.Add(company);
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
        private void UpdateJsonAdministrators(List<EmployeeModel> administrators)
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
        private void UpdateJsonAll(DatabaseData DatabaseData)
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
