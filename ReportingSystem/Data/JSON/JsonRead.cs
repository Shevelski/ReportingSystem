using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;

namespace ReportingSystem.Data.JSON
{
    public class JsonRead
    {
        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }
        private async Task<List<CustomerModel>> LoadCustomersFromJson()
        {
            await Task.Delay(10); // Simulate async delay

            if (File.Exists(Context.Json) && new FileInfo(Context.Json).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(Context.Json))
                {
                    jsonData = await reader.ReadToEndAsync();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null && data.Customers != null)
                {
                    return data.Customers;
                }
            }

            return new List<CustomerModel>();
        }
        public async Task<List<CustomerModel>?> GetCustomers()
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();
            return Customers;
        }
        public async Task<CustomerModel?> GetCustomer(string id)
        {
            List<CustomerModel>? customers = await LoadCustomersFromJson();
            CustomerModel? customer = customers.FirstOrDefault(id=>id.Equals(id));
            return customer;
        }
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();

            if (Customers == null || string.IsNullOrEmpty(idCu))
            {
                return null;
            }

            Guid id;
            if (!Guid.TryParse(idCu, out id) || id.Equals(Guid.Empty))
            {
                id = Customers[0].id;
            }

            var customer = Customers.FirstOrDefault(co => co.id.Equals(id));

            if (customer != null && customer.companies != null)
            {
                return customer.companies;
            }

            return null;
        }
        public async Task<CompanyModel?> GetCompany(string idCu, string idCo)
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();

            if (Customers == null || string.IsNullOrEmpty(idCu))
            {
                return null;
            }

            Guid idCustomer;
            if (!Guid.TryParse(idCu, out idCustomer) || idCustomer.Equals(Guid.Empty))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(cu => cu.id.Equals(idCustomer));



            if (customer != null && customer.companies != null)
            {
                Guid idCompany;
                if (!Guid.TryParse(idCo, out idCompany) || idCompany.Equals(Guid.Empty))
                {
                    return null;
                }
                var company = customer.companies.First(co => co.id.Equals(idCompany));
                return company;
            }

            return null;
        }
        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();

            if (Customers == null || !Guid.TryParse(idCu, out Guid id))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(co => co.id.Equals(id));

            if (customer != null && customer.companies != null)
            {
                return customer.companies
                    .Where(item => item.status != null && item.status.companyStatusType.Equals(CompanyStatus.Actual))
                    .ToList();
            }

            return null;
        }
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();

            if (Customers == null || string.IsNullOrEmpty(idCu) || string.IsNullOrEmpty(idCo) || !Guid.TryParse(idCu, out Guid idCustomer) || !Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

            if (company != null && company.rolls != null)
            {
                return company.rolls;
            }

            return null;
        }
        public async Task<List<EmployeeRolModel>?> GetDevRolls()
        {
            await Task.Delay(10);
            List<EmployeeRolModel> devRols = new List<EmployeeRolModel>();
            EmployeeRolModel devRol = new EmployeeRolModel()
            {
                rolType = EmployeeRolStatus.Developer,
                rolName = EmployeeRolStatus.Developer.GetDisplayName()
            };
            devRols.Add(devRol);
            devRol = new EmployeeRolModel()
            {
                rolType = EmployeeRolStatus.DevAdministrator,
                rolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
            };
            devRols.Add(devRol);
            return devRols;
        }
    }
}
