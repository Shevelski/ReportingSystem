using Bogus.DataSets;
using Dapper;
using Microsoft.Identity.Client;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            var query = $"SELECT [Rol] FROM [{Context.dbName}].[dbo].[Administrators] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        public async Task<EmployeeModel> GetEmployeeAdministrator(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Administrators] Where Id = @Id";
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
        public async Task<List<EmployeeModel>> GetAdministrators()
        {
            List<EmployeeModel>? employees = [];

            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Administrators]";
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
                var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Administrators] Where Id = @Id";
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
        #endregion
        #region Positions
        public async Task<EmployeePositionModel> GetPositionEmployee(Guid idPos)
        {
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Name] FROM [{Context.dbName}].[dbo].[EmployeePosition] Where Id = @IdPos";
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
        public async Task<Guid> GetPositionIdByType(EmployeePositionModel positionInput, Guid idCu, Guid idCo)
        {
            using var database = Context.ConnectToSQL;
            var query = $"SELECT[Id] FROM [{Context.dbName}].[dbo].[EmployeePosition] Where Name = @Name AND CustomerId = @CustomerId AND CompanyId = @CompanyId";
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
            var query = $"SELECT[Id] FROM [{Context.dbName}].[dbo].[EmployeePosition] Where Name = @Name AND CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                Name = name,
                CustomerId = idCu,
                CompanyId = idCo
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<List<EmployeePositionModel>?> GetAllPositions(string idCu, string idCo)
        {
            List<EmployeePositionModel> list = [];
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[EmployeePosition] Where CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);
            foreach (var ids in result)
            {
                EmployeePositionModel position = await GetPosition(ids);
                list.Add(position);
            }
            return list;
        }
        public async Task<EmployeePositionModel> GetPosition(Guid idPo)
        {
            EmployeePositionModel employeePositionModel = new();
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[EmployeePosition] Where Id = @Id";
            var para = new
            {
                Id = idPo
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<EmployeePosition>(query, para);
            foreach (var position in result)
            {
                employeePositionModel.NamePosition = position.Name;
            };
            return employeePositionModel;
        }
        public async Task<List<EmployeePositionEmpModel>?> GetAllPositionsWithEmployee(string idCu, string idCo)
        {
            List<EmployeePositionEmpModel>? positions = [];

            if (!Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            if (!Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            List<EmployeeModel>? employees = await new SQLRead().GetEmployees(idCustomer, idCompany);

            foreach (var item in employees)
            {
                EmployeePositionEmpModel employeePositionEmpModel = new()
                {
                    NamePosition = item.Position?.NamePosition,
                    Employee = item
                };
                positions.Add(employeePositionEmpModel);
            }
            return positions;
        }
        public async Task<List<EmployeePositionModel>?> GetUniqPositions(string idCu, string idCo)
        {
            var query = $"SELECT DISTINCT [Name] FROM [{Context.dbName}].[dbo].[EmployeePosition] WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId;";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);
            List<EmployeePositionModel> list = [];
            foreach (var position in result)
            {
                EmployeePositionModel employeePositionModel = new()
                {
                    NamePosition = position
                };
                list.Add(employeePositionModel);
            };
            return list;
        }
        #endregion
        #region Rolls
        public async Task<Guid> GetEmployeeRoleId(Guid id)
        {
            var query = $"SELECT [Rol] FROM [{Context.dbName}].[dbo].[Employees] Where Id = @Id";
            return await GetRoleId(id, query);
        }
        public async Task<Guid> GetEmployeeRoleIdByName(string name)
        {
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeRolStatus] Where Name = @Name";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeRolStatus] Where Type = @type";
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
        public async Task<EmployeeRolModel> GetRoleById(Guid id)
        {
            try
            {
                using var database = Context.ConnectToSQL;
                var query = $"SELECT [Type] FROM [{Context.dbName}].[dbo].[EmployeeRolStatus] Where Id = @Id";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeRolStatus] Where Type = @Type";
            var para = new
            {
                Type = (int)rol.RolType,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            List<EmployeeRolModel> rolls = [];
            using (var database = Context.ConnectToSQL)
            {
                var query = $"SELECT [RolId] FROM [{Context.dbName}].[dbo].[CompanyRolls] Where CustomerId = @CustomerId AND CompanyId = @CompanyId";
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
        #endregion
        #region Customers
        public async Task<CustomerLicenceStatusModel> GetLicenceStatus(Guid id)
        {
            CustomerLicenceStatusModel status = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Type] FROM [{Context.dbName}].[dbo].[StatusLicence] Where Id = @Id";
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
                var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Customers]";

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
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[HistoryOperations] WHERE[CustomerId] = @Id";
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
                var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Customers] WHERE Id = @Id";
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
        public async Task<EmployeeModel> GetEmployeeCustomer(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Customers] Where Id = @Id";
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
        #endregion
        #region  Authorization
        public async Task<bool> IsPasswordAdminOk(Guid id, string inputPassword)
        {
            var query = $"SELECT [Password] FROM [{Context.dbName}].[dbo].[Administrators] Where Id = @Id";
            return await IsPasswordOk(id, inputPassword, query);
        }
        public async Task<bool> IsPasswordEmployeeOk(Guid id, string inputPassword)
        {
            var query = $"SELECT [Password] FROM [{Context.dbName}].[dbo].[Employees] Where Id = @Id";
            return await IsPasswordOk(id, inputPassword, query);
        }
        public async Task<bool> IsPasswordCustomerOk(Guid id, string inputPassword)
        {
            var query = $"SELECT [Password] FROM [{Context.dbName}].[dbo].[Customers] Where Id = @Id";
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
        public async Task<bool> IsBusyEmail(string email)
        {
            using var database = Context.ConnectToSQL;
            var query1 = $"SELECT COUNT(*) FROM [{Context.dbName}].[dbo].[Customers] WHERE [Email] = @email";
            var query2 = $"SELECT COUNT(*) FROM [{Context.dbName}].[dbo].[Employees] WHERE [Email] = @email";
            var query3 = $"SELECT COUNT(*) FROM [{Context.dbName}].[dbo].[Administrators] WHERE [Email] = @email";
            var para = new { email };
            var result1 = await database.QueryAsync<int>(query1, para);
            var result2 = await database.QueryAsync<int>(query2, para);
            var result3 = await database.QueryAsync<int>(query3, para);

            if ((result1.First() + result2.First() + result3.First()) > 0)
                return true;
            else
                return false;
        }
        #endregion
        #region Employees
        public async Task<EmployeeStatusModel> GetEmployeeStatus(Guid id)
        {
            EmployeeStatusModel status = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Type] FROM [{Context.dbName}].[dbo].[EmployeeStatus] Where Id = @Id";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeStatus] Where Type = @Type";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeStatus] Where Type = @Type";
            var para = new
            {
                Type = 1,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<Guid> GetEmployeeStatusIdByType(EmployeeStatus statusInput)
        {
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeStatus] Where Type = @Type";
            var para = new
            {
                Type = statusInput,
            };
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        public async Task<EmployeeModel> GetEmployeeData(Guid id)
        {
            EmployeeModel employee = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Employees] Where Id = @Id";
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

                var adminTableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Administrators] Where Id = @Id";
                var customerTableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Customers] Where Id = @Id";

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
        public async Task<List<EmployeeModel>> GetEmployeesInfo(Guid idCu, Guid idCo)
        {
            List<EmployeeModel>? employees = [];

            if (idCu.Equals(Guid.Empty) || idCo.Equals(Guid.Empty))
            {
                return employees;
            }
            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo";

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

                var TableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo";

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
        public async Task<List<EmployeeModel>> GetEmployeesByRoll(string idCu, string idCo, string rol)
        {
            List<EmployeeModel>? employees = [];

            if (idCu.Equals(Guid.Empty) || idCo.Equals(Guid.Empty))
            {
                return employees;
            }
            using (var database = Context.ConnectToSQL)
            {

                var TableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo AND Rol = @Rol";

                var para = new
                {
                    IdCu = idCu,
                    IdCo = idCo,
                    Rol = await new SQLRead().GetEmployeeRoleIdByName(rol)
                };
                var resultEmployees = await database.QueryAsync<Guid>(TableQuery, para);

                if (resultEmployees.Any())
                {
                    var x = resultEmployees;
                    foreach (var emp in resultEmployees)
                    {;
                        EmployeeModel employee = await GetEmployeeData(emp);
                        employees.Add(employee);
                    }
                    return employees;
                }
            }
            return employees;
        }
        public async Task<int> GetEmployeePositionCount(Guid idCu, Guid idCo)
        {
            using var database = Context.ConnectToSQL;
            var query = $"SELECT COUNT (*) FROM [{Context.dbName}].[dbo].[EmployeePosition] WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo
            };
            var result = await database.QueryAsync<int>(query, para);
            return result.First();
        }
        public async Task<List<EmployeeModel>?> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            List<EmployeeModel>? employees = [];

            if (idCu.Equals(Guid.Empty) || idCo.Equals(Guid.Empty))
            {
                return employees;
            }
            using (var database = Context.ConnectToSQL)
            {
                var positionId = await new SQLRead().GetPositionIdByName(pos, Guid.Parse(idCu), Guid.Parse(idCo));
                var TableQuery = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Employees] Where CustomerId = @IdCu AND CompanyId = @IdCo AND Position = @Position";

                var para = new
                {
                    IdCu = idCu,
                    IdCo = idCo,
                    position = positionId
                };
                var resultEmployees = await database.QueryAsync<Guid>(TableQuery, para);

                if (resultEmployees.Any())
                {
                    var x = resultEmployees;
                    foreach (var emp in resultEmployees)
                    {
                        EmployeeModel employee = await GetEmployee(Guid.Parse(idCu), Guid.Parse(idCo), emp);
                        employees.Add(employee);
                    }
                    return employees;
                }
            }
            return employees;
        }
        #endregion
        #region Companies
        public async Task<CompanyStatusModel?> GetCompanyStatus(Guid id)
        {
            CompanyStatusModel companyStatusModel = new();
            using var database = Context.ConnectToSQL;
            var query = $"SELECT [Type] FROM [{Context.dbName}].[dbo].[CompanyStatus] Where Id = @Id";
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
                var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Companies] Where CustomerId = @CustomerId";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[CompanyStatus] WHERE Type = @Type";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[CompanyStatus] WHERE Type = @Type";
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
                var query = $"SELECT * FROM [{Context.dbName}].[dbo].[Companies] Where CustomerId = @CustomerId AND Status = @Status";
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
        #endregion
        #region Projects
        public async Task<List<ProjectModel>?> GetProjects(string idCu, string idCo)
        {
            List<ProjectModel> projects = [];

            var query = $"SELECT[Guid] FROM [{Context.dbName}].[dbo].[Projects] Where CustomerId = @CustomerId AND CompanyId = @CompanyId";
            var para = new
            {
               CustomerId = idCu,
               CompanyId = idCo
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);


            if (result.Any())
            {
                var x = result;
                foreach (var pr in result)
                {
                    ProjectModel project = await GetProject(idCu, idCo, pr);
                    projects.Add(project);
                }
            }
            return projects;
        }
        public async Task<List<Guid>> GetProjectsGuidByCategory(Guid idCu, Guid idCo, int numCat, Guid idCat)
        {
            List<Guid> projects = [];

            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[Projects] Where CustomerId = @CustomerId AND CompanyId = @CompanyId AND CategoryModel{numCat} = @CategoryModel";
            var para = new
            {
               CustomerId = idCu,
               CompanyId = idCo,
               CategoryModel = idCat
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            foreach (var pr in result)
            {
                projects.Add(pr);
            }
            
            return projects;
        }
        public async Task<ProjectModel>? GetProject(string idCu, string idCo, string idPr)
        {
            ProjectModel project = new();

            var query = $"SELECT [*] FROM [{Context.dbName}].[dbo].[Projects] Where CustomerId = @CustomerId AND CompanyId = @CompanyId AND Id = @Id";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idPr
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<TableTypeSQL.Project>(query, para);
            foreach (var readProj in result)
            {
                project.Id = readProj.Id;
                project.CustomerId = readProj.IdCustomer;
                project.CompanyId = readProj.IdCompany;
                project.Name = readProj.Name;
                project.Description = readProj.Description;
                project.ProjectCostsForCompany = readProj.ProjectCostsForCompany;
                project.ProjectCostsForCustomer = readProj.ProjectCostsForCustomer;
                project.StartDate = readProj.StartDate;
                project.PlanDate = readProj.PlanDate;
                project.EndDate = readProj.EndDate;
                project.Status = await GetProjectStatus(readProj.Status);
                project.Head = await GetEmployeeData(readProj.Head);
                //project.Category = await GetCategoryData(readProj.CategoryModel);
                //project.Category = await GetCategoryData(readProj.CategoryModel2);
                //project.Category = await GetCategoryData(readProj.CategoryModel3);
            }
            return project;
        }
        public async Task<ProjectStatusModel?> GetProjectStatus(Guid id)
        {
            var query = $"SELECT [Type] FROM [{Context.dbName}].[dbo].[ProjectStatus] Where Id = @Id";
            var para = new
            {
                Id = id,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<int>(query, para);

            ProjectStatusModel projectStatus = new();

            projectStatus.Type = (ProjectStatus)result.First();
            projectStatus.Name = projectStatus.Type.GetDisplayName();

            return projectStatus;
        }
        public async Task<Guid> GetProjectStatusId(ProjectStatusModel status)
        {

            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[ProjectStatus] Where Type = @Type";
            var para = new
            {
                Type = status.Type,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            return result.First();
        }
        #endregion
        #region Categories
        public async Task<Guid> GetCompanyCategory0Id(Guid idCu,Guid idCo, Guid idPr)
        {

            var query = @$"SELECT [Id] 
                        FROM [{Context.dbName}].[dbo].[CompanyCategory0] 
                        WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId AND Id = @Id";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idPr,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return Guid.Empty;
            }
        }
        public async Task<Guid> GetCompanyCategory1Id(Guid idCu,Guid idCo, Guid idPr, Guid idCat0)
        {

            var query = @$"SELECT [Id] 
                        FROM [{Context.dbName}].[dbo].[CompanyCategory1] 
                        WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId AND Id = @Id AND CompanyCategory0 = @CompanyCategory0";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idPr,
                CompanyCategory0 = idCat0
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);


            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return Guid.Empty;
            }
        }
        public async Task<Guid> GetCompanyCategory2Id(Guid idCu,Guid idCo, Guid idPr, Guid idCat1)
        {

            var query = @$"SELECT [Id] 
                        FROM [{Context.dbName}].[dbo].[CompanyCategory2] 
                        WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId AND Id = @Id AND CompanyCategory1 = @CompanyCategory1";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idPr,
                CompanyCategory1 = idCat1
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            if (result.Any())
            {
                return result.First();
            } else
            {
                return Guid.Empty;
            }
            
        }
        public async Task<Guid> GetCompanyCategory3Id(Guid idCu,Guid idCo, Guid idPr, Guid idCat2)
        {

            var query = @$"SELECT [Id] 
                        FROM [{Context.dbName}].[dbo].[CompanyCategory3] 
                        WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId AND Id = @Id AND CompanyCategory2 = @CompanyCategory2";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idPr,
                CompanyCategory2 = idCat2
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return Guid.Empty;
            }
        }
        public async Task<List<ProjectCategoryModel>?> GetCategories(string idCu, string idCo)
        {
            List<ProjectCategoryModel> projectCategoryModels = new List<ProjectCategoryModel>();
            ProjectCategoryModel categoryModel = new ProjectCategoryModel();

            var query = $@"SELECT [Id]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory0]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);
            foreach (var id0 in result)
            {
                categoryModel = await GetCategory0(Guid.Parse(idCu), Guid.Parse(idCo), id0);
                projectCategoryModels.Add(categoryModel);
            }

            return projectCategoryModels;
        }
        public async Task<ProjectCategoryModel> GetCategory0(Guid idCu, Guid idCo, Guid idCa0)
        {
            ProjectCategoryModel categoryModel = new();

            var query = $@"SELECT [Name]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory0]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND Id = @Id";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                Id = idCa0
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);

            categoryModel.Id = idCa0;
            categoryModel.Name = result.First();
            categoryModel.Projects = await new SQLRead().GetProjectsGuidByCategory(idCu,idCo,0,idCa0);

            categoryModel.CategoriesLevel1 = await GetCategories1(idCu, idCo, idCa0);            
            return categoryModel;
        }
        public async Task<List<ProjectCategoryModel1>> GetCategories1(Guid idCu, Guid idCo, Guid idCa0)
        {
            List<ProjectCategoryModel1> projectsCategoryModel1 = [];
            ProjectCategoryModel1 projectCategoryModel1 = new();
            var query = $@"SELECT [Id]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory1]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory0 = @CompanyCategory0";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory0 = idCa0,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            foreach (var idCa1 in result)
            {
                projectCategoryModel1 = await GetCategory1(idCu, idCo, idCa0, idCa1);
                projectsCategoryModel1.Add(projectCategoryModel1);
            }
            return projectsCategoryModel1;
        }
        public async Task<ProjectCategoryModel1> GetCategory1(Guid idCu, Guid idCo, Guid idCa0, Guid idCa1)
        {
            ProjectCategoryModel1 categoryModel = new();

            var query = $@"SELECT [Name]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory1]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId 
                              AND [CompanyCategory0] = @CompanyCategory0
                              AND [Id] = @CompanyCategory1";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory0 = idCa0,
                CompanyCategory1 = idCa1
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);

            categoryModel.Id = idCa1;
            categoryModel.Name = result.First();
            categoryModel.Projects = await new SQLRead().GetProjectsGuidByCategory(idCu, idCo, 1, idCa1);

            categoryModel.CategoriesLevel2 = await GetCategories2(idCu, idCo, idCa1);
            return categoryModel;
        }
        public async Task<List<ProjectCategoryModel2>> GetCategories2(Guid idCu, Guid idCo, Guid idCa1)
        {
            List<ProjectCategoryModel2> projectsCategoryModel2 = [];
            ProjectCategoryModel2 projectCategoryModel2 = new();
            var query = $@"SELECT [Id]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory2]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId 
                        AND CompanyCategory1 = @CompanyCategory1";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory1 = idCa1,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            foreach (var idCa2 in result)
            {
                projectCategoryModel2 = await GetCategory2(idCu, idCo, idCa1, idCa2);
                projectsCategoryModel2.Add(projectCategoryModel2);
            }
            return projectsCategoryModel2;
        }
        public async Task<ProjectCategoryModel2> GetCategory2(Guid idCu, Guid idCo, Guid idCa1, Guid idCa2)
        {
            ProjectCategoryModel2 categoryModel = new();

            var query = $@"SELECT [Name]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory2]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId 
                              AND [CompanyCategory1] = @CompanyCategory1
                              AND [Id] = @CompanyCategory2";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory1 = idCa1,
                CompanyCategory2 = idCa2
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);

            categoryModel.Id = idCa2;
            categoryModel.Name = result.First();
            categoryModel.Projects = await new SQLRead().GetProjectsGuidByCategory(idCu, idCo, 2, idCa2);

            categoryModel.CategoriesLevel3 = await GetCategories3(idCu, idCo, idCa2);
            return categoryModel;
        }
        public async Task<List<ProjectCategoryModel3>> GetCategories3(Guid idCu, Guid idCo, Guid idCa2)
        {
            List<ProjectCategoryModel3> projectsCategoryModel3 = [];
            ProjectCategoryModel3 projectCategoryModel3 = new();
            var query = $@"SELECT [Id]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory3]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId 
                        AND CompanyCategory2 = @CompanyCategory2";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory2 = idCa2,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);

            foreach (var idCa3 in result)
            {
                projectCategoryModel3 = await GetCategory3(idCu, idCo, idCa2, idCa3);
                projectsCategoryModel3.Add(projectCategoryModel3);
            }
            return projectsCategoryModel3;
        }
        public async Task<ProjectCategoryModel3> GetCategory3(Guid idCu, Guid idCo, Guid idCa2, Guid idCa3)
        {
            ProjectCategoryModel3 categoryModel = new();

            var query = $@"SELECT [Name]
                        FROM [{Context.dbName}].[dbo].[CompanyCategory3]
                        WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId 
                              AND [CompanyCategory2] = @CompanyCategory2
                              AND [Id] = @CompanyCategory3";
            var para = new
            {
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory2 = idCa2,
                CompanyCategory3 = idCa3
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<string>(query, para);

            categoryModel.Id = idCa3;
            categoryModel.Name = result.First();
            categoryModel.Projects = await new SQLRead().GetProjectsGuidByCategory(idCu, idCo, 3, idCa3);
            return categoryModel;
        }

        #endregion
    }
}
