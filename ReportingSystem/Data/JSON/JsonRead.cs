using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using static ReportingSystem.Data.SQL.TableTypeSQL;

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
        #region Load
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
        #endregion
        #region Administrators
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
                if (employee.Password != null)
                {
                    employee.Password = EncryptionHelper.Decrypt(employee.Password);
                }
            }

            return list;
        }
        #endregion
        #region Positions
        public async Task<List<EmployeePositionModel>?> GetAllPositions(string idCu, string idCo)
        {
            var customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null)
                {
                    return company.Positions;
                }
            }

            return null;
        }
        public async Task<List<EmployeePositionEmpModel>?> GetAllPositionsWithEmployee(string idCu, string idCo)
        {

            List<EmployeePositionEmpModel>? positions = [];

            var customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Positions == null || company.Employees == null)
                {
                    return null;
                }
                int i = 0;
                foreach (var item in company.Positions)
                {

                    EmployeePositionEmpModel employeePositionEmpModel = new()
                    {
                        NamePosition = item.NamePosition,
                        Employee = company.Employees[i]
                    };
                    positions.Add(employeePositionEmpModel);
                    i++;
                }
                i = 0;
                return positions;

            }

            return null;
        }

        #endregion
        #region Rolls
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            List<CustomerModel>? Customers = await GetCustomers();

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
            EmployeeRolModel devRol = new()
            {
                RolType = EmployeeRolStatus.Developer,
                RolName = EmployeeRolStatus.Developer.GetDisplayName()
            };
            devRols.Add(devRol);
            devRol = new EmployeeRolModel()
            {
                RolType = EmployeeRolStatus.DevAdministrator,
                RolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
            };
            devRols.Add(devRol);
            return devRols;
        }
        public async Task<List<EmployeeModel>?> GetEmployeesByRoll(string idCu, string idCo, string rol)
        {
            List<CustomerModel> customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Employees == null)
                {
                    return null;
                }

                return company.Employees.Where(employee => employee.Rol != null && employee.Rol.RolName != null && employee.Rol.RolName.Equals(rol)).ToList();
            }

            return null;
        }

        #endregion
        #region Customers
            public async Task<List<EmployeePositionModel>?> GetUniqPositions(string idCu, string idCo)
        {
            var customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    var uniquePositions = company.Positions
                        .Where(position => !string.IsNullOrEmpty(position.NamePosition))
                        .GroupBy(position => position.NamePosition)
                        .Select(group => group.First())
                        .ToList();

                    return uniquePositions;
                }
            }
            return null;
        }
        public async Task<List<CustomerModel>?> GetCustomers()
        {
            List<CustomerModel>? Customers = await LoadCustomersFromJson();
            return Customers;
        }
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            List<CustomerModel>? customers = await GetCustomers();
            CustomerModel? customer = customers?.FirstOrDefault(id => id.Equals(idCu));
            return customer;
        }
        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            List<CustomerModel>? Customers = await GetCustomers();

            if (Customers == null || !Guid.TryParse(idCu, out Guid id))
            {
                return null;
            }

            var customer = Customers.FirstOrDefault(co => co.Id.Equals(id));

            if (customer != null && customer.Companies != null)
            {
                return customer.Companies
                    .Where(item => item.Status != null && item.Status.CompanyStatusType.Equals(CompanyStatus.Actual))
                    .ToList();
            }

            return null;
        }
        #endregion
        #region Authorization
        public async Task<bool> IsBusyEmail(string email)
        {
            var administrators = await GetAdministrators();

            if (administrators == null)
            {
                return false;
            }

            foreach (var administrator in administrators)
            {
                if (administrator.EmailWork != null && administrator.EmailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            var customers = await GetCustomers();

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
                                if (employee.EmailWork != null && employee.EmailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
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
        #endregion
        #region Employees
        public async Task<List<EmployeeModel>?> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            List<EmployeeModel> employeesByPosition = new List<EmployeeModel>();
            var customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Employees != null)
                {
                    var employees = company.Employees;
                    employeesByPosition = employees
                        .Where(employee => employee.Position != null && !string.IsNullOrEmpty(employee.Position.NamePosition) && employee.Position.NamePosition.Equals(pos))
                        .ToList();

                    if (employeesByPosition.Any())
                    {
                        return employeesByPosition;
                    }
                }
            }

            return null;
        }
        public async Task<EmployeeModel?> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            //if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo) || !Guid.TryParse(ar[2], out Guid idEm))
            //{
            //    return null;
            //}

            if (idCu == Guid.Empty && idCo == Guid.Empty && idEm != Guid.Empty)
            {
                List<EmployeeModel>? developers = await GetAdministrators();

                var developer = developers?.First(dev => dev.Id.Equals(idEm));

                var dev = developer;

                if (dev != null && dev.Password != null)
                {
                    dev.Password = EncryptionHelper.Decrypt(dev.Password);
                }

                return dev;
            }

            //var customers = DatabaseMoq.Customers;

            //if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            //{
            //    return null;
            //}

            List<CustomerModel>? customers = await GetCustomers();

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

            var employee = employees.FirstOrDefault(comp => comp.Id.Equals(idEm));

            var empl = employee;

            if (empl != null && empl.Password != null)
            {
                empl.Password = EncryptionHelper.Decrypt(empl.Password);
            }

            return empl;
        }
        public async Task<List<EmployeeModel>?> GetEmployees(Guid idCu, Guid idCo)
        {
            List<CustomerModel>? customers = await GetCustomers();

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
                if (employee.Password != null)
                {
                    employee.Password = EncryptionHelper.Decrypt(employee.Password);
                }
            }

            return list;
        }
        #endregion
        #region Categories
        public async Task<List<ProjectCategoryModel>?> GetCategories(string idCu, string idCo)
        {
            List<ProjectCategoryModel> projectCategoryModels = new List<ProjectCategoryModel>();
            ProjectCategoryModel categoryModel = new ProjectCategoryModel();

            var customers = await GetCustomers();

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null)
            {
                return null;
            }

            if (!Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            if (company.Categories == null)
            {
                return null;
            }

            projectCategoryModels = company.Categories;

            return projectCategoryModels;

        }
        #endregion
        #region Companies
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            List<CustomerModel>? Customers = await GetCustomers();

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
            List<CustomerModel>? Customers = await GetCustomers();

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
        #endregion
        #region Projects
        public async Task<List<ProjectModel>?> GetProjects(string idCu, string idCo)
        {
            List<ProjectModel> projects = [];

            List<CustomerModel>? Customers = await GetCustomers();

            if (Customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = Customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null)
            {
                return null;
            }

            if (!Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            if (company.Projects == null)
            {
                return null;
            }

            projects = company.Projects;

            return projects;
        }

        #endregion
    }
}
