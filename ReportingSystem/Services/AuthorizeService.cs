using Dapper;
using Newtonsoft.Json;
using ReportingSystem.Data;
using ReportingSystem.Data.SQL;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Authorize;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class AuthorizeService
    {
        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        List<EmployeeModel> administrators = new List<EmployeeModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();
        EmployeeModel employee = new EmployeeModel();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        AuthorizeModel authorize = new AuthorizeModel();
        List<EmployeeRolModel> employeeRolModels = new List<EmployeeRolModel>();

        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }

        //public AuthorizeModel? CheckEmail(string email)
        //{
        //    AuthorizeModel? result = new AuthorizeModel();

        //    if (DatabaseMoq.Administrators != null)
        //    {
        //        administrators = DatabaseMoq.Administrators;
        //        foreach (var administrator in administrators)
        //        {
        //            if (administrator.emailWork != null && administrator.emailWork.ToLower().Equals(email.ToLower()))
        //            {
        //                authorize.Email = administrator.emailWork;
        //                authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
        //                authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
        //                authorize.Role = administrator.rol;
        //                return authorize;
        //            }
        //        }
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        foreach (var customer in customers)
        //        {
        //            if (customer.email != null && customer.email.ToLower().Equals(email.ToLower()))
        //            {
        //                authorize.Email = customer.email;
        //                authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
        //                authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
        //                authorize.Role = new EmployeeRolModel()
        //                {
        //                    rolType = EmployeeRolStatus.Customer,
        //                    rolName = EmployeeRolStatus.Customer.GetDisplayName(),
        //                };
        //                return authorize;
        //            }

        //            if (customer.companies != null)
        //            {
        //                foreach (var company in customer.companies)
        //                {
        //                    if (company.employees != null)
        //                    {
        //                        foreach (var employee in company.employees)
        //                        {
        //                            if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
        //                            {
        //                                authorize.Email = employee.emailWork;
        //                                authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                                authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
        //                                authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
        //                                authorize.Role = new EmployeeRolModel();
        //                                authorize.Role = employee.rol;
        //                                return authorize;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //    authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailFailed;
        //    authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailFailed.GetDisplayName();
        //    return authorize;
        //}

        //public AuthorizeModel? CheckPassword(string email, string password)
        //{
        //    if (DatabaseMoq.Administrators != null)
        //    {
        //        administrators = DatabaseMoq.Administrators;
        //        foreach (var administrator in administrators)
        //        {
        //            if (administrator.emailWork == email)
        //            {
        //                if (administrator.password != null && administrator.password.Equals(password))//EncryptionHelper.Decrypt(administrator.password).Equals(password))
        //                {
        //                    authorize.Email = administrator.emailWork;
        //                    authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                    authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
        //                    authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
        //                    authorize.Role = administrator.rol;
        //                    authorize.Employee = administrator;
        //                    return authorize;
        //                }
        //            }
        //        }
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        foreach (var customer in customers)
        //        {
        //            if (customer.email == email)
        //            {
        //                if (customer.password != null && customer.password.Equals(password))//EncryptionHelper.Decrypt(customer.password).Equals(password))
        //                {
        //                    authorize.Email = customer.email;
        //                    authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                    authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
        //                    authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
        //                    authorize.Role = new EmployeeRolModel()
        //                    {
        //                        rolType = EmployeeRolStatus.Customer,
        //                        rolName = EmployeeRolStatus.Customer.GetDisplayName(),
        //                    };
        //                    authorize.Employee = new EmployeeModel()
        //                    {
        //                        customerId = customer.id,
        //                        firstName = customer.firstName,
        //                        secondName = customer.secondName,
        //                        thirdName = customer.thirdName,
        //                        rol = new EmployeeRolModel()
        //                        {
        //                            rolType = EmployeeRolStatus.Customer,
        //                            rolName = EmployeeRolStatus.Customer.GetDisplayName(),
        //                        }
        //                    };
        //                    return authorize;
        //                }
        //            }


        //            if (customer.companies != null)
        //            {
        //                foreach (var company in customer.companies)
        //                {
        //                    if (company.employees != null)
        //                    {
        //                        foreach (var employee in company.employees)
        //                        {
        //                            if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
        //                            {
        //                                authorize.Email = employee.emailWork;
        //                                authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //                                authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
        //                                authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
        //                                authorize.Role = new EmployeeRolModel();
        //                                authorize.Role = employee.rol;

        //                                if (employee.password != null && employee.password.Equals(password))//EncryptionHelper.Decrypt(employee.password).Equals(password))
        //                                {
        //                                    authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
        //                                    authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
        //                                    authorize.Employee = employee;
        //                                    return authorize;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
        //    authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordFailed;
        //    authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordFailed.GetDisplayName();
        //    return authorize;
        //}
        public async Task<AuthorizeModel> CheckEmailSQL(string email)
        {
            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            AuthorizeStatusModel authorizeStatusModel = authorize.AuthorizeStatusModel;

            using (var database = Context.ConnectToSQL)
            {
                var adminTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators] Where EmailWork = @email";
                var customerTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Customers] Where email = @email";
                var employeeTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Employees] Where EmailWork = @email";

                var para = new
                {
                    email = email,
                };
                var resultAdmin = await database.QueryAsync<Guid>(adminTableQuery, para);
                var resultCustomer = await database.QueryAsync<Guid>(customerTableQuery, para);
                var resultEmployee = await database.QueryAsync<Guid>(employeeTableQuery, para);

                Guid id;

                int count = 0;
                if (resultAdmin.Any())
                {
                    count++;
                    id = resultAdmin.First();
                    Guid rolId = await new SQLRead().GetAdministratorRoleId(id);
                    authorize.Role = await new SQLRead().GetRoleById(rolId);
                }
                if (resultCustomer.Any())
                {
                    count++;
                    id = resultCustomer.First();
                    authorize.Role = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.Customer,
                        rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                    };
                }
                if (resultEmployee.Any())
                {
                    count++;
                    id = resultEmployee.First();
                    Guid rolId = await new SQLRead().GetEmployeeRoleId(id);
                    authorize.Role = await new SQLRead().GetRoleById(rolId);
                }

                authorize.Email = email;
                authorizeStatusModel.authorizeStatusType = (count == 1) ? AuthorizeStatus.EmailOk : AuthorizeStatus.EmailFailed;
                authorizeStatusModel.authorizeStatusName = authorizeStatusModel.authorizeStatusType.GetDisplayName();

                return authorize;
            }
        }
        public AuthorizeModel? CheckEmailJson(string email)
        {
            List<CustomerModel>? Customers = new List<CustomerModel>();
            List<EmployeeModel>? Administrators = new List<EmployeeModel>();

            if (File.Exists(Context.Json) && new FileInfo(Context.Json).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(Context.Json))
                {
                    jsonData = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null)
                {
                    Customers = data.Customers;
                    Administrators = data.Administrators;
                }
            }

            if (Administrators != null)
            {
                foreach (var administrator in Administrators)
                {
                    if (administrator.emailWork != null && administrator.emailWork.ToLower().Equals(email.ToLower()))
                    {
                        authorize.Email = administrator.emailWork;
                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                        authorize.Role = administrator.rol;
                        return authorize;
                    }
                }
            }

            if (Customers != null)
            {
                foreach (var customer in Customers)
                {
                    if (customer.email != null && customer.email.ToLower().Equals(email.ToLower()))
                    {
                        authorize.Email = customer.email;
                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                        authorize.Role = new EmployeeRolModel()
                        {
                            rolType = EmployeeRolStatus.Customer,
                            rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                        };
                        return authorize;
                    }

                    if (customer.companies != null)
                    {
                        foreach (var company in customer.companies)
                        {
                            if (company.employees != null)
                            {
                                foreach (var employee in company.employees)
                                {
                                    if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
                                    {
                                        authorize.Email = employee.emailWork;
                                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                                        authorize.Role = new EmployeeRolModel();
                                        authorize.Role = employee.rol;
                                        return authorize;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailFailed;
            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailFailed.GetDisplayName();
            return authorize;
        }
        public async Task<AuthorizeModel> CheckPasswordSQL(string email, string password)
        {
            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            AuthorizeStatusModel authorizeStatusModel = authorize.AuthorizeStatusModel;

            using (var database = Context.ConnectToSQL)
            {
                var adminTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators] Where EmailWork = @email";
                var customerTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Customers] Where email = @email";
                var employeeTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Employees] Where EmailWork = @email";

                var para = new
                {
                    email = email,
                };
                var resultAdmin = await database.QueryAsync<Guid>(adminTableQuery, para);
                var resultCustomer = await database.QueryAsync<Guid>(customerTableQuery, para);
                var resultEmployee = await database.QueryAsync<Guid>(employeeTableQuery, para);

                Guid id;

                int count = 0;
                if (resultAdmin.Any())
                {
                    count++;
                    id = resultAdmin.First();
                    Guid rolId = await new SQLRead().GetAdministratorRoleId(id);
                    authorize.Role = await new SQLRead().GetRoleById(rolId);
                    if (await new SQLRead().IsPasswordAdminOk(id, password))
                    {
                        authorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                        authorizeStatusModel.authorizeStatusName = authorizeStatusModel.authorizeStatusType.GetDisplayName();
                        authorize.Employee = await new SQLRead().GetEmployeeAdministrator(id);
                    }
                }
                if (resultCustomer.Any())
                {
                    count++;
                    id = resultCustomer.First();
                    authorize.Role = new EmployeeRolModel()
                    {
                        rolType = EmployeeRolStatus.Customer,
                        rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                    };
                    if (await new SQLRead().IsPasswordCustomerOk(id, password))
                    {
                        authorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                        authorizeStatusModel.authorizeStatusName = authorizeStatusModel.authorizeStatusType.GetDisplayName();
                    }
                }
                if (resultEmployee.Any())
                {
                    count++;
                    id = resultEmployee.First();
                    Guid rolId = await new SQLRead().GetEmployeeRoleId(id);
                    authorize.Role = await new SQLRead().GetRoleById(rolId);
                    if (await new SQLRead().IsPasswordEmployeeOk(id, password))
                    {
                        authorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                        authorizeStatusModel.authorizeStatusName = authorizeStatusModel.authorizeStatusType.GetDisplayName();
                    }
                }

                authorize.Email = email;
                authorizeStatusModel.authorizeStatusType = (count == 1) ? AuthorizeStatus.PasswordOk : AuthorizeStatus.PasswordFailed;
                authorizeStatusModel.authorizeStatusName = authorizeStatusModel.authorizeStatusType.GetDisplayName();

                return authorize;
            }
        }
        public AuthorizeModel? CheckPasswordJson(string email, string password)
        {

            List<CustomerModel>? Customers = new List<CustomerModel>();
            List<EmployeeModel>? Administrators = new List<EmployeeModel>();

            if (File.Exists(Context.Json) && new FileInfo(Context.Json).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(Context.Json))
                {
                    jsonData = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<DatabaseData>(jsonData);
                if (data != null)
                {
                    Customers = data.Customers;
                    Administrators = data.Administrators;
                }
            }

            if (Administrators != null)
            {
                administrators = Administrators;
                foreach (var administrator in administrators)
                {
                    if (administrator.emailWork == email)
                    {
                        if (administrator.password != null && administrator.password.Equals(password))//EncryptionHelper.Decrypt(administrator.password).Equals(password))
                        {
                            authorize.Email = administrator.emailWork;
                            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
                            authorize.Role = administrator.rol;
                            authorize.Employee = administrator;
                            return authorize;
                        }
                    }
                }
            }

            if (Customers != null)
            {
                customers = Customers;
                foreach (var customer in customers)
                {
                    if (customer.email == email)
                    {
                        if (customer.password != null && customer.password.Equals(password))//EncryptionHelper.Decrypt(customer.password).Equals(password))
                        {
                            authorize.Email = customer.email;
                            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
                            authorize.Role = new EmployeeRolModel()
                            {
                                rolType = EmployeeRolStatus.Customer,
                                rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                            };
                            authorize.Employee = new EmployeeModel()
                            {
                                customerId = customer.id,
                                firstName = customer.firstName,
                                secondName = customer.secondName,
                                thirdName = customer.thirdName,
                                rol = new EmployeeRolModel()
                                {
                                    rolType = EmployeeRolStatus.Customer,
                                    rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                                }
                            };
                            return authorize;
                        }
                    }


                    if (customer.companies != null)
                    {
                        foreach (var company in customer.companies)
                        {
                            if (company.employees != null)
                            {
                                foreach (var employee in company.employees)
                                {
                                    if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
                                    {
                                        authorize.Email = employee.emailWork;
                                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                                        authorize.Role = new EmployeeRolModel();
                                        authorize.Role = employee.rol;

                                        if (employee.password != null && employee.password.Equals(password))//EncryptionHelper.Decrypt(employee.password).Equals(password))
                                        {
                                            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                                            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
                                            authorize.Employee = employee;
                                            return authorize;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordFailed;
            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordFailed.GetDisplayName();
            return authorize;
        }
        public string? GetRolController(AuthorizeModel authorizeModel)
            {
            employeeRolModels = DefaultEmployeeRolls.Get();
            foreach (var item in employeeRolModels)
            {
       
                if (authorize.Role != null && authorize.Role.rolType.Equals(item.rolType))
                {
                    switch (item.rolType)
                    {
                        case EmployeeRolStatus.Administrator:
                            return "EUAdministrator";
                        case EmployeeRolStatus.Developer:
                            return "EUDeveloper";
                        case EmployeeRolStatus.DevAdministrator:
                            return "EUDevAdministrator";
                        case EmployeeRolStatus.ProjectManager:
                            return "EUProjectManager";
                        case EmployeeRolStatus.User:
                            return "EUUser";
                        case EmployeeRolStatus.Customer:
                            return "EUCustomer";
                        case EmployeeRolStatus.CEO:
                            return "EUCEO";
                        default:
                            return "EUUser";
                    }
                }
            }
            return null;
        }
    }
}
