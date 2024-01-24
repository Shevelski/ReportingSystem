using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Data.Entity.Migrations.Model;
using static ReportingSystem.Data.SQL.TableTypeSQL;

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

        #region Administrators
        public async Task<Guid> GetAdministratorRoleId(Guid id)
        {
            var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        #endregion

        public async Task<Guid> GetEmployeeRoleId(Guid id)
        {
            var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Employees] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        public async Task<Guid> GetEmployeeRoleIdByName(string name)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Name = @Name";
            var para = new
            {
                Name = name,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }
        public async Task<Guid> GetEmployeeRoleIdByType(int type)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Type = @type";
            var para = new
            {
                Type = type,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }
        public async Task<Guid> GetRoleId(Guid id, string query)
        {
            using var database = Context.ConnectToSQL;
            var para = new
            {
                Id = id,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<CustomerLicenceStatusModel> GetLicenceStatus(Guid id)
        {
            CustomerLicenceStatusModel status = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[StatusLicence] Where Id = @Id";
            var para = new
            {
                Id = id,
            };
            var result = await database.QueryAsync<int>(query, para);

            status.LicenceType = (LicenceType)result.First();
            status.LicenceName = status.LicenceType.GetDisplayName();

            return status;
        }
        public async Task<List<CustomerModel>> GetCustomers()
        {
            List<CustomerModel> customers = [];

            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers]";

                var results = await database.QueryAsync<TableTypeSQL.Customer>(query);

                foreach (var customerSQL in results)
                {

                    CustomerModel customer = new()
                    {
                        Id = customerSQL.Id,
                        FirstName = customerSQL.FirstName,
                        SecondName = customerSQL.SecondName,
                        ThirdName = customerSQL.ThirdName,
                        StatusLicence = await new SQLRead().GetLicenceStatus(customerSQL.StatusLicenceId),
                        Phone = customerSQL.Phone,
                        Email = customerSQL.Email,
                        Password = customerSQL.Password != null ? EncryptionHelper.Decrypt(customerSQL.Password) : "1111",
                        EndTimeLicense = customerSQL.EndTimeLicense,
                        DateRegistration = customerSQL.DateRegistration,
                        Companies = await new SQLRead().GetCompanies(customerSQL.Id.ToString()),
                        HistoryOperations = await new SQLRead().GetHistoryOperations(customerSQL.Id),
                    };
                    if (customer.Companies != null)
                    {
                        foreach (var company in customer.Companies)
                        {
                            if (company != null)
                            {
                                company.Employees = await new SQLRead().GetEmployeesInfo(customerSQL.Id, company.Id);
                            }
                        }
                    }
                    customers.Add(customer);
                }
            }
            return customers;
        }

        public async Task<List<CustomerLicenseOperationModel>> GetHistoryOperations(Guid idCu)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT * FROM[ReportingSystem].[dbo].[HistoryOperations] WHERE[CustomerId] = @Id";
            var para = new
            {
                Id = idCu,
            };
            var results = await database.QueryAsync<TableTypeSQL.HistoryOperation>(query, para);

            List<CustomerLicenseOperationModel> list = [];

            foreach (var ho in results)
            {
                CustomerLicenseOperationModel customerLicenseOperationModel = new()
                {
                    Id = ho.Id,
                    IdCustomer = ho.IdCustomer,
                    DateChange = ho.DateChange,
                    OldEndDateLicence = ho.OldEndDateLicence,
                    NewEndDateLicence = ho.NewEndDateLicence,
                    OldStatus = await new SQLRead().GetLicenceStatus(ho.OldStatus),
                    NewStatus = await new SQLRead().GetLicenceStatus(ho.NewStatus),
                    Price = ho.Price,
                    Period = ho.Period,
                    NameOperation = ho.NameOperation
                };
                list.Add(customerLicenseOperationModel);
            }
            return list; ;
        }

        public async Task<CustomerModel> GetCustomer(Guid id)
        {
            CustomerModel customer = new();

            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers] WHERE Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<TableTypeSQL.Customer>(query, para);

                foreach (var customerSQL in results)
                {
                    customer = new CustomerModel
                    {
                        Id = customerSQL.Id,
                        FirstName = customerSQL.FirstName,
                        SecondName = customerSQL.SecondName,
                        ThirdName = customerSQL.ThirdName
                    };
                    customer.StatusLicence = await new SQLRead().GetLicenceStatus(customer.Id);
                    customer.Phone = customerSQL.Phone;
                    customer.Email = customerSQL.Email;
                    customer.Password = customerSQL.Password;
                    customer.EndTimeLicense = customerSQL.EndTimeLicense;
                    customer.DateRegistration = customerSQL.DateRegistration;
                }
            }
            return customer;
        }
        public async Task<EmployeeRolModel> GetRoleById(Guid id)
        {
            try
            {
                using var database = Context.ConnectToSQL;
                var query = "SELECT [Type] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Id = @Id";
                var paraRol = new
                {
                    Id = id,
                };
                var resultRol = await database.QueryAsync<int>(query, paraRol);
                int typeRol = resultRol.First();
                EmployeeRolModel model = new()
                {
                    RolType = (EmployeeRolStatus)typeRol
                };
                model.RolName = model.RolType.GetDisplayName();

                return model;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task<Guid> GetRolIdByType(EmployeeRolModel rol)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Type = @Type";
            var para = new
            {
                Type = (int)rol.RolType,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }
        public async Task<EmployeePositionModel> GetPositionEmployee(Guid idPos)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Name] FROM [ReportingSystem].[dbo].[EmployeePosition] Where Id = @IdPos";
            var para = new
            {
                IdPos = idPos
            };
            var result = await database.QueryAsync<string>(query, para);
            var position = result.First();
            EmployeePositionModel model = new()
            {
                NamePosition = position
            };
            return model;
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
            using var database = Context.ConnectToSQL;
            var para = new
            {
                Id = id,
            };
            var result = await database.QueryAsync<string>(query, para);

            string encryptPassword = result.First();
            string decryptPassword = EncryptionHelper.Decrypt(encryptPassword);

            if (decryptPassword.Equals(inputPassword))
                return true;
            return false;
        }
        public async Task<EmployeeStatusModel> GetEmployeeStatus(Guid id)
        {
            EmployeeStatusModel status = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[EmployeeStatus] Where Id = @Id";
            var para = new
            {
                Id = id,
            };
            var result = await database.QueryAsync<int>(query, para);

            status.EmployeeStatusType = (EmployeeStatus)result.First();
            status.EmployeeStatusName = status.EmployeeStatusType.GetDisplayName();

            return status;
        }

        public async Task<Guid> GetEmployeeStatusIdByType(EmployeeStatusModel statusInput)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Id] FROM[ReportingSystem].[dbo].[EmployeeStatus] Where Type = @Type";
            var para = new
            {
                Type = statusInput.EmployeeStatusType,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }

        public async Task<Guid> GetEmployeeStatusIdByType(int statusInput)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Id] FROM[ReportingSystem].[dbo].[EmployeeStatus] Where Type = @Type";
            var para = new
            {
                Type = 1,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }

        public async Task<bool> IsBusyEmail(string email)
        {
            using var database = Context.ConnectToSQL;
            var query1 = "SELECT COUNT(*) FROM [ReportingSystem].[dbo].[Customers] WHERE [Email] = @email";
            var query2 = "SELECT COUNT(*) FROM [ReportingSystem].[dbo].[Employees] WHERE [Email] = @email";
            var query3 = "SELECT COUNT(*) FROM [ReportingSystem].[dbo].[Administrators] WHERE [Email] = @email";
            var para = new { email };
            var result1 = await database.QueryAsync<int>(query1, para);
            var result2 = await database.QueryAsync<int>(query2, para);
            var result3 = await database.QueryAsync<int>(query3, para);

            if ((result1.First() + result2.First() + result3.First()) > 0)
                return true;
            else
                return false;
        }

        public async Task<Guid> GetEmployeeStatusIdByType(EmployeeStatus statusInput)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Id] FROM[ReportingSystem].[dbo].[EmployeeStatus] Where Type = @Type";
            var para = new
            {
                Type = statusInput,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<Guid> GetPositionIdByType(EmployeePositionModel positionInput, Guid idCu, Guid idCo)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Id] FROM[ReportingSystem].[dbo].[EmployeePosition] Where Name = @Name AND CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                Name = positionInput.NamePosition,
                CustomerId = idCu,
                CompanyId = idCo
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<Guid> GetPositionIdByName(string name, Guid idCu, Guid idCo)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Id] FROM[ReportingSystem].[dbo].[EmployeePosition] Where Name = @Name AND CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                Name = name,
                CustomerId = idCu,
                CompanyId = idCo
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<EmployeeModel> GetEmployeeAdministrator(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT * FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
            var para = new
            {
                Id = id,
            };

            var results = await database.QueryAsync<TableTypeSQL.Administrator>(query, para);
            foreach (var administrator in results)
            {
                employee.Id = administrator.Id;
                employee.CompanyId = Guid.Empty;
                employee.CustomerId = Guid.Empty;
                employee.FirstName = administrator.FirstName;
                employee.SecondName = administrator.SecondName;
                employee.ThirdName = administrator.ThirdName;
                employee.PhoneWork = administrator.PhoneWork;
                employee.EmailWork = administrator.EmailWork;
                employee.Login = administrator.Login;
                employee.Password = administrator.Password != null ? EncryptionHelper.Decrypt(administrator.Password) : "";
                employee.Status = await GetEmployeeStatus(administrator.Status); ;
                employee.BirthDate = administrator.BirthDate;
                employee.Rol = await new SQLRead().GetRoleById(administrator.Rol);
            };

            return employee;
        }
        public async Task<EmployeeModel> GetEmployeeCustomer(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";
            var para = new
            {
                Id = id,
            };

            var results = await database.QueryAsync<TableTypeSQL.Customer>(query, para);
            foreach (var customer in results)
            {
                employee.Id = customer.Id;
                employee.CompanyId = Guid.Empty;
                employee.CustomerId = Guid.Empty;
                employee.FirstName = customer.FirstName;
                employee.SecondName = customer.SecondName;
                employee.ThirdName = customer.ThirdName;

                employee.PhoneWork = customer.Phone;
                employee.EmailWork = customer.Email;
                employee.Login = customer.Login;
                employee.Password = customer.Password != null ? EncryptionHelper.Decrypt(customer.Password) : "";
                employee.Rol = new EmployeeRolModel
                {
                    RolType = EmployeeRolStatus.Customer
                };
                employee.Rol.RolName = employee.Rol.RolType.GetDisplayName();
            }
            return employee;
        }
        public async Task<EmployeeModel> GetEmployeeData(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT * FROM [ReportingSystem].[dbo].[Employees] Where Id = @Id";
            var para = new
            {
                Id = id,
            };

            var results = await database.QueryAsync<TableTypeSQL.Employee>(query, para);
            foreach (var employees in results)
            {
                employee.Id = employees.Id;
                employee.CompanyId = employees.CompanyId;
                employee.CustomerId = employees.CustomerId;
                employee.FirstName = employees.FirstName;
                employee.SecondName = employees.SecondName;
                employee.ThirdName = employees.ThirdName;
                employee.PhoneWork = employees.PhoneWork;
                employee.PhoneSelf = employees.PhoneSelf;
                employee.EmailWork = employees.EmailWork;
                employee.EmailSelf = employees.EmailSelf;
                employee.TaxNumber = employees.TaxNumber;
                employee.AddressReg = employees.AddressReg;
                employee.AddressFact = employees.AddressFact;
                employee.Photo = employees.Photo;
                employee.Login = employees.Login;
                employee.Password = employees.Password != null ? EncryptionHelper.Decrypt(employees.Password) : "";
                employee.Salary = employees.Salary;
                employee.AddSalary = employees.AddSalary;
                employee.Status = await GetEmployeeStatus(employees.Status);
                employee.BirthDate = employees.BirthDate;
                employee.WorkStartDate = employees.WorkStartDate;
                employee.WorkEndDate = employees.WorkEndDate;
                employee.Rol = await new SQLRead().GetRoleById(employees.Rol);
                employee.Position = await GetPositionEmployee(employees.Position);
            };

            return employee;
        }

        public async Task<EmployeeModel> GetEmployee(Guid idCu, Guid idCo, Guid idEm)
        {
            EmployeeModel employee = new();
            if (!idCu.Equals(Guid.Empty) && !idCo.Equals(Guid.Empty))
            {
                return await GetEmployeeData(idEm);
            }
            using (var database = Context.ConnectToSQL)
            {

                var adminTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var customerTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";

                var paraAdm = new
                {
                    Id = idEm,
                };
                var paraCu = new
                {
                    Id = idCu,
                };
                var resultAdmin = await database.QueryAsync<Guid>(adminTableQuery, paraAdm);
                var resultCustomer = await database.QueryAsync<Guid>(customerTableQuery, paraCu);

                if (resultAdmin.Any())
                {
                    return employee = await new SQLRead().GetEmployeeAdministrator(idEm);
                }
                if (resultCustomer.Any())
                {
                    return employee = await GetEmployeeCustomer(idCu);
                }
            }
            return employee;
        }
        
        public async Task<List<EmployeeModel>> GetAdministrators()
        {
            List<EmployeeModel>? employees = [];

            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators]";
                var resultEmployees = await database.QueryAsync<Guid>(TableQuery);

                if (resultEmployees.Any())
                {
                    var x = resultEmployees;
                    foreach (var emp in resultEmployees)
                    {
                        EmployeeModel employee = await GetAdministrator(emp);
                        employees.Add(employee);
                    }
                    return employees;
                }
            }
            return employees;
        }

        public async Task<EmployeeModel> GetAdministrator(Guid id)
        {
            EmployeeModel employee = new();
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                
                try
                {
                    var results = await database.QueryAsync<TableTypeSQL.Employee>(query, para);
                    foreach (var employees in results)
                    {
                        employee.Id = employees.Id;
                        employee.CompanyId = employees.CompanyId;
                        employee.CustomerId = employees.CustomerId;
                        employee.FirstName = employees.FirstName;
                        employee.SecondName = employees.SecondName;
                        employee.ThirdName = employees.ThirdName;
                        employee.PhoneWork = employees.PhoneWork;
                        employee.PhoneSelf = employees.PhoneSelf;
                        employee.EmailWork = employees.EmailWork;
                        employee.EmailSelf = employees.EmailSelf;
                        employee.TaxNumber = employees.TaxNumber;
                        employee.AddressReg = employees.AddressReg;
                        employee.AddressFact = employees.AddressFact;
                        employee.Photo = employees.Photo;
                        employee.Login = employees.Login;
                        employee.Password = employees.Password != null ? EncryptionHelper.Decrypt(employees.Password) : "";
                        employee.Salary = employees.Salary;
                        employee.AddSalary = employees.AddSalary;
                        employee.Status = await GetEmployeeStatus(employees.Status);

                        employee.BirthDate = employees.BirthDate;
                        employee.WorkStartDate = employees.WorkStartDate;
                        employee.WorkEndDate = employees.WorkEndDate;
                        employee.Rol = await new SQLRead().GetRoleById(employees.Rol);
                        
                    };
                }
                catch (Exception)
                {

                    throw;
                }
                
                
            }
            return employee;
        }

        

        public async Task<List<EmployeeModel>> GetEmployeesInfo(Guid idCu, Guid idCo)
        {
            List<EmployeeModel>? employees = [];

            if (idCu.Equals(Guid.Empty) || idCo.Equals(Guid.Empty))
            {
                return employees;
            }
            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo";

                var para = new
                {
                    IdCu = idCu,
                    IdCo = idCo,
                };
                var resultEmployees = await database.QueryAsync<Guid>(TableQuery, para);

                if (resultEmployees.Any())
                {
                    var x = resultEmployees;
                    foreach (var emp in resultEmployees)
                    {
                        EmployeeModel employee = await GetEmployeeCustomer(emp);
                        employees.Add(employee);
                    }
                    return employees;
                }
            }
            return employees;
        }

        public async Task<List<EmployeeModel>> GetEmployees(Guid idCu, Guid idCo)
        {
            List<EmployeeModel>? employees = [];

            if (idCu.Equals(Guid.Empty) || idCo.Equals(Guid.Empty))
            {
                return employees;
            }
            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo";

                var para = new
                {
                    IdCu = idCu,
                    IdCo = idCo,
                };
                var resultEmployees = await database.QueryAsync<Guid>(TableQuery, para);

                if (resultEmployees.Any())
                {
                    var x = resultEmployees;
                    foreach (var emp in resultEmployees)
                    {
                        //EmployeeModel employee = await GetEmployeeCustomer(emp);
                        EmployeeModel employee = await GetEmployeeData(emp);
                        employees.Add(employee);
                    }
                    return employees;
                }
            }
            return employees;
        }

        public async Task<CompanyStatusModel?> GetCompanyStatus(Guid id)
        {
            CompanyStatusModel companyStatusModel = new();
            using var database = Context.ConnectToSQL;
            var query = "SELECT[Type] FROM[ReportingSystem].[dbo].[CompanyStatus] Where Id = @Id";
            var para = new
            {
                Id = id,
            };
            var result = await database.QueryAsync<int>(query, para);

            companyStatusModel.CompanyStatusType = (CompanyStatus)result.First();
            companyStatusModel.CompanyStatusName = companyStatusModel.CompanyStatusType.GetDisplayName();

            return companyStatusModel;
        }
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            List<CompanyModel> companies = [];
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
                    CompanyModel company1 = new()
                    {
                        Id = company.Id,
                        Name = company.Name,
                        Code = company.Code,
                        Phone = company.Phone,
                        Address = company.Address,
                        IdCustomer = company.CustomerId,
                        Actions = company.Actions,
                        Email = company.Email,
                        RegistrationDate = company.RegistrationDate,
                        StatutCapital = company.StatutCapital,
                        StatusWeb = company.StatusWeb,
                        Status = await new SQLRead().GetCompanyStatus(company.Status),
                        Chief = await new SQLRead().GetEmployeeData(company.Chief)
                    };
                    companies.Add(company1);
                }
            }
            return companies;
        }
        public async Task<Guid> GetCompanyStatusId(CompanyStatusModel companyStatus)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[CompanyStatus] WHERE Type = @Type";
            var para = new
            {
                Type = (int)companyStatus.CompanyStatusType,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }

        public async Task<Guid> GetCompanyStatusId(CompanyStatus status)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[CompanyStatus] WHERE Type = @Type";
            var para = new
            {
                Type = status,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }


        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            await Task.Delay(10);
            List<CompanyModel> companies = [];

            CompanyStatusModel companyStatus = new()
            {
                CompanyStatusType = CompanyStatus.Actual
            };
            companyStatus.CompanyStatusName = companyStatus.CompanyStatusType.GetDisplayName();
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
                    CompanyModel company1 = new()
                    {
                        Id = company.Id,
                        Name = company.Name,
                        Code = company.Code,
                        Phone = company.Phone,
                        Address = company.Address,
                        IdCustomer = company.CustomerId,
                        Actions = company.Actions,
                        Email = company.Email,
                        RegistrationDate = company.RegistrationDate,
                        StatutCapital = company.StatutCapital,
                        StatusWeb = company.StatusWeb,
                        Status = await new SQLRead().GetCompanyStatus(company.Status),
                        Chief = await new SQLRead().GetEmployeeData(company.Chief)
                    };
                    companies.Add(company1);
                }
            }
            return companies;
        }
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            List<EmployeeRolModel> rolls = [];
            using (var database = Context.ConnectToSQL)
            {
                var query = "SELECT [RolId] FROM [ReportingSystem].[dbo].[CompanyRolls] Where CustomerId = @CustomerId AND CompanyId = @CompanyId";
                var para = new
                {
                    CustomerId = idCu,
                    CompanyId = idCo,
                };
                var result = await database.QueryAsync<Guid>(query, para);
                foreach (var rollsIds in result)
                {
                    
                    try
                    {
                        var rol = await GetRoleById(rollsIds);
                        rolls.Add(rol);
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync(ex.ToString());
                        throw;
                    }
                    
                }
                
                
            }
            return rolls;
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

        public async Task<int> GetEmployeePositionCount(Guid idCu, Guid idCo)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT COUNT(*) FROM[ReportingSystem].[dbo].[EmployeePosition] WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo
            };
            var result = await database.QueryAsync<int>(query, para);
            return result.First();
        }
    }
}
