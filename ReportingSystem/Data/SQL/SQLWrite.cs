using Bogus;
using Bogus.DataSets;
using Dapper;
using Newtonsoft.Json;
using ReportingSystem.Data.JSON;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Hubs;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Migrations.Model;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static ReportingSystem.Data.SQL.TableTypeSQL;

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

        public async Task DeleteAdministrator(string id)
        {
            var query = "DELETE FROM [dbo].[Administrators] WHERE [CustomerId] = @idCu AND [CompanyId] = @idCo AND [Id] = @id";
            var para = new
            {
                Id = id,
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteEmployee(string[] ar)
        {
            var query = "DELETE FROM [dbo].[Employees] WHERE [CustomerId] = @idCu AND [CompanyId] = @idCo AND [Id] = @id";
            var para = new
            {
                idCu = ar[0],
                idCo = ar[1],
                Id = ar[2],
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task FromArchiveAdministrator(string id)
        {
            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            var query = "UPDATE[dbo].[Administrators] SET[Status] = @Status WHERE [Id] = @Id";
            var para = new
            {
                Id = id,
                Status = statusId,
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task FromArchiveEmployee(string[] ar)
        {

            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            var query = "UPDATE[dbo].[Employees] SET[Status] = @Status WHERE[Id] = @Id AND[CompanyId] = @CompanyId AND[CustomerId] = @CustomerId";
            var para = new
            {
                Id = ar[2],
                CompanyId = ar[1],
                CustomerId = ar[0],
                Status = statusId,
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task CreateAdministrator(string[] ar)
        {
            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            Guid rolId = await new SQLRead().GetEmployeeRoleIdByName(ar[5]);

            var query = "INSERT INTO[dbo].[Administrators]([Id], [CompanyId], [CustomerId], [FirstName], [SecondName], [ThirdName], [PhoneWork], [EmailWork], [Login], [Password], [Status], [BirthDate], [WorkStartDate], [Rol]) " +
                        "VALUES(@Id, @CompanyId, @CustomerId, @FirstName, @SecondName, @ThirdName, @PhoneWork, @EmailWork, @Login, @Password, @Status, @BirthDate, @WorkStartDate, @Rol)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.Empty,
                CustomerId = Guid.Empty,
                FirstName = ar[0],
                SecondName = ar[1],
                ThirdName = ar[2],
                PhoneWork = ar[8],
                EmailWork = ar[9],
                Login = ar[4],
                Password = ar[7],
                Status = statusId,
                BirthDate = DateTime.Parse(ar[3]),
                WorkStartDate = DateTime.Parse(ar[6]),
                Rol = rolId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task CreateEmployee(string[] ar)
        {
            if (ar.Length != 21 || !Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo))
            {
                return;
            }

            Guid positionId = await new SQLRead().GetPositionIdByName(ar[7], idCu, idCo);
            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            Guid rolId = await new SQLRead().GetEmployeeRoleIdByName(ar[9]);

            var query = "INSERT INTO[dbo].[Employees]([Id], [CompanyId], [CustomerId], [FirstName], [SecondName], [ThirdName], [PhoneWork], [PhoneSelf], [EmailWork], [EmailSelf], [TaxNumber], [AddressReg], [AddressFact], [Login], [Password], [Salary], [AddSalary], [Status], [BirthDate], [WorkStartDate], [Position], [Rol]) " +
                        "VALUES(@Id, @CompanyId, @CustomerId, @FirstName, @SecondName, @ThirdName, @PhoneWork, @PhoneSelf, @EmailWork, @EmailSelf, @TaxNumber, @AddressReg, @AddressFact, @Login, @Password, @Salary, @AddSalary, @Status, @BirthDate, @WorkStartDate, @Position, @Rol)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CompanyId = idCo,
                CustomerId = idCu,
                FirstName = ar[2],
                SecondName = ar[3],
                ThirdName = ar[4],
                PhoneWork = ar[12],
                PhoneSelf = ar[13],
                EmailWork = ar[14],
                EmailSelf = ar[15],
                TaxNumber = ar[19],
                AddressReg = ar[16],
                AddressFact = ar[17],
                //Photo = ,
                Login = ar[8],
                Password = ar[11],
                Salary = double.TryParse(ar[18], out double parsedSalary) ? parsedSalary : 0.0,
                AddSalary = double.TryParse(ar[20], out double parsedAddSalary) ? parsedAddSalary : 0.0,
                Status = statusId,
                BirthDate = DateTime.Parse(ar[5]),
                WorkStartDate = DateTime.Parse(ar[6]),
                //WorkEndDate = ,
                Position = positionId,
                Rol = rolId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task ArchiveEmployee(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo) || !Guid.TryParse(ar[2], out Guid idEm))
            {
                return;
            }

            var statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Archive);

            var query = "UPDATE [dbo].[Employees] SET [Status] = @status WHERE [Id] = @id AND [CustomerId] = @customerId AND [CompanyId] = @companyId";
            var para = new
            {
                id = idEm,
                customerId = idCu,
                companyId = idCo,
                status = statusId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task ArchiveAdministrator(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idEm))
            {
                return;
            }

            var statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Archive);

            var query = "UPDATE [dbo].[Administrators] SET [Status] = @status WHERE [Id] = @id";
            var para = new
            {
                id = idEm,
                status = statusId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task EditEmployee(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return;
                }

                Guid idCu = editedEmployee.customerId;
                Guid idCo = editedEmployee.companyId;
                Guid idEm = editedEmployee.id;

                EmployeeModel employee = await new SQLRead().GetEmployee(idCu, idCo, idEm);

                if (employee.status == null || employee.position == null || employee.rol == null)
                {
                    return;
                }
                Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(employee.status);
                Guid positionId = await new SQLRead().GetPositionIdByType(employee.position, employee.customerId, employee.companyId);
                Guid rolId = await new SQLRead().GetRolIdByType(employee.rol);

                StringBuilder sqlBuilder = new();
                sqlBuilder.Append("UPDATE [dbo].[Employees] SET ");

                sqlBuilder.Append(!string.IsNullOrEmpty(employee.firstName) ? "[FirstName] = @firstName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.secondName) ? "[SecondName] = @secondName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.thirdName) ? "[ThirdName] = @thirdName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.phoneWork) ? "[PhoneWork] = @phoneWork, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.phoneSelf) ? "[PhoneSelf] = @phoneSelf, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.emailWork) ? "[EmailWork] = @emailWork, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.emailSelf) ? "[EmailSelf] = @emailSelf, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.taxNumber) ? "[TaxNumber] = @taxNumber, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.addressReg) ? "[AddressReg] = @addressReg, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.addressFact) ? "[AddressFact] = @addressFact, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.photo) ? "[Photo] = @photo, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.login) ? "[Login] = @login, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.password) ? "[Password] = @password, " : "");
                sqlBuilder.Append(employee?.salary != null ? "[Salary] = @salary, " : "");
                sqlBuilder.Append(employee?.addSalary != null ? "[AddSalary] = @addSalary, " : "");
                sqlBuilder.Append(employee?.status != null ? "[Status] = @status, " : "");
                sqlBuilder.Append(employee?.birthDate != null ? "[BirthDate] = @birthDate, " : "");
                sqlBuilder.Append(employee?.workStartDate != null ? "[WorkStartDate] = @workStartDate, " : "");
                sqlBuilder.Append(employee?.workEndDate != null ? "[WorkEndDate] = @workEndDate, " : "");
                sqlBuilder.Append(employee?.position != null ? "[Position] = @position, " : "");
                sqlBuilder.Append(employee?.rol != null ? "[Rol] = @rol, " : "");

                sqlBuilder.Length -= 2;

                sqlBuilder.Append(" WHERE [Id] = @Id AND [CompanyId] = @CompanyId AND [CustomerId] = @CustomerId");

                string query = sqlBuilder.ToString();

                var para = new
                {
                    FirstName = employee?.firstName,
                    SecondName = employee?.secondName,
                    ThirdName = employee?.thirdName,
                    PhoneWork = employee?.phoneWork,
                    PhoneSelf = employee?.phoneSelf,
                    EmailWork = employee?.emailWork,
                    EmailSelf = employee?.emailSelf,
                    TaxNumber = employee?.taxNumber,
                    AddressReg = employee?.addressFact,
                    AddressFact = employee?.addressFact,
                    Photo = employee?.photo,
                    Login = employee?.login,
                    Password = employee?.password,
                    Salary = employee?.salary,
                    AddSalary = employee?.addSalary,
                    Status = statusId,
                    BirthDate = employee?.birthDate,
                    WorkStartDate = employee?.workStartDate,
                    WorkEndDate = employee?.workEndDate,
                    Position = positionId,
                    Rol = rolId
                };
                using var database = Context.ConnectToSQL;
                await database.QueryAsync(query, para);
            }
        }

        public async Task EditAdministrator(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return;
                }

                Guid idCu = editedEmployee.customerId;
                Guid idCo = editedEmployee.companyId;
                Guid idEm = editedEmployee.id;

                EmployeeModel employee = await new SQLRead().GetEmployee(idCu, idCo, idEm);

                if (employee.status == null || employee.position == null || employee.rol == null)
                {
                    return;
                }
                Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(employee.status);
                Guid positionId = await new SQLRead().GetPositionIdByType(employee.position, employee.customerId, employee.companyId);
                Guid rolId = await new SQLRead().GetRolIdByType(employee.rol);

                StringBuilder sqlBuilder = new();
                sqlBuilder.Append("UPDATE [dbo].[Administrators] SET ");

                sqlBuilder.Append(!string.IsNullOrEmpty(employee.firstName) ? "[FirstName] = @firstName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.secondName) ? "[SecondName] = @secondName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.thirdName) ? "[ThirdName] = @thirdName, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.phoneWork) ? "[PhoneWork] = @phoneWork, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.phoneSelf) ? "[PhoneSelf] = @phoneSelf, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.emailWork) ? "[EmailWork] = @emailWork, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.emailSelf) ? "[EmailSelf] = @emailSelf, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.taxNumber) ? "[TaxNumber] = @taxNumber, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.addressReg) ? "[AddressReg] = @addressReg, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.addressFact) ? "[AddressFact] = @addressFact, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.photo) ? "[Photo] = @photo, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.login) ? "[Login] = @login, " : "");
                sqlBuilder.Append(!string.IsNullOrEmpty(employee.password) ? "[Password] = @password, " : "");
                sqlBuilder.Append(employee?.salary != null ? "[Salary] = @salary, " : "");
                sqlBuilder.Append(employee?.addSalary != null ? "[AddSalary] = @addSalary, " : "");
                sqlBuilder.Append(employee?.status != null ? "[Status] = @status, " : "");
                sqlBuilder.Append(employee?.birthDate != null ? "[BirthDate] = @birthDate, " : "");
                sqlBuilder.Append(employee?.workStartDate != null ? "[WorkStartDate] = @workStartDate, " : "");
                sqlBuilder.Append(employee?.workEndDate != null ? "[WorkEndDate] = @workEndDate, " : "");
                sqlBuilder.Append(employee?.position != null ? "[Position] = @position, " : "");
                sqlBuilder.Append(employee?.rol != null ? "[Rol] = @rol, " : "");

                sqlBuilder.Length -= 2;

                sqlBuilder.Append(" WHERE [Id] = @Id");

                string query = sqlBuilder.ToString();

                var para = new
                {
                    FirstName = employee?.firstName,
                    SecondName = employee?.secondName,
                    ThirdName = employee?.thirdName,
                    PhoneWork = employee?.phoneWork,
                    PhoneSelf = employee?.phoneSelf,
                    EmailWork = employee?.emailWork,
                    EmailSelf = employee?.emailSelf,
                    TaxNumber = employee?.taxNumber,
                    AddressReg = employee?.addressFact,
                    AddressFact = employee?.addressFact,
                    Photo = employee?.photo,
                    Login = employee?.login,
                    Password = employee?.password,
                    Salary = employee?.salary,
                    AddSalary = employee?.addSalary,
                    Status = statusId,
                    BirthDate = employee?.birthDate,
                    WorkStartDate = employee?.workStartDate,
                    WorkEndDate = employee?.workEndDate,
                    Position = positionId,
                    Rol = rolId
                };
                using var database = Context.ConnectToSQL;
                await database.QueryAsync(query, para);
            }
        }

        public async Task EditCustomer(string[] ar)
        {
            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid idCu))
            {
                return;
            }
            var query = "UPDATE [dbo].[Customers] SET [FirstName] = @FirstName, [SecondName] = @SecondName, [ThirdName] = @ThirdName, [Phone] = @Phone, [Email] = @Email, [Password] = @Password WHERE [Id] = @Id";
            var para = new
            {
                Id = idCu,
                FirstName = ar[1],
                SecondName = ar[2],
                ThirdName = ar[3],
                Phone = ar[4],
                Email = ar[5],
                Password = ar[6]
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task CancellationLicence(string[] ar)
        {
            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id))
            {
                return;
            }

            var query1 = "INSERT INTO[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
                         "VALUES(@Id, @CustomerId, @DateChange, @OldEndDateLicence, @NewEndDateLicence, @OldStatus, @NewStatus, @Price, @Period, @NameOperation)";
            var para1 = new
            {
                Id = Guid.NewGuid(),
                CustomerId = id,
                DateChange = DateTime.Now,
                OldEndDateLicence = await GetEndDateLicence(id),
                NewEndDateLicence = DateTime.Now.AddDays(1),
                OldStatus = await GetLicenceId(id),
                NewStatus = await GetLicenceIdByType(5),
                Price = 0.0,
                Period = "",
                NameOperation = "Анулювання"
            };
            var query2 = "UPDATE [dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId, [EndTimeLicense] = @EndTimeLicense WHERE [Id] = @Id";
            var para2 = new
            {
                StatusLicenceId = await GetLicenceIdByType(5),
                Id = id,
                EndTimeLicense = DateTime.Now.AddDays(1)
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query1, para1);
            await database.QueryAsync(query2, para2);
        }

        public async Task DeleteHistoryOperations(Guid idCu)
        {
            var query = "DELETE FROM [dbo].[HistoryOperations] WHERE CustomerId = @CustomerId";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteLicence(string[] ar)
        {
            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid idCu))
            {
                return;
            }

            await DeleteCompanies(idCu);
            await DeleteHistoryOperations(idCu);
            var query = "DELETE FROM[dbo].[Customers] WHERE[Id] = @Id";
            var para = new
            {
                Id = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }


        public async Task ArchivingLicence(string[] ar)
        {
            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id))
            {
                return;
            }

            var query1 = "INSERT INTO[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
                         "VALUES(@Id, @CustomerId, @DateChange, @OldEndDateLicence, @NewEndDateLicence, @OldStatus, @NewStatus, @Price, @Period, @NameOperation)";
            var para1 = new
            {
                Id = Guid.NewGuid(),
                CustomerId = id,
                DateChange = DateTime.Now,
                OldEndDateLicence = await GetEndDateLicence(id),
                NewEndDateLicence = await GetEndDateLicence(id),
                OldStatus = await GetLicenceId(id),
                NewStatus = await GetLicenceIdByType(4),
                Price = 0.0,
                Period = "",
                NameOperation = "Архівування"
            };
            var query2 = "UPDATE [dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId WHERE [Id] = @Id";
            var para2 = new
            {
                StatusLicenceId = await GetLicenceIdByType(4),
                Id = id
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query1, para1);
            await database.QueryAsync(query2, para2);
        }

        public async Task RenewalLicense(string[] ar)
        {
            if (ar.Length < 4 || !Guid.TryParse(ar[0], out Guid id) || !DateTime.TryParse(ar[1], out DateTime desiredDate))
            {
                return;
            }

            var query1 = "INSERT INTO[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
                         "VALUES(@Id, @CustomerId, @DateChange, @OldEndDateLicence, @NewEndDateLicence, @OldStatus, @NewStatus, @Price, @Period, @NameOperation)";
            var para1 = new
            {
                Id = Guid.NewGuid(),
                CustomerId = id,
                DateChange = DateTime.Now,
                OldEndDateLicence = await GetEndDateLicence(id),
                NewEndDateLicence = desiredDate,
                OldStatus = await GetLicenceId(id),
                NewStatus = await GetLicenceIdByType(2),
                Price = double.TryParse(ar[2].Trim(), out double parsedPrice) ? parsedPrice : 0.0,
                Period = ar[3].Trim(),
                NameOperation = "Продовження"
            };
            var query2 = "UPDATE [dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId,[EndTimeLicense] = @EndTimeLicense WHERE [Id] = @Id";
            var para2 = new
            {
                StatusLicenceId = await GetLicenceIdByType(2),
                EndTimeLicense = desiredDate,
                Id = id
            };
            using (var database = Context.ConnectToSQL)
            {
                await database.QueryAsync(query1, para1);
                
            }

            using (var database = Context.ConnectToSQL)
            {
                await database.QueryAsync(query2, para2);
            }
        }

        public async Task<Guid> GetLicenceIdByType(int type)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT [Id] FROM [ReportingSystem].[dbo].[StatusLicence] Where Type = @type";
            var para = new
            {
                Type = type,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.FirstOrDefault();

        }

        public async Task<Guid> GetLicenceId(Guid idCu)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[StatusLicenceId] FROM[ReportingSystem].[dbo].[Customers] WHERE Id = @Id";
            var para = new
            {
                Id = idCu,
            };
            var result = await database.QueryAsync<Guid>(query, para);
            return result.FirstOrDefault();
        }

        public async Task<DateTime> GetEndDateLicence(Guid idCu)
        {
            using var database = Context.ConnectToSQL;
            var query = "SELECT[EndTimeLicense] FROM[ReportingSystem].[dbo].[Customers] WHERE Id = @Id";
            var para = new
            {
                Id = idCu,
            };
            var result = await database.QueryFirstOrDefaultAsync<DateTime?>(query, para);
            return result ?? DateTime.MinValue;
        }


        public async Task InsertCustomerHistory(Guid customerId, DateTime oldEndDateLicence, DateTime newEndDateLicence, Guid oldStatus, Guid newStatus, double price, string period, string nameOperation)
        {
            using var database = Context.ConnectToSQL;
            var query = "INSERT INTO [dbo].[HistoryOperations] ([Id],[CustomerId],[DateChange],[OldEndDateLicence],[NewEndDateLicence],[OldStatus],[NewStatus],[Price],[Period],[NameOperation])" +
                        "VALUES (@id,@CustomerId,@DateChange,@OldEndDateLicence,@NewEndDateLicence,@OldStatus,@NewStatus,@Price,@Period,@NameOperation);";
            var para = new
            {
                id = Guid.NewGuid(),
                CustomerId = customerId,
                DateChange = DateTime.Today,
                OldEndDateLicence = oldEndDateLicence,
                NewEndDateLicence = newEndDateLicence,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                Price = price,
                Period = period,
                NameOperation = nameOperation
            };
            await database.QueryAsync<Guid>(query, para);
        }

        public async Task RegistrationCustomer(string[] ar)
        {
            using var database = Context.ConnectToSQL;
            var query = "INSERT INTO[dbo].[Customers]([Id], [FirstName], [SecondName], [ThirdName], [StatusLicenceId], [ConfigureId], [Phone], [Email], [Password], [EndTimeLicense], [DateRegistration])" +
                        "VALUES(@id, @firstName, @secondName, @thirdName, @statusLicenceId, @phone, @email, @password, @endTimeLicense, @dateRegistration)";
            var para = new
            {
                id = Guid.NewGuid(),
                firstName = ar[1],
                secondName = ar[2],
                thirdName = ar[3],
                statusLicenceId = GetLicenceIdByType(4),
                phone = ar[4],
                email = ar[0],
                password = EncryptionHelper.Encrypt(ar[5]),
                endTimeLicense = DateTime.Today.AddDays(30),
                dateRegistration = DateTime.Today
            };
            await database.QueryAsync(query, para);

            await InsertCustomerHistory(para.id,
                para.dateRegistration, para.endTimeLicense,
                await GetLicenceIdByType(0), await GetLicenceIdByType(4),
                0.0, "30d", "Реєстрація замовника");
        }

        public async Task EditCompany(string[] ar)
        {
            using var database = Context.ConnectToSQL;
            var query = "UPDATE [dbo].[Companies] SET [Name] = @Name, [Address] = @Address, [Actions] = @Actions, [Phone] = @Phone, [Email] = @Email WHERE Id = @Id AND CustomerId = @CustomerId";
            var para = new
            {
                Id = ar[0],
                CustomerId = ar[6],
                Name = ar[1],
                Address = ar[2],
                Actions = ar[3],
                Phone = ar[4],
                Email = ar[5]
            };
            await database.QueryAsync(query, para);
        }
        public async Task ArchiveCompany(string[] ar)
        {
            CompanyStatusModel companyStatusModel = new()
            {
                companyStatusType = CompanyStatus.Archive
            };
            companyStatusModel.companyStatusName = companyStatusModel.companyStatusType.GetDisplayName();
            var companyStatusId = await new SQLRead().GetCompanyStatusId(companyStatusModel);

            using var database = Context.ConnectToSQL;
            var query = "UPDATE [dbo].[Companies] SET [Status] = @Status WHERE Id = @Id AND CustomerId = @CustomerId";
            var para = new
            {
                Id = ar[0],
                CustomerId = ar[1],
                Status = companyStatusId
            };
            await database.QueryAsync(query, para);
        }

        public async Task DeleteEmployeePosition(Guid idCo, Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[EmployeePosition] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
            var para = new
            {
                CompanyId = idCo,
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteEmployeePosition(Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[EmployeePosition] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteEmployees(Guid idCo, Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[Employees] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
            var para = new
            {
                CompanyId = idCo,
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteEmployees(Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[Employees] WHERE CustomerId = @CustomerId";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteCompanyRolls(Guid idCo, Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[CompanyRolls] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
            var para = new
            {
                CompanyId = idCo,
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteCompanyRolls(Guid idCu)
        {   
            var query = "DELETE FROM [dbo].[CompanyRolls] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }


        public async Task DeleteCompany(string[] ar)
        {
          
            if (ar.Length < 2 || !Guid.TryParse(ar[0], out Guid idCo) || !Guid.TryParse(ar[1], out Guid idCu))
            {
                return;
            }

            await DeleteEmployeePosition(idCo, idCu);
            await DeleteCompanyRolls(idCo, idCu);
            await DeleteEmployees(idCo, idCu);


            var query = "DELETE FROM [dbo].[Companies] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
            var para = new
            {
                CompanyId = idCo,
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteCompanies(Guid idCu)
        {
         
            await DeleteEmployeePosition(idCu);
            await DeleteCompanyRolls(idCu);
            await DeleteEmployees(idCu);


            var query = "DELETE FROM [dbo].[Companies] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        //public async Task DeleteEmployee(string[] ar)
        //{
        //    if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo))
        //    {
        //        return;
        //    }

        //    await DeleteEmployeePosition(idCu);
        //    await DeleteCompanyRolls(idCu);
        //    await DeleteEmployees(idCu);


        //    var query = "DELETE FROM [dbo].[Companies] WHERE CustomerId = @CustomerId";
        //    var para = new
        //    {
        //        CustomerId = idCu
        //    };
        //    using var database = Context.ConnectToSQL;
        //    await database.QueryAsync(query, para);
        //}



        public async Task CreateCompany(string[] ar)
        {
            if (!Guid.TryParse(ar[6], out Guid idCu))
            {
                return;
            }

            Guid IdCo = Guid.NewGuid();
            Guid statusId = await new SQLRead().GetCompanyStatusId(CompanyStatus.Actual);

            //допрацювати механізм додавання директора для порожньої компанії

            var query = "INSERT FROM [dbo].[Companies] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                Id = IdCo,
                CustomerId = idCu,
                Name = ar[0],
                Address = ar[2],
                Code = ar[1],
                Actions = ar[3],
                StatusWeb = ar[7],
                Phone = ar[4],
                Email = ar[5],
                StatutCapital = ar[8],
                RegistrationDate = DateTime.Today,
                Status = statusId,
                Chief = ""
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
          
        }
    }
}
