using Dapper;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Authorize;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Data
{
    public class UserOperations
    {

        public class AdminTypeSQL
        {
            public Guid Id;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public string? PhoneWork;
            public string? EmailWork;
            public string? Login;
            public string? Password;
            public Guid Status;
            public DateTime BirthDate;
            public Guid Rol;
        }

        public class CustomerTypeSQL
        {
            public Guid Id;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public Guid StatusLicenceId;
            public string? Phone;
            public string? Email;
            public string? Login;
            public string? Password;
            public Guid Status;
            public DateTime EndTimeLicense;
            public DateTime DateRegistration;
        }

        public class EmployeeTypeSQL
        {
            public Guid Id;
            public Guid CompanyId;
            public Guid CustomerId;
            public string? FirstName;
            public string? SecondName;
            public string? ThirdName;
            public string? PhoneWork;
            public string? PhoneSelf;
            public string? EmailWork;
            public string? EmailSelf;
            public string? TaxNumber;
            public string? AddressReg;
            public string? AddressFact;
            public string? Photo;
            public string? Login;
            public string? Password;
            public double Salary;
            public double AddSalary;
            public Guid Status;
            public DateTime BirthDate;
            public DateTime WorkStartDate;
            public DateTime WorkEndDate;
            public Guid Position;
            public Guid Rol;
        }

        public async Task<Guid> GetRoleAdminId(Guid id)
        {
            using (var database = Context.Connect)
            {
                var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<Guid>(query, para);

                return result.First();
            }
        }

        public async Task<Guid> GetRoleEmployeeId(Guid id)
        {
            using (var database = Context.Connect)
            {
                var query = "SELECT [Rol] FROM [ReportingSystem].[dbo].[Employee] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };
                var result = await database.QueryAsync<Guid>(query, para);

                return result.First();
            }
        }

        public async Task<EmployeeRolModel> GetRoleAdmin(Guid id)
        {
            using (var database = Context.Connect)
            {
                var queryRol = "SELECT [Type] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Id = @Id";
                var paraRol = new
                {
                    Id = id
                };
                var resultRol = await database.QueryAsync<int>(queryRol, paraRol);
                int typeRol = resultRol.First();
                EmployeeRolModel model = new EmployeeRolModel();
                model.rolType = (EmployeeRolStatus)typeRol;
                model.rolName = model.rolType.GetDisplayName();

                return model;
            }
        }

        public async Task<EmployeeRolModel> GetRoleEmployee(Guid id)
        {
            using (var database = Context.Connect)
            {
                var queryRol = "SELECT [Type] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Id = @Id";
                var paraRol = new
                {
                    Id = id,
                };
                var resultRol = await database.QueryAsync<int>(queryRol, paraRol);
                int typeRol = resultRol.First();
                EmployeeRolModel model = new EmployeeRolModel();
                model.rolType = (EmployeeRolStatus)typeRol;
                model.rolName = model.rolType.GetDisplayName();

                return model;
            }
        }

        public async Task<EmployeePositionModel> GetPositionEmployee(Guid id)
        {
            using (var database = Context.Connect)
            {
                var query = "SELECT [Type] FROM [ReportingSystem].[dbo].[EmployeeRolStatus] Where Id = @Id";
                var para = new
                {
                    Id = id,//idRol,
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
            using (var database = Context.Connect)
            {
                var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
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
        }

        public async Task<bool> IsPasswordEmployeeOk(Guid id, string inputPassword)
        {
            using (var database = Context.Connect)
            {
                var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Employee] Where Id = @Id";
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
        }

        public async Task<bool> IsPasswordCustomerOk(Guid id, string inputPassword)
        {
            using (var database = Context.Connect)
            {
                var query = "SELECT [Password] FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";
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
        }

        public async Task<CustomerLicenceStatusModel> GetLicenceStatus(Guid id)
        {
            CustomerLicenceStatusModel status = new CustomerLicenceStatusModel();
            using (var database = Context.Connect)
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

        public async Task<EmployeeStatusModel> GetEmployeeStatus(Guid id)
        {
            EmployeeStatusModel status = new EmployeeStatusModel();
            using (var database = Context.Connect)
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

        public async Task<EmployeeModel> GetAdminData(Guid id)
        {
            EmployeeModel employee = new EmployeeModel();
            using (var database = Context.Connect)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<AdminTypeSQL>(query, para);
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
                    employee.password = administrator.Password != null ? EncryptionHelper.Decrypt(administrator.Password) : "";
                    employee.status = await GetEmployeeStatus(administrator.Status); ;
                    employee.birthDate = administrator.BirthDate;
                    employee.rol = await GetRoleAdmin(administrator.Rol);
                };

                return employee;
            }
        }

        public async Task<EmployeeModel> GetCustomerData(Guid id)
        {
            EmployeeModel employee = new EmployeeModel();
            using (var database = Context.Connect)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<CustomerTypeSQL>(query, para);
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
                    employee.password = customer.Password != null ? EncryptionHelper.Decrypt(customer.Password) : "";
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
            using (var database = Context.Connect)
            {
                var query = "SELECT * FROM [ReportingSystem].[dbo].[Employee] Where Id = @Id";
                var para = new
                {
                    Id = id,
                };

                var results = await database.QueryAsync<EmployeeTypeSQL>(query, para);
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
                    employee.password = employees.Password != null ? EncryptionHelper.Decrypt(employees.Password) : "";
                    employee.salary = employees.Salary;
                    employee.addSalary = employees.AddSalary;
                    employee.status = await GetEmployeeStatus(employees.Status);

                    employee.birthDate = employees.BirthDate;
                    employee.workStartDate = employees.WorkStartDate;
                    employee.workEndDate = employees.WorkEndDate;
                    employee.rol = await GetRoleEmployee(employees.Rol);
                    employee.position = await GetPositionEmployee(employees.Position);
                };

                return employee;
            }
        }

        public async Task<EmployeeModel> GetEmployeeData(Guid idCu, Guid idCo, Guid idEm)
        {
            EmployeeModel employee = new EmployeeModel();
            if (!idCu.Equals(Guid.Empty) && !idCo.Equals(Guid.Empty))
            {
                return employee = await GetEmployeeData(idEm);
            }
            using (var database = Context.Connect)
            {

                var adminTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Administrators] Where Id = @Id";
                var customerTableQuery = "SELECT [Id] FROM [ReportingSystem].[dbo].[Customers] Where Id = @Id";

                var para = new
                {
                    Id = idEm,
                };
                var resultAdmin = await database.QueryAsync<Guid>(adminTableQuery, para);
                var resultCustomer = await database.QueryAsync<Guid>(customerTableQuery, para);
                
                Guid id;

                int count = 0;
                if (resultAdmin.Any())
                {
                    return employee = await GetAdminData(idEm);
                }
                if (resultCustomer.Any())
                {
                    return employee = await GetCustomerData(idEm);
                }
            }
            return employee;
        }
    }
}
