using Bogus.DataSets;
using Dapper;
using Newtonsoft.Json;
using ReportingSystem.Data.JSON;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
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

        #region Administrators
        public async Task DeleteAdministrator(string id)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Administrators] WHERE [CustomerId] = @idCu AND [CompanyId] = @idCo AND [Id] = @id";
            var para = new
            {
                Id = id,
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task FromArchiveAdministrator(string id)
        {
            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            var query = $"UPDATE [{Context.dbName}].[dbo].[Administrators] SET[Status] = @Status WHERE [Id] = @Id";
            var para = new
            {
                Id = id,
                Status = statusId,
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateAdministrator(string[] ar)
        {
            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            Guid rolId = await new SQLRead().GetEmployeeRoleIdByName(ar[5]);

            var query = $"INSERT INTO [{Context.dbName}].[dbo].[Administrators]([Id], [CompanyId], [CustomerId], [FirstName], [SecondName], [ThirdName], [PhoneWork], [EmailWork], [Login], [Password], [Status], [BirthDate], [WorkStartDate], [Rol]) " +
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
        public async Task ArchiveAdministrator(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idEm))
            {
                return;
            }

            var statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Archive);

            var query = $"UPDATE [{Context.dbName}].[dbo].[Administrators] SET [Status] = @status WHERE [Id] = @id";
            var para = new
            {
                id = idEm,
                status = statusId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
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

                Guid idCu = editedEmployee.CustomerId;
                Guid idCo = editedEmployee.CompanyId;
                Guid idEm = editedEmployee.Id;

                EmployeeModel employee = await new SQLRead().GetEmployee(idCu, idCo, idEm);

                if (employee.Status == null || employee.Rol == null)
                {
                    return;
                }
                Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(employee.Status);
                Guid rolId = await new SQLRead().GetRolIdByType(employee.Rol);

                StringBuilder sqlBuilder = new();
                sqlBuilder.Append($"UPDATE [{Context.dbName}].[dbo].[Administrators] SET ");

                sqlBuilder.Append(UpdateField("FirstName", editedEmployee.FirstName, employee.FirstName));
                sqlBuilder.Append(UpdateField("SecondName", editedEmployee.SecondName, employee.SecondName));
                sqlBuilder.Append(UpdateField("ThirdName", editedEmployee.ThirdName, employee.ThirdName));
                sqlBuilder.Append(UpdateField("PhoneWork", editedEmployee.PhoneWork, employee.PhoneWork));
                sqlBuilder.Append(UpdateField("EmailWork", editedEmployee.EmailWork, employee.EmailWork));
                sqlBuilder.Append(UpdateField("TaxNumber", editedEmployee.TaxNumber, employee.TaxNumber));
                sqlBuilder.Append(UpdateField("AddressReg", editedEmployee.AddressReg, employee.AddressReg));
                sqlBuilder.Append(UpdateField("AddressFact", editedEmployee.AddressFact, employee.AddressFact));
                sqlBuilder.Append(UpdateField("Photo", editedEmployee.Photo, employee.Photo));
                sqlBuilder.Append(UpdateField("Login", editedEmployee.Login, employee.Login));
                sqlBuilder.Append(UpdateField("Password", editedEmployee.Password, employee.Password));
                sqlBuilder.Append(UpdateField("Status", editedEmployee?.Status?.EmployeeStatusType, employee?.Status?.EmployeeStatusType));
                sqlBuilder.Append(UpdateField("BirthDate", editedEmployee?.BirthDate, employee?.BirthDate));
                sqlBuilder.Append(UpdateField("Rol", editedEmployee?.Rol?.RolName, employee?.Rol?.RolName));

                if (editedEmployee != null)
                {
                    employee = editedEmployee;
                }

                sqlBuilder.Length -= 2;

                sqlBuilder.Append(" WHERE [Id] = @Id");

                string query = sqlBuilder.ToString();

                var para = new
                {
                    Id = idEm,
                    FirstName = employee?.FirstName,
                    SecondName = employee?.SecondName,
                    ThirdName = employee?.ThirdName,
                    PhoneWork = employee?.PhoneWork,
                    EmailWork = employee?.EmailWork,
                    TaxNumber = employee?.TaxNumber,
                    AddressReg = employee?.AddressFact,
                    AddressFact = employee?.AddressFact,
                    Photo = employee?.Photo,
                    Login = employee?.Login,
                    Password = employee?.Password,
                    Status = statusId,
                    BirthDate = employee?.BirthDate,
                    Rol = rolId
                };
                using var database = Context.ConnectToSQL;
                try
                {
                    await database.QueryAsync(query, para);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
        #endregion
        #region Positions
        public async Task DeleteEmployeePosition(Guid idCo, Guid idCu)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[EmployeePosition] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
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
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[EmployeePosition] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateEmployeePosition(Guid idCu, Guid idCo, string name)
        {
            int countPosition = await new SQLRead().GetEmployeePositionCount(idCu, idCo);
            countPosition++;

            var query = $"INSERT INTO [{Context.dbName}].[dbo].[EmployeePosition]([Id], [CustomerId], [CompanyId], [Type], [Name]) VALUES(@Id, @CustomerId, @CompanyId, @Type, @Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                Type = countPosition,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreatePosition(string[] ar) 
        {
            var idCu = Guid.Parse(ar[0]);
            var idCo = Guid.Parse(ar[1]); 

            int countPos = await new SQLRead().GetEmployeePositionCount(idCu, idCo);
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[EmployeePosition] ([Id], [CustomerId], [CompanyId], [Type], [Name]) VALUES (@Id, @CustomerId, @CompanyId, @Type, @Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                Type = countPos++,
                Name = ar[2]
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditPosition(string[] ar)
        {
            var query = $"UPDATE [{Context.dbName}].[dbo].[EmployeePosition] SET [Name] = @NewName WHERE [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND [Name] = @OldName";
            var para = new
            {
                CustomerId = ar[0],
                CompanyId = ar[1],
                NewName = ar[3],
                OldName = ar[2]
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task DeletePosition(string[] ar)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[EmployeePosition] WHERE CustomerId = @CustomerId AND CompanyId = CompanyId AND Name = @Name";
            var para = new
            {
                CustomerId = ar[0],
                CompanyId = ar[1], 
                Name = ar[2]
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        #endregion
        #region Rolls
        public async Task DeleteCompanyRolls(Guid idCo, Guid idCu)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[CompanyRolls] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
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
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[CompanyRolls] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        #endregion
        #region Customers
        public async Task EditCustomer(string[] ar)
        {
            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid idCu))
            {
                return;
            }
            var query = $"UPDATE [{Context.dbName}].[dbo].[Customers] SET [FirstName] = @FirstName, [SecondName] = @SecondName, [ThirdName] = @ThirdName, [Phone] = @Phone, [Email] = @Email, [Password] = @Password WHERE [Id] = @Id";
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

            var query1 = $"INSERT INTO [{Context.dbName}].[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
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
            var query2 = $"UPDATE [{Context.dbName}].[dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId, [EndTimeLicense] = @EndTimeLicense WHERE [Id] = @Id";
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
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[HistoryOperations] WHERE CustomerId = @CustomerId";
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
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Customers] WHERE[Id] = @Id";
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

            var query1 = $"INSERT INTO [{Context.dbName}].[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
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
            var query2 = $"UPDATE [{Context.dbName}].[dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId WHERE [Id] = @Id";
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

            var query1 = $"INSERT INTO [{Context.dbName}].[dbo].[HistoryOperations]([Id], [CustomerId], [DateChange], [OldEndDateLicence], [NewEndDateLicence], [OldStatus], [NewStatus], [Price], [Period], [NameOperation])" +
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
            var query2 = $"UPDATE [{Context.dbName}].[dbo].[Customers] SET [StatusLicenceId] = @StatusLicenceId,[EndTimeLicense] = @EndTimeLicense WHERE [Id] = @Id";
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
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[StatusLicence] Where Type = @type";
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
            var query = $"SELECT [StatusLicenceId] FROM[{Context.dbName}].[dbo].[Customers] WHERE Id = @Id";
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
            var query = $"SELECT [EndTimeLicense] FROM [{Context.dbName}].[dbo].[Customers] WHERE Id = @Id";
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
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[HistoryOperations] ([Id],[CustomerId],[DateChange],[OldEndDateLicence],[NewEndDateLicence],[OldStatus],[NewStatus],[Price],[Period],[NameOperation])" +
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
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[Customers] ([Id], [FirstName], [SecondName], [ThirdName], [StatusLicenceId], [ConfigureId], [Phone], [Email], [Password], [EndTimeLicense], [DateRegistration])" +
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
        #endregion
        #region Authorization
        #endregion
        #region Employees
        public async Task FromArchiveEmployee(string[] ar)
        {

            Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(EmployeeStatus.Actual);
            var query = $"UPDATE [{Context.dbName}].[dbo].[Employees] SET [Status] = @Status WHERE [Id] = @Id AND [CompanyId] = @CompanyId AND [CustomerId] = @CustomerId";
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
        public async Task DeleteEmployee(string[] ar)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Employees] WHERE [CustomerId] = @idCu AND [CompanyId] = @idCo AND [Id] = @id";
            var para = new
            {
                idCu = ar[0],
                idCo = ar[1],
                Id = ar[2],
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

            var query = $"INSERT INTO [{Context.dbName}].[dbo].[Employees]([Id], [CompanyId], [CustomerId], [FirstName], [SecondName], [ThirdName], [PhoneWork], [PhoneSelf], [EmailWork], [EmailSelf], [TaxNumber], [AddressReg], [AddressFact], [Login], [Password], [Salary], [AddSalary], [Status], [BirthDate], [WorkStartDate], [Position], [Rol]) " +
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

            var query = $"UPDATE [{Context.dbName}].[dbo].[Employees] SET [Status] = @status WHERE [Id] = @id AND [CustomerId] = @customerId AND [CompanyId] = @companyId";
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

                Guid idCu = editedEmployee.CustomerId;
                Guid idCo = editedEmployee.CompanyId;
                Guid idEm = editedEmployee.Id;

                EmployeeModel employee = await new SQLRead().GetEmployee(idCu, idCo, idEm);

                if (employee.Status == null || employee.Position == null || employee.Rol == null)
                {
                    return;
                }
                Guid statusId = await new SQLRead().GetEmployeeStatusIdByType(employee.Status);
                Guid positionId = await new SQLRead().GetPositionIdByType(employee.Position, employee.CustomerId, employee.CompanyId);
                Guid rolId = await new SQLRead().GetRolIdByType(employee.Rol);


                StringBuilder sqlBuilder = new();
                sqlBuilder.Append($"UPDATE [{Context.dbName}].[dbo].[Employees] SET ");

                sqlBuilder.Append(UpdateField("FirstName", editedEmployee.FirstName, employee.FirstName));
                sqlBuilder.Append(UpdateField("SecondName", editedEmployee.SecondName, employee.SecondName));
                sqlBuilder.Append(UpdateField("ThirdName", editedEmployee.ThirdName, employee.ThirdName));
                sqlBuilder.Append(UpdateField("PhoneWork", editedEmployee.PhoneWork, employee.PhoneWork));
                sqlBuilder.Append(UpdateField("PhoneSelf", editedEmployee.PhoneSelf, employee.PhoneSelf));
                sqlBuilder.Append(UpdateField("EmailWork", editedEmployee.EmailWork, employee.EmailWork));
                sqlBuilder.Append(UpdateField("EmailSelf", editedEmployee.EmailSelf, employee.EmailSelf));
                sqlBuilder.Append(UpdateField("TaxNumber", editedEmployee.TaxNumber, employee.TaxNumber));
                sqlBuilder.Append(UpdateField("AddressReg", editedEmployee.AddressReg, employee.AddressReg));
                sqlBuilder.Append(UpdateField("AddressFact", editedEmployee.AddressFact, employee.AddressFact));
                sqlBuilder.Append(UpdateField("Photo", editedEmployee.Photo, employee.Photo));
                sqlBuilder.Append(UpdateField("Login", editedEmployee.Login, employee.Login));
                sqlBuilder.Append(UpdateField("Password", editedEmployee.Password, employee.Password));
                sqlBuilder.Append(UpdateField("Salary", editedEmployee?.Salary, employee?.Salary));
                sqlBuilder.Append(UpdateField("AddSalary", editedEmployee?.AddSalary, employee?.AddSalary));
                sqlBuilder.Append(UpdateField("Status", editedEmployee?.Status?.EmployeeStatusType, employee?.Status.EmployeeStatusType));
                sqlBuilder.Append(UpdateField("BirthDate", editedEmployee?.BirthDate, employee?.BirthDate));
                sqlBuilder.Append(UpdateField("WorkStartDate", editedEmployee?.WorkStartDate, employee?.WorkStartDate));
                sqlBuilder.Append(UpdateField("WorkEndDate", editedEmployee?.WorkEndDate, employee?.WorkEndDate));
                sqlBuilder.Append(UpdateField("Position", editedEmployee?.Position?.NamePosition, employee?.Position.NamePosition));
                sqlBuilder.Append(UpdateField("Rol", editedEmployee?.Rol?.RolName, employee?.Rol.RolName));


                if (editedEmployee != null)
                {
                    employee = editedEmployee;
                }

                sqlBuilder.Length -= 2;

                sqlBuilder.Append(" WHERE [Id] = @Id AND [CompanyId] = @CompanyId AND [CustomerId] = @CustomerId");

                string query = sqlBuilder.ToString();
                var para = new
                {
                    Id = idEm,
                    CompanyId = idCo,
                    CustomerId = idCu,
                    FirstName = employee?.FirstName,
                    SecondName = employee?.SecondName,
                    ThirdName = employee?.ThirdName,
                    PhoneWork = employee?.PhoneWork,
                    PhoneSelf = employee?.PhoneSelf,
                    EmailWork = employee?.EmailWork,
                    EmailSelf = employee?.EmailSelf,
                    TaxNumber = employee?.TaxNumber,
                    AddressReg = employee?.AddressFact,
                    AddressFact = employee?.AddressFact,
                    Photo = employee?.Photo,
                    Login = employee?.Login,
                    Password = employee?.Password,
                    Salary = employee?.Salary,
                    AddSalary = employee?.AddSalary,
                    Status = statusId,
                    BirthDate = employee?.BirthDate,
                    WorkStartDate = employee?.WorkStartDate,
                    WorkEndDate = employee?.WorkEndDate,
                    Position = positionId,
                    Rol = rolId
                };
                using var database = Context.ConnectToSQL;
                try
                {
                    await database.QueryAsync(query, para);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
        public async Task DeleteEmployees(Guid idCo, Guid idCu)
        {
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Employees] WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId ";
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
            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Employees] WHERE CustomerId = @CustomerId";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditEmployeeRol(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo) || !Guid.TryParse(ar[2], out Guid idEm))
            {
                return;
            }
            var rolId = await new SQLRead().GetEmployeeRoleIdByName(ar[3]);
            var query = $"UPDATE [{Context.dbName}].[dbo].[Employees] SET [Rol] = @Rol WHERE [Id] = @EmployeeId AND [CompanyId] = @CompanyId AND [CustomerId] = @CustomerId";
            var para = new
            {
                CustomerId = ar[0],
                CompanyId = ar[1],
                EmployeeId = ar[2],
                Rol = rolId
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);

        }
        public async Task EditEmployeePosition(string[] ar)
        {
            var positionId = await new SQLRead().GetPositionIdByName(ar[3], Guid.Parse(ar[0]), Guid.Parse(ar[1]));

            var query = $"UPDATE [{Context.dbName}].[dbo].[Employees] SET [Position] = @Position WHERE [Id] = @EmployeeId AND [CompanyId] = @CompanyId AND [CustomerId] = @CustomerId";
            var para = new
            {
                CustomerId = ar[0],
                CompanyId = ar[1],
                EmployeeId = ar[2],
                Position = positionId

            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        #endregion
        #region Companies
        public async Task EditCompany(string[] ar)
        {
            using var database = Context.ConnectToSQL;
            var query = $"UPDATE [{Context.dbName}].[dbo].[Companies] SET [Name] = @Name, [Address] = @Address, [Actions] = @Actions, [Phone] = @Phone, [Email] = @Email WHERE Id = @Id AND CustomerId = @CustomerId";
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
                CompanyStatusType = CompanyStatus.Archive
            };
            companyStatusModel.CompanyStatusName = companyStatusModel.CompanyStatusType.GetDisplayName();
            var companyStatusId = await new SQLRead().GetCompanyStatusId(companyStatusModel);

            using var database = Context.ConnectToSQL;
            var query = $"UPDATE [{Context.dbName}].[dbo].[Companies] SET [Status] = @Status WHERE Id = @Id AND CustomerId = @CustomerId";
            var para = new
            {
                Id = ar[0],
                CustomerId = ar[1],
                Status = companyStatusId
            };
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


            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Companies] WHERE Id = @CompanyId AND CustomerId = @CustomerId ";
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


            var query = $"DELETE FROM [{Context.dbName}].[dbo].[Companies] WHERE CustomerId = @CustomerId ";
            var para = new
            {
                CustomerId = idCu
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateCompany(string[] ar)
        {
            if (!Guid.TryParse(ar[6], out Guid idCu))
            {
                return;
            }

            Guid idCo = Guid.NewGuid();
            Guid statusId = await new SQLRead().GetCompanyStatusId(CompanyStatus.Actual);

            string vId = Guid.NewGuid().ToString();
            string vIdCo = idCo.ToString();
            string vIdCu = idCu.ToString();


            string[] arEm = [vId, vIdCo, vIdCu,];

            var query = $"INSERT INTO [{Context.dbName}].[dbo].[Companies] ([Id], [CustomerId], [Name], [Address], [Code], [Actions], [StatusWeb], [Phone], [Email], [StatutCapital], [RegistrationDate], [Status], [Chief]) " +
                        "VALUES(@Id, @CustomerId, @Name, @Address, @Code, @Actions, @StatusWeb, @Phone, @Email, @StatutCapital, @RegistrationDate, @Status, @Chief)";

            var para = new
            {
                Id = idCo,
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
                Chief = Guid.Empty
            };
            using var database = Context.ConnectToSQL;
            try
            {
                await database.QueryAsync(query, para);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Projects
        public async Task CreateProject(string[] ar)
        {
            //if (!Guid.TryParse(ar[0], out Guid idCu))
            //{
            //    return;
            //}
            //if (!Guid.TryParse(ar[1], out Guid idCo))
            //{
            //    return;
            //}




            ////-----------------------------------------------------------------------------0

            //if (ar[2] == null && company.Categories != null)
            //{
            //    ProjectCategoryModel categoryModel = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = ar[6],
            //        Projects = [],
            //        CategoriesLevel1 = []
            //    };
            //    company.Categories.Add(categoryModel);
            //    UpdateJsonCustomers(customers);
            //    return;
            //}

            ////-----------------------------------------------------------------------------1

            //if (ar[2] != null && ar[3] == null && company.Categories != null)
            //{

            //    if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
            //    {
            //        return;
            //    }

            //    var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));

            //    if (cat0.CategoriesLevel1 == null)
            //    {
            //        return;
            //    }
            //    ProjectCategoryModel1 categoryModel1 = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = ar[6],
            //        Projects = [],
            //        CategoriesLevel2 = []
            //    };
            //    cat0.CategoriesLevel1.Add(categoryModel1);
            //    UpdateJsonCustomers(customers);
            //}

            ////-----------------------------------------------------------------------------

            //if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            //{
            //    if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
            //    {
            //        return;
            //    }

            //    var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
            //    if (cat0.CategoriesLevel1 == null)
            //    {
            //        return;
            //    }

            //    var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

            //    if (cat1.CategoriesLevel2 == null)
            //    {
            //        return;
            //    }
            //    ProjectCategoryModel2 categoryModel2 = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = ar[6],
            //        Projects = [],
            //        CategoriesLevel3 = []
            //    };
            //    cat1.CategoriesLevel2.Add(categoryModel2);
            //    UpdateJsonCustomers(customers);


            //}

            ////-----------------------------------------------------------------------------

            //if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            //{
            //    if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
            //    {
            //        return;
            //    }

            //    var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
            //    if (cat0.CategoriesLevel1 == null)
            //    {
            //        return;
            //    }

            //    var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
            //    if (cat1.CategoriesLevel2 == null)
            //    {
            //        return;
            //    }

            //    var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
            //    if (cat2.CategoriesLevel3 == null)
            //    {
            //        return;
            //    }

            //    ProjectCategoryModel3 categoryModel3 = new()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = ar[6],
            //        Projects = []
            //    };
            //    cat2.CategoriesLevel3.Add(categoryModel3);
            //    UpdateJsonCustomers(customers);
            //}
        }
        #endregion
        #region Categories
        public async Task CreateCategory(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo))
            {
                return;
            }

            if (ar[2] == null)
            {
                await CreateCategory0(idCu, idCo, ar[6]);
                return;
            }

            if (ar[2] != null && ar[3] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0))
                {
                    return;
                }
                await CreateCategory1(idCu, idCo, idCat0, ar[6]);
                return;
            }

            if (ar[2] != null && ar[3] != null && ar[4] == null)
            {
                if (!Guid.TryParse(ar[3], out Guid idCat1))
                {
                    return;
                }
                await CreateCategory2(idCu, idCo, idCat1, ar[6]);
                return;
            }

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null)
            {
                if (!Guid.TryParse(ar[4], out Guid idCat2))
                {
                    return;
                }

                await CreateCategory3(idCu, idCo, idCat2, ar[6]);
                return;
            }
        }
        public async Task CreateCategory0(Guid idCu, Guid idCo, string name)
        {
            var query = $@"INSERT INTO [{Context.dbName}].[dbo].[CompanyCategory0]
                               ([Id]
                               ,[CustomerId]
                               ,[CompanyId]
                               ,[Name])
                         VALUES
                               (@Id
                               ,@CustomerId
                               ,@CompanyId
                               ,@Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateCategory1(Guid idCu, Guid idCo, Guid idCat0, string name)
        {
            var query = $@"INSERT INTO [{Context.dbName}].[dbo].[CompanyCategory1]
                               ([Id]
                               ,[CustomerId]
                               ,[CompanyId]
                               ,[CompanyCategory0]
                               ,[Name])
                         VALUES
                               (@Id
                               ,@CustomerId
                               ,@CompanyId
                               ,@CompanyCategory0
                               ,@Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory0 = idCat0,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateCategory2(Guid idCu, Guid idCo, Guid idCat1, string name)
        {
            var query = $@"INSERT INTO [{Context.dbName}].[dbo].[CompanyCategory2]
                               ([Id]
                               ,[CustomerId]
                               ,[CompanyId]
                               ,[CompanyCategory1]
                               ,[Name])
                         VALUES
                               (@Id
                               ,@CustomerId
                               ,@CompanyId
                               ,@CompanyCategory1
                               ,@Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory1 = idCat1,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task CreateCategory3(Guid idCu, Guid idCo, Guid idCat2, string name)
        {
            var query = $@"INSERT INTO [{Context.dbName}].[dbo].[CompanyCategory3]
                               ([Id]
                               ,[CustomerId]
                               ,[CompanyId]
                               ,[CompanyCategory2]
                               ,[Name])
                         VALUES
                               (@Id
                               ,@CustomerId
                               ,@CompanyId
                               ,@CompanyCategory2
                               ,@Name)";
            var para = new
            {
                Id = Guid.NewGuid(),
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory2 = idCat2,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditNameCategory(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idCu))
            {
                return;
            }

            if (!Guid.TryParse(ar[1], out Guid idCo))
            {
                return;
            }

            if (ar[2] != null && ar[3] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0))
                {
                    return;
                }
                await EditNameCategory0(idCu, idCo, idCat0, ar[6]);
                return;
            }

            if (ar[2] != null && ar[3] != null && ar[4] == null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1))
                {
                    return;
                }

                await EditNameCategory1(idCu, idCo, idCat0, idCat1, ar[6]);
                return;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1) || !Guid.TryParse(ar[4], out Guid idCat2))
                {
                    return;
                }

                await EditNameCategory2(idCu, idCo, idCat1, idCat2, ar[6]);
            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1) || !Guid.TryParse(ar[4], out Guid idCat2) || !Guid.TryParse(ar[5], out Guid idCat3))
                {
                    return;
                }

                await EditNameCategory3(idCu, idCo, idCat2, idCat3, ar[6]);
            }
            return;
        }
        public async Task EditNameCategory0(Guid idCu, Guid idCo, Guid idCat0, string name)
        {
            var query = $@"UPDATE [{Context.dbName}].[dbo].[CompanyCategory0]
                           SET [Name] = @Name
                         WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId";
            var para = new
            {
                Id = idCat0,
                CustomerId = idCu,
                CompanyId = idCo,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditNameCategory1(Guid idCu, Guid idCo, Guid idCat0, Guid idCat1, string name)
        {
            var query = $@"UPDATE [{Context.dbName}].[dbo].[CompanyCategory1]
                           SET [Name] = @Name
                         WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory0 = @CompanyCategory0";
            var para = new
            {
                Id = idCat1,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory0 = idCat0,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditNameCategory2(Guid idCu, Guid idCo, Guid idCat1, Guid idCat2, string name)
        {
            var query = $@"UPDATE [{Context.dbName}].[dbo].[CompanyCategory2]
                           SET [Name] = @Name
                         WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory1 = @CompanyCategory1";
            var para = new
            {
                Id = idCat2,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory1 = idCat1,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task EditNameCategory3(Guid idCu, Guid idCo, Guid idCat2, Guid idCat3, string name)
        {
            var query = $@"UPDATE [{Context.dbName}].[dbo].[CompanyCategory3]
                           SET [Name] = @Name
                         WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory2 = @CompanyCategory2";
            var para = new
            {
                Id = idCat3,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory2 = idCat2,
                Name = name
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        public async Task DeleteCategory(string[] ar)
        {
            if (!Guid.TryParse(ar[0], out Guid idCu) || !Guid.TryParse(ar[1], out Guid idCo))
            {
                return;
            }

            if (ar[2] != null && ar[3] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0))
                {
                    return;
                }

                await DeleteCategory0(idCu, idCo, idCat0);
            }


            if (ar[2] != null && ar[3] != null && ar[4] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1))
                {
                    return;
                }
                await DeleteCategory1(idCu, idCo, idCat0, idCat1);
            }

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1) || !Guid.TryParse(ar[4], out Guid idCat2))
                {
                    return;
                }
                await DeleteCategory2(idCu, idCo, idCat1, idCat2);
            }

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCat0) || !Guid.TryParse(ar[3], out Guid idCat1) || !Guid.TryParse(ar[4], out Guid idCat2) || !Guid.TryParse(ar[5], out Guid idCat3))
                {
                    return;
                }
                await DeleteCategory3(idCu, idCo, idCat2, idCat3);
            }
            return;
        }

        public async Task DeleteCategory0(Guid idCu, Guid idCo, Guid idCat0) 
        {
            var cat1 = await new SQLRead().GetCategories1(idCu, idCo, idCat0);
            if (cat1.Count > 0)
            {
                foreach (var c1 in cat1)
                {
                    await DeleteCategory1(idCu, idCo, idCat0, c1.Id);
                    var cat2 = await new SQLRead().GetCategories2(idCu, idCo, c1.Id);
                    if (cat2.Count > 0)
                    {
                        foreach (var c2 in cat2)
                        {
                            await DeleteCategory2(idCu, idCo, c1.Id, c2.Id);
                            var cat3 = await new SQLRead().GetCategories3(idCu, idCo, c2.Id);
                            if (cat3.Count > 0)
                            {
                                foreach (var c3 in cat3)
                                {
                                    await DeleteCategory3(idCu, idCo, c2.Id, c3.Id);
                                }
                            }
                        }
                    }
                }
            }
            var query = $@"DELETE FROM [{Context.dbName}].[dbo].[CompanyCategory0]
                        WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId";
            var para = new
            {
                Id = idCat0,
                CustomerId = idCu,
                CompanyId = idCo
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }

        public async Task DeleteCategory1(Guid idCu, Guid idCo, Guid idCat0, Guid idCat1) 
        {
            var cat2 = await new SQLRead().GetCategories2(idCu, idCo, idCat1);
            if (cat2.Count > 0)
            {
                foreach (var c2 in cat2)
                {
                    await DeleteCategory2(idCu, idCo, idCat1, c2.Id);
                    var cat3 = await new SQLRead().GetCategories3(idCu, idCo, c2.Id);
                    if (cat3.Count > 0)
                    {
                        foreach (var c3 in cat3)
                        {
                            await DeleteCategory3(idCu, idCo, c2.Id, c3.Id);
                        }
                    }
                }
            }

            var query = $@"DELETE FROM [{Context.dbName}].[dbo].[CompanyCategory1]
                        WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory0 = @CompanyCategory0";
            var para = new
            {
                Id = idCat1,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory0 = idCat0
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        
        public async Task DeleteCategory2(Guid idCu, Guid idCo, Guid idCat1, Guid idCat2) 
        {
            var cat3 = await new SQLRead().GetCategories3(idCu, idCo, idCat2);
            if (cat3.Count > 0)
            {
                foreach (var c3 in cat3)
                {
                    await DeleteCategory3(idCu, idCo, idCat2, c3.Id);
                }
            }

            var query = $@"DELETE FROM [{Context.dbName}].[dbo].[CompanyCategory2]
                        WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory1 = @CompanyCategory1;";
            var para = new
            {
                Id = idCat2,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory1 = idCat1
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);

        }

        public async Task DeleteCategory3(Guid idCu, Guid idCo, Guid idCat2, Guid idCat3) 
        {
            var query = $@"DELETE FROM [{Context.dbName}].[dbo].[CompanyCategory3]
                        WHERE [Id] = @Id AND [CustomerId] = @CustomerId AND [CompanyId] = @CompanyId AND CompanyCategory2 = @CompanyCategory2";
            var para = new
            {
                Id = idCat3,
                CustomerId = idCu,
                CompanyId = idCo,
                CompanyCategory2 = idCat2
            };
            using var database = Context.ConnectToSQL;
            await database.QueryAsync(query, para);
        }
        #endregion
        #region ExtendedFunction
        string UpdateField(string fieldName, object? editedValue, object? originalValue)
        {
            return !IsNullOrEmpty(editedValue) && !AreEqual(editedValue, originalValue)
                ? $"[{fieldName}] = @{fieldName}, "
                : "";
        }
        bool IsNullOrEmpty(object? value)
        {
            return value == null || (value is string str && string.IsNullOrEmpty(str));
        }
        bool AreEqual(object? value1, object? value2)
        {
            return Object.Equals(value1, value2);
        }
        #endregion
    }
}
