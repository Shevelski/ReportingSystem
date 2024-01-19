using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

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
        public async Task<List<CustomerModel>> LoadCustomersFromJson()
        {
            //await Task.Delay(10); // Simulate async delay

            if (File.Exists(Context.Json) && new FileInfo(Context.Json).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(Context.Json))
                {
                    jsonData = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null && data.Customers != null)
                {
                    return data.Customers;
                }
            }
            return [];
        }
        public async Task<List<EmployeeModel>> LoadAdministratorsFromJson()
        {

            if (File.Exists(Context.Json) && new FileInfo(Context.Json).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(Context.Json))
                {
                    jsonData = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null && data.Administrators != null)
                {
                    return data.Administrators;
                }
            }
            return [];
        }

        public async Task<bool> IsBusyEmail(string email)
        {
            var administrators = await LoadAdministratorsFromJson();

            if (administrators == null)
            {
                return false;
            }

            foreach (var administrator in administrators)
            {
                if (administrator.emailWork != null && administrator.emailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            var customers = await LoadCustomersFromJson();

            if (customers == null)
            {
                return false;
            }

            foreach (var customer in customers)
            {
                if (customer.Email != null && customer.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                if (customer.Companies != null)
                {
                    foreach (var company in customer.Companies)
                    {
                        if (company.Employees != null)
                        {
                            foreach (var employee in company.Employees)
                            {
                                if (employee.emailWork != null && employee.emailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }


        public async Task<EmployeeModel?> GetEmployee(Guid idCu, Guid idCo, Guid idEm) 
        {
            //if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo) || !Guid.TryParse(ar[2], out Guid idEm))
            //{
            //    return null;
            //}

            if (idCu == Guid.Empty && idCo == Guid.Empty && idEm != Guid.Empty)
            {
                List<EmployeeModel>? developers = await LoadAdministratorsFromJson();

                

                var developer = developers.First(dev => dev.id.Equals(idEm));

                var dev = developer;

                if (dev != null && dev.password != null)
                {
                    dev.password = EncryptionHelper.Decrypt(dev.password);
                }

                return dev;
            }

            //var customers = DatabaseMoq.Customers;

            //if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            //{
            //    return null;
            //}

            List<CustomerModel>? customers = await LoadCustomersFromJson();

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(idCu));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            var companies = customer.Companies;

            //if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
            //{
            //    return null;
            //}

            var company = companies.FirstOrDefault(comp => comp.Id.Equals(idCo));

            if (company == null)
            {
                return null;
            }

            var employees = company.Employees;

            if (employees == null)
            {
                return null;
            }
            //if (employees == null || !Guid.TryParse(idEm, out Guid idEmployee))
            //{
            //    return null;
            //}

            var employee = employees.FirstOrDefault(comp => comp.id.Equals(idEm));

            var empl = employee;

            if (empl != null && empl.password != null)
            {
                empl.password = EncryptionHelper.Decrypt(empl.password);
            }

            return empl;
        }

        public async Task<List<EmployeeModel>?> GetAdministrators()
        {

            List<EmployeeModel>? administrators = await LoadAdministratorsFromJson();

            if (administrators == null)
            {
                return null;
            }

            List<EmployeeModel> list = administrators;

            foreach (var employee in list)
            {
                if (employee.password != null)
                {
                    employee.password = EncryptionHelper.Decrypt(employee.password);
                }
            }

            return list;
        }

        public async Task<List<EmployeeModel>?> GetEmployees(Guid idCu, Guid idCo)
        {
            List<CustomerModel>? customers = await LoadCustomersFromJson();

            //if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            //{
            //    return null;
            //}

            var customer = customers?.FirstOrDefault(cu => cu.Id.Equals(idCu));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            var companies = customer.Companies;

            //if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
            //{
            //    return null;
            //}

            var company = companies.FirstOrDefault(comp => comp.Id.Equals(idCo));

            if (company == null)
            {
                return null;
            }
            List<EmployeeModel> list = [];
            if (company.Employees == null)
            {
                return null;
            }
            list = company.Employees;

            foreach (var employee in list)
            {
                if (employee.password != null)
                {
                    employee.password = EncryptionHelper.Decrypt(employee.password);
                }
            }

            return list;
        }

        //public async Task<List<CustomerModel>?> GetCheckCompany(string id)
        //{
        //    if (Guid.TryParse(id, out Guid guid) && companiesData.TryGetValue(guid, out var companyDetails))
        //    {
        //        companiesData.Remove(guid);
        //        DatabaseMoq.UpdateJson();
        //        return companyDetails;
        //    }
        //    return null;
        //}

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
