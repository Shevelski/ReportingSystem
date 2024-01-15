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
                using (StreamReader reader = new (Context.Json))
                {
                    jsonData = await reader.ReadToEndAsync();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null && data.Customers != null)
                {
                    return data.Customers;
                }
            }

            return [];
        }
        public async Task<List<CustomerModel>?> GetCustomers()
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();
            return Customers;
        }
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            List<CustomerModel>? customers = await LoadCustomersFromJson();
            CustomerModel? customer = customers.FirstOrDefault(id=>id.Equals(idCu));
            return customer;
        }
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();

            if (Customers == null || string.IsNullOrEmpty(idCu))
            {
                return null;
            }

            if (!Guid.TryParse(idCu, out Guid id) || id.Equals(Guid.Empty))
            {
                id = Customers[0].Id;
            }

            var customer = Customers.FirstOrDefault(co => co.Id.Equals(id));

            if (customer != null && customer.Companies != null)
            {
                return customer.Companies;
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

            if (!Guid.TryParse(idCu, out Guid idCustomer) || idCustomer.Equals(Guid.Empty))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(cu => cu.Id.Equals(idCustomer));



            if (customer != null && customer.Companies != null)
            {
                if (!Guid.TryParse(idCo, out Guid idCompany) || idCompany.Equals(Guid.Empty))
                {
                    return null;
                }
                var company = customer.Companies.First(co => co.Id.Equals(idCompany));
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

            var customer = Customers.FirstOrDefault(co => co.Id.Equals(id));

            if (customer != null && customer.Companies != null)
            {
                return customer.Companies
                    .Where(item => item.Status != null && item.Status.companyStatusType.Equals(CompanyStatus.Actual))
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

            var customer = Customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

            if (company != null && company.Rolls != null)
            {
                return company.Rolls;
            }

            return null;
        }
        public async Task<List<EmployeeRolModel>?> GetDevRolls()
        {
            await Task.Delay(10);
            List<EmployeeRolModel> devRols = [];
            EmployeeRolModel devRol = new ()
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
