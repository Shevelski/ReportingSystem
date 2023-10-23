using Dapper;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;

namespace ReportingSystem.Data.SQL
{
    public class SQLRead
    {
        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }
        public async Task<Guid> GetAdministratorRoleId(Guid id)
        {
            var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        public async Task<Guid> GetEmployeeRoleId(Guid id)
        {
            var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Employees] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        public async Task<Guid> GetRoleId(Guid id, string query)
        {
            using (var database = Context.ConnectToSQL)
            {
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<Guid>(query, para);

                return result.First();
            }
        }
        public async Task<CustomerLicenceStatusModel> GetLicenceStatus(Guid id)
        {
            CustomerLicenceStatusModel status = new CustomerLicenceStatusModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[StatusLicence] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<int>(query, para);

                status.licenceType = (LicenceType)result.First();
                status.licenceName = status.licenceType.GetDisplayName();

                return status;
            }
        }
        public async Task<List<CustomerModel>> GetCustomers(Guid id)
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers]";

                var results = await database.QueryAsync<TableTypeSQL.Customer>(query);

                foreach (var customerSQL in results)
                {

                    CustomerModel customer = new CustomerModel();
                    customer.id = customerSQL.Id;
                    customer.firstName = customerSQL.FirstName;
                    customer.secondName = customerSQL.SecondName;
                    customer.thirdName = customerSQL.ThirdName;
                    customer.statusLicence = await new SQLRead().GetLicenceStatus(customer.id);
                    customer.phone = customerSQL.Phone;
                    customer.email = customerSQL.Email;
                    customer.password = customerSQL.Password;
                    customer.endTimeLicense = customerSQL.EndTimeLicense;
                    customer.dateRegistration = customerSQL.DateRegistration;
                    customers.Add(customer);
                }
            }
            return customers;
        }
        public async Task<CustomerModel> GetCustomer(Guid id)
        {
            CustomerModel customer = new CustomerModel();

            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers] WHERE Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<TableTypeSQL.Customer>(query,para);

                foreach (var customerSQL in results)
                {
                    customer = new CustomerModel();
                    customer.id = customerSQL.Id;
                    customer.firstName = customerSQL.FirstName;
                    customer.secondName = customerSQL.SecondName;
                    customer.thirdName = customerSQL.ThirdName;
                    customer.statusLicence = await new SQLRead().GetLicenceStatus(customer.id);
                    customer.phone = customerSQL.Phone;
                    customer.email = customerSQL.Email;
                    customer.password = customerSQL.Password;
                    customer.endTimeLicense = customerSQL.EndTimeLicense;
                    customer.dateRegistration = customerSQL.DateRegistration;
                }
            }
            return customer;
        }
        public async Task<EmployeeRolModel> GetRoleById(Guid id)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT [Type] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Id = @Id";
                var paraRol = new
                {
                    Id = id,
                };
                var resultRol = await database.QueryAsync<int>(query, paraRol);
                int typeRol = resultRol.First();
                EmployeeRolModel model = new EmployeeRolModel();
                model.rolType = (EmployeeRolStatus)typeRol;
                model.rolName = model.rolType.GetDisplayName();

                return model;
            }
        }
        public async Task<Guid> GetRolIdByType(EmployeeRolModel rol)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Type = @Type";
                var para = new
                {
                    Type = (int)rol.rolType,
                };
                var result = await database.QueryAsync<Guid>(query, para);
                return result.First();
            }
        }
        public async Task<EmployeePositionModel> GetPositionEmployee(Guid idPos)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT [Name] FROM [ReportingSystem].[dbo].[EmployeePosition] Where Id = @IdPos";
                var para = new
                {
                    IdPos = idPos
                };
                var result = await database.QueryAsync<string>(query, para);
                var position = result.First();
                EmployeePositionModel model = new EmployeePositionModel();
                model.namePosition = position;
                return model;
            }
        }
        public async Task<bool> IsPasswordAdminOk(Guid id, string inputPassword)
        {
            var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
            return await IsPasswordOk(id, inputPassword, query);
        }
        public async Task<bool> IsPasswordEmployeeOk(Guid id, string inputPassword)
        {
            var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Employees] Where Id = @Id";
            return await IsPasswordOk(id, inputPassword, query);
        }
        public async Task<bool> IsPasswordCustomerOk(Guid id, string inputPassword)
        {
            var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";
            return await IsPasswordOk(id, inputPassword, query);

        }
        public async Task<bool> IsPasswordOk(Guid id, string inputPassword, string query)
        {
            using (var database = Context.ConnectToSQL)
            {
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<string>(query, para);

                string encryptPassword = result.First();
                string decryptPassword = encryptPassword;//EncryptionHelper.Decrypt(encryptPassword);

                if (decryptPassword.Equals(inputPassword))
                    return true;
                return false;
            }
        }
        public async Task<EmployeeStatusModel> GetEmployeeStatus(Guid id)
        {
            EmployeeStatusModel status = new EmployeeStatusModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[EmployeeStatus] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<int>(query, para);

                status.employeeStatusType = (EmployeeStatus)result.First();
                status.employeeStatusName = status.employeeStatusType.GetDisplayName();

                return status;
            }
        }
        public async Task<EmployeeModel> GetEmployeeAdministrator(Guid id)
        {
            EmployeeModel employee = new EmployeeModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<TableTypeSQL.Administrator>(query, para);
                foreach (var administrator in results)
                {
                    employee.id = administrator.Id;
                    employee.companyId = Guid.Empty;
                    employee.customerId = Guid.Empty;
                    employee.firstName = administrator.FirstName;
                    employee.secondName = administrator.SecondName;
                    employee.thirdName = administrator.ThirdName;
                    employee.phoneWork = administrator.PhoneWork;
                    employee.emailWork = administrator.EmailWork;
                    employee.login = administrator.Login;
                    employee.password = administrator.Password;//administrator.Password != null ? EncryptionHelper.Decrypt(administrator.Password) : "";
                    employee.status = await GetEmployeeStatus(administrator.Status); ;
                    employee.birthDate = administrator.BirthDate;
                    employee.rol = await new SQLRead().GetRoleById(administrator.Rol);
                };

                return employee;
            }
        }
        public async Task<EmployeeModel> GetEmployeeCustomer(Guid id)
        {
            EmployeeModel employee = new EmployeeModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<TableTypeSQL.Customer>(query, para);
                foreach (var customer in results)
                {
                    employee.id = customer.Id;
                    employee.companyId = Guid.Empty;
                    employee.customerId = Guid.Empty;
                    employee.firstName = customer.FirstName;
                    employee.secondName = customer.SecondName;
                    employee.thirdName = customer.ThirdName;

                    employee.phoneWork = customer.Phone;
                    employee.emailWork = customer.Email;
                    employee.login = customer.Login;
                    employee.password = customer.Password; //customer.Password != null ? EncryptionHelper.Decrypt(customer.Password) : "";
                    employee.rol = new EmployeeRolModel();
                    employee.rol.rolType = EmployeeRolStatus.Customer;
                    employee.rol.rolName = employee.rol.rolType.GetDisplayName();
                }
                return employee;
            }
        }
        public async Task<EmployeeModel> GetEmployeeData(Guid id)
        {
            EmployeeModel employee = new EmployeeModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Employees] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<TableTypeSQL.Employee>(query, para);
                foreach (var employees in results)
                {
                    employee.id = employees.Id;
                    employee.companyId = employees.CompanyId;
                    employee.customerId = employees.CustomerId;
                    employee.firstName = employees.FirstName;
                    employee.secondName = employees.SecondName;
                    employee.thirdName = employees.ThirdName;
                    employee.phoneWork = employees.PhoneWork;
                    employee.phoneSelf = employees.PhoneSelf;
                    employee.emailWork = employees.EmailWork;
                    employee.emailSelf = employees.EmailSelf;
                    employee.taxNumber = employees.TaxNumber;
                    employee.addressReg = employees.AddressReg;
                    employee.addressFact = employees.AddressFact;
                    employee.photo = employees.Photo;
                    employee.login = employees.Login;
                    employee.password = employees.Password;//employees.Password != null ? EncryptionHelper.Decrypt(employees.Password) : "";
                    employee.salary = employees.Salary;
                    employee.addSalary = employees.AddSalary;
                    employee.status = await GetEmployeeStatus(employees.Status);

                    employee.birthDate = employees.BirthDate;
                    employee.workStartDate = employees.WorkStartDate;
                    employee.workEndDate = employees.WorkEndDate;
                    employee.rol = await new SQLRead().GetRoleById(employees.Rol);
                    employee.position = await GetPositionEmployee(employees.Position);
                };

                return employee;
            }
        }
        public async Task<EmployeeModel> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            EmployeeModel employee = new EmployeeModel();
            if (!idCu.Equals(Guid.Empty) && !idCo.Equals(Guid.Empty))
            {
                return employee = await GetEmployeeData(idEm);
            }
            using (var database = Context.ConnectToSQL)
            {

                var adminTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var customerTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";

                var para = new
                {
                    Id = idEm,
                };
                var resultAdmin = await database.QueryAsync<Guid>(adminTableQuery, para);
                var resultCustomer = await database.QueryAsync<Guid>(customerTableQuery, para);

                if (resultAdmin.Any())
                {
                    return employee = await new SQLRead().GetEmployeeAdministrator(idEm);
                }
                if (resultCustomer.Any())
                {
                    return employee = await GetEmployeeCustomer(idEm);
                }
            }
            return employee;
        }
        public async Task<CompanyStatusModel?> GetCompanyStatus(Guid id)
        {
            CompanyStatusModel companyStatusModel = new CompanyStatusModel();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[CompanyStatus] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<int>(query, para);

                companyStatusModel.companyStatusType = (CompanyStatus)result.First();
                companyStatusModel.companyStatusName = companyStatusModel.companyStatusType.GetDisplayName();

                return companyStatusModel;
            }
        }
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            List<CompanyModel> companies = new List<CompanyModel>();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM[ReportingSystem].[dbo].[Companies] Where CustomerId = @CustomerId";
                var para = new
                {
                    CustomerId = idCu,
                };
                var result = await database.QueryAsync<TableTypeSQL.Company>(query, para);
                foreach (var company in result)
                {
                    CompanyModel company1 = new CompanyModel();
                    company1.id = company.Id;
                    company1.name = company.Name;
                    company1.code = company.Code;
                    company1.phone = company.Phone;
                    company1.address = company.Address;
                    company1.idCustomer = company.CustomerId;
                    company1.actions = company.Actions;
                    company1.email = company.Email;
                    company1.registrationDate = company.RegistrationDate;
                    company1.statutCapital = company.StatutCapital;
                    company1.statusWeb = company.StatusWeb;
                    company1.status = await new SQLRead().GetCompanyStatus(company.Status);
                    company1.chief = await new SQLRead().GetEmployeeData(company.Chief);
                    companies.Add(company1);
                }
            }
            return companies;
        }
        public async Task<Guid> GetCompanyStatusId(CompanyStatusModel companyStatus)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[CompanyStatus] WHERE Type = @Type";
                var para = new
                {
                    Type = (int)companyStatus.companyStatusType,
                };
                var result = await database.QueryAsync<Guid>(query, para);
                return result.First();
            }
        }
        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            await Task.Delay(10);
            List<CompanyModel> companies = new List<CompanyModel>();

            CompanyStatusModel companyStatus = new CompanyStatusModel();
            companyStatus.companyStatusType = CompanyStatus.Actual;
            companyStatus.companyStatusName = companyStatus.companyStatusType.GetDisplayName();
            Guid statusCompanyId = await GetCompanyStatusId(companyStatus);


            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM[ReportingSystem].[dbo].[Companies] Where CustomerId = @CustomerId AND Status = @Status";
                var para = new
                {
                    CustomerId = idCu,
                    Status = statusCompanyId,
                };
                var result = await database.QueryAsync<TableTypeSQL.Company>(query, para);
                foreach (var company in result)
                {
                    CompanyModel company1 = new CompanyModel();
                    company1.id = company.Id;
                    company1.name = company.Name;
                    company1.code = company.Code;
                    company1.phone = company.Phone;
                    company1.address = company.Address;
                    company1.idCustomer = company.CustomerId;
                    company1.actions = company.Actions;
                    company1.email = company.Email;
                    company1.registrationDate = company.RegistrationDate;
                    company1.statutCapital = company.StatutCapital;
                    company1.statusWeb = company.StatusWeb;
                    company1.status = await new SQLRead().GetCompanyStatus(company.Status);
                    company1.chief = await new SQLRead().GetEmployeeData(company.Chief);
                    companies.Add(company1);
                }
            }
            return companies;
        }
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            List<EmployeeRolModel> rolls = new List<EmployeeRolModel>();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM[ReportingSystem].[dbo].[CompanyRolls] Where CustomerId = @CustomerId AND CompanyId = @CompanyId";
                var para = new
                {
                    CustomerId = idCu,
                    CompanyId = idCo,
                };
                var result = await database.QueryAsync<Guid>(query, para);
                
                foreach (var rollsIds in result)
                {
                    var rol = await GetRoleById(rollsIds);
                    rolls.Add(rol); 
                }
            }
            return rolls;
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
