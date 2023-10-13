using Newtonsoft.Json;
using ReportingSystem.Data.JSON;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Data.SQL
{
    public class SQLWrite
    {
        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }
        public async Task<CompanyModel?> EditCompany(string[] ar)
        {
            await Task.Delay(1000);
            if (DatabaseMoq.Customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

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
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }
        public async Task<CompanyModel?> ArchiveCompany(string[] ar)
        {
            List<CustomerModel>? Customers = await new JsonRead().GetCustomers();

            if (Customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

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
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }
        public async Task<CompanyModel?> DeleteCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    customer.companies.Remove(company);
                    return company;
                }
            }

            return null;
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
    }
}
