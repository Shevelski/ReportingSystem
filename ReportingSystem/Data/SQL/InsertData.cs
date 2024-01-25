using Bogus.DataSets;
using Dapper;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace ReportingSystem.Data.SQL
{

    public class InsertData
    {
        private readonly IDbConnection _database;

        public InsertData()
        {
            _database = Context.ConnectToSQL;
        }

        private async Task InsertEnumData<T>(string tableName, IEnumerable<T> enumValues) where T : Enum
        {
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[{tableName}] ([Id], [Type], [Name]) VALUES (@Id, @Type, @Name)";

            foreach (T enumValue in enumValues)
            {
                var parameters = new
                {
                    Id = Guid.NewGuid(),
                    Type = enumValue,
                    Name = enumValue.GetDisplayName()
                };

                await _database.ExecuteAsync(query, parameters);
            }
        }

        public async Task StatusLicence() => await InsertEnumData("StatusLicence", Enum.GetValues<LicenceType>());

        public async Task AuthorizeStatus() => await InsertEnumData("AuthorizeStatus", Enum.GetValues<AuthorizeStatus>());

        public async Task AuthorizeHistory() => await InsertEnumData("AuthorizeHistory", Enum.GetValues<AuthorizeStatus>());

        public async Task CompanyStatus() => await InsertEnumData("CompanyStatus", Enum.GetValues<CompanyStatus>());

        public async Task EmployeeRolStatus() => await InsertEnumData("EmployeeRolStatus", Enum.GetValues<EmployeeRolStatus>());

        public async Task EmployeeStatus() => await InsertEnumData("EmployeeStatus", Enum.GetValues<EmployeeStatus>());

        public async Task ProjectStatus() => await InsertEnumData("ProjectStatus", Enum.GetValues<ProjectStatus>());

        public async Task EmployeePosition(EmployeePositionModel position, Guid customerId, Guid companyId, int type)
        {
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[EmployeePosition] ([Id], [CustomerId], [CompanyId], [Type], [Name]) VALUES (@Id, @CustomerId, @CompanyId, @Type, @Name)";
            var parameters = new
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CompanyId = companyId,
                Type = type,
                Name = position.NamePosition
            };

            await _database.ExecuteAsync(query, parameters);
        }

        public async Task CompanyRolls(Guid rolId, Guid customerId, Guid companyId)
        {
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[CompanyRolls] ([Id], [CustomerId], [CompanyId], [RolId]) VALUES (@Id, @CustomerId, @CompanyId, @RolId)";
            var parameters = new
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CompanyId = companyId,
                RolId = rolId
            };

            await _database.ExecuteAsync(query, parameters);
        }

        public async Task Employee(EmployeeModel employee)
        {
            var checkQuery1 = "SELECT COUNT(*) " +
                             $"FROM [{Context.dbName}].[dbo].[Administrators]" +
                             "Where EmailWork = @EmailWork";
            var checkQuery2 = "SELECT COUNT(*) " +
                             $"FROM [{Context.dbName}].[dbo].[Employees]" +
                             "Where EmailWork = @EmailWork";
            var paraCheck = new
            {
                EmailWork = employee.EmailWork,
            };

            var checkResult1 = await _database.QueryAsync<int>(checkQuery1, paraCheck);
            var checkResult2 = await _database.QueryAsync<int>(checkQuery2, paraCheck);

            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
            {
                if (employee.Status == null || employee.Rol == null || employee.Password == null || employee.Position == null)
                {
                    return;
                }
                var paraStatus = new
                {
                    Type = employee.Status.EmployeeStatusType,
                };
                var statusQuery = "SELECT [id]" +
                              $"FROM [{Context.dbName}].[dbo].[EmployeeStatus]" +
                              "Where Type = @Type";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery, paraStatus);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                var paraStatus1 = new
                {
                    Type = employee.Rol.RolType,
                };
                var rolQuery = "SELECT [id]" +
                          $"FROM [{Context.dbName}].[dbo].[EmployeeRolStatus]" +
                          "Where Type = @Type";
                var rolResult = await _database.QueryAsync<Guid>(rolQuery, paraStatus1);

                Guid rol = Guid.Empty;

                if (rolResult.Any())
                {
                    rol = rolResult.First();
                }

                var paraPosition = new
                {
                    Name = employee.Position.NamePosition,
                };
                var statusPosition = "SELECT [id]" +
                              $"FROM [{Context.dbName}].[dbo].[EmployeePosition]" +
                              "Where Name = @Name";
                var positionResult = await _database.QueryAsync<Guid>(statusPosition, paraPosition);

                Guid statusPos = Guid.Empty;

                if (positionResult.Any())
                {
                    statusPos = positionResult.First();
                }

                var query = $"INSERT INTO [{Context.dbName}].[dbo].[Employees]" +
                    "([Id],[CompanyId],[CustomerId],[FirstName],[SecondName],[ThirdName],[PhoneWork],[PhoneSelf],[EmailWork],[EmailSelf],[TaxNumber],[AddressReg],[AddressFact],[Photo],[Login],[Password],[Salary],[AddSalary],[Status],[BirthDate],[WorkStartDate],[WorkEndDate],[Position],[Rol])" +
                    "VALUES (@Id,@CompanyId,@CustomerId,@FirstName,@SecondName,@ThirdName,@PhoneWork,@PhoneSelf,@EmailWork,@EmailSelf,@TaxNumber,@AddressReg,@AddressFact,@Photo,@Login,@Password,@Salary,@AddSalary,@Status,@BirthDate,@WorkStartDate,@WorkEndDate,@Position,@Rol)";

                var parameters = new
                {
                    Id = employee.Id,
                    CompanyId = employee.CompanyId,
                    CustomerId = employee.CustomerId,
                    FirstName = employee.FirstName,
                    SecondName = employee.SecondName,
                    ThirdName = employee.ThirdName,
                    PhoneWork = employee.PhoneWork,
                    PhoneSelf = employee.PhoneSelf,
                    EmailWork = employee.EmailWork,
                    EmailSelf = employee.EmailSelf,
                    TaxNumber = employee.TaxNumber,
                    AddressReg = employee.AddressReg,
                    AddressFact = employee.AddressFact,
                    Photo = employee.Photo,
                    Login = employee.Login,
                    Password = employee.Password,//EncryptionHelper.Encrypt(employee.Password),
                    Salary = employee.Salary,
                    AddSalary = employee.AddSalary,
                    Status = status,
                    BirthDate = employee.BirthDate,
                    WorkStartDate = employee.WorkStartDate,
                    WorkEndDate = employee.WorkEndDate,
                    Position = statusPos,
                    Rol = rol
                };
                try
                {
                    await _database.ExecuteAsync(query, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
                }
                
            }
        }

        public async Task Administrators(List<EmployeeModel> employees)
        {
            foreach (EmployeeModel employee in employees)
            {
                await Administrator(employee);
            }
        }

        public async Task Administrator(EmployeeModel employee)
        {
            // Your Administrator insertion logic
            var checkQuery1 = "SELECT COUNT(*) " +
                             $"FROM [{Context.dbName}].[dbo].[Administrators]" +
                             "Where EmailWork = @EmailWork";
            var checkQuery2 = "SELECT COUNT(*) " +
                             $"FROM [{Context.dbName}].[dbo].[Employees]" +
                             "Where EmailWork = @EmailWork";
            var paraCheck = new
            {
                EmailWork = employee.EmailWork,
            };

            var checkResult1 = await _database.QueryAsync<int>(checkQuery1, paraCheck);
            var checkResult2 = await _database.QueryAsync<int>(checkQuery2, paraCheck);

            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
            {
                var statusQuery = "SELECT [id]" +
                              $"FROM [{Context.dbName}].[dbo].[EmployeeStatus]" +
                              "Where Type = 1";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                var rol0 = employee.Rol;

                var paraStatus1 = new
                {
                    Type = rol0 != null ? rol0.RolType : Enums.EmployeeRolStatus.Developer,
                };

                var rolQuery = "SELECT [id]" +
                          $"FROM [{Context.dbName}].[dbo].[EmployeeRolStatus]" +
                          "Where Type = @Type";
                var rolResult = await _database.QueryAsync<Guid>(rolQuery, paraStatus1);

                Guid rol = Guid.Empty;

                if (rolResult.Any())
                {
                    rol = rolResult.First();
                }

                if (employee.Password == null)
                {
                    return;
                }

                var query = $"INSERT INTO [{Context.dbName}].[dbo].[Administrators] " +
                            "([Id],[FirstName],[SecondName],[ThirdName],[PhoneWork],[EmailWork],[Login],[Password],[Status],[BirthDate],[Rol])" +
                            "VALUES (@Id,@FirstName,@SecondName,@ThirdName,@PhoneWork,@EmailWork,@Login,@Password,@Status,@BirthDate,@Rol)";

                var parameters = new
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    SecondName = employee.SecondName,
                    ThirdName = employee.ThirdName,
                    PhoneWork = employee.PhoneWork,
                    EmailWork = employee.EmailWork,
                    Login = employee.Login,
                    Password = employee.Password,//EncryptionHelper.Encrypt(employee.password),
                    Status = status,
                    BirthDate = employee.BirthDate,
                    Rol = rol
                };
                try
                {
                    await _database.ExecuteAsync(query, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
                }
                
            }
        }

        public async Task Customer(CustomerModel customer)
        {
            // Your Customer insertion logic
            using (var database = Context.ConnectToSQL)
            {
                // Створення нового Configure і отримання Id
                var configureQuery = $"INSERT INTO [{Context.dbName}].[dbo].[Configure] " +
                                    "([Id],[Param1],[Param2],[Param3],[Param4],[Param5],[Param6],[Param7],[Param8],[Param9],[Param10],[Param11],[Param12]) " +
                                    "VALUES (@Id, @Param1, @Param2, @Param3, @Param4, @Param5, @Param6, @Param7, @Param8, @Param9, @Param10, @Param11, @Param12); ";

                var configureParameters = new
                {
                    Id = Guid.NewGuid(),
                    Param1 = -1,
                    Param2 = -1,
                    Param3 = -1,
                    Param4 = -1,
                    Param5 = "string",
                    Param6 = "string",
                    Param7 = "string",
                    Param8 = "string",
                    Param9 = Guid.Empty,
                    Param10 = Guid.Empty,
                    Param11 = Guid.Empty,
                    Param12 = Guid.Empty,
                };

                await _database.ExecuteAsync(configureQuery, configureParameters);

                // Вставка запису в таблицю Customers з використанням отриманого newConfigId
                var customersQuery = $"INSERT INTO [{Context.dbName}].[dbo].[Customers] " +
                                     "([Id],[FirstName],[SecondName],[ThirdName],[StatusLicenceId],[ConfigureId],[Phone],[Email],[Password],[EndTimeLicense],[DateRegistration]) " +
                                     "VALUES (@Id, @FirstName, @SecondName, @ThirdName, @StatusLicenceId, @ConfigureId, @Phone, @Email, @Password, @EndTimeLicense, @DateRegistration);";


                int type = -1;
                if (customer != null && customer.StatusLicence != null)
                {
                    type = (int)customer.StatusLicence.LicenceType;
                }

                var parametersStatus = new
                {
                    Type = type,
                };

                var statusQuery = "SELECT [id]" +
                              $"FROM [{Context.dbName}].[dbo].[StatusLicence]" +
                              "Where Type = @Type";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery, parametersStatus);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                if (customer != null && customer.Password != null)
                {
                    var parameters = new
                    {
                        Id = customer.Id,
                        FirstName = customer.FirstName,
                        SecondName = customer.SecondName,
                        ThirdName = customer.ThirdName,
                        StatusLicenceId = status,
                        ConfigureId = configureParameters.Id,
                        Phone = customer.Phone,
                        Email = customer.Email,
                        Password = customer.Password,//EncryptionHelper.Encrypt(customer.password),
                        EndTimeLicense = customer.EndTimeLicense,
                        DateRegistration = customer.DateRegistration,
                    };

                    await _database.ExecuteAsync(customersQuery, parameters);
                }
            }
        }

        public async Task Company(CompanyModel company, Guid customerId)
        {
            // Your Company insertion logic
            var query = $"INSERT INTO [{Context.dbName}].[dbo].[Companies]" +
                "([Id],[CustomerId],[Name],[Address],[Code],[Actions],[StatusWeb],[Phone],[Email],[StatutCapital],[RegistrationDate],[Status],[Chief])" +
                "VALUES (@Id, @CustomerId, @Name, @Address, @Code, @Actions, @StatusWeb, @Phone, @Email, @StatutCapital, @RegistrationDate, @Status, @Chief)";

            int type = -1;
            if (company != null && company.Status != null && company.Status != null)
            {
                company.Status.CompanyStatusName = Enums.CompanyStatus.Actual.GetDisplayName();
                company.Status.CompanyStatusType = Enums.CompanyStatus.Actual;
                type = (int)company.Status.CompanyStatusType;
            }

            var parametersStatus = new
            {
                Type = type,
            };

            var statusQuery = "SELECT [id]" +
                          $"FROM [{Context.dbName}].[dbo].[CompanyStatus]" +
                          "Where Type = @Type";
            var statusResult = await _database.QueryAsync<Guid>(statusQuery, parametersStatus);

            Guid status = Guid.Empty;

            if (statusResult.Any())
            {
                status = statusResult.First();
            }

            if (company != null)
            {
                Guid? chief = Guid.Empty;
                if (company.Chief != null)
                {
                    chief = company.Chief.Id;
                }
                var parameters = new
                {
                    Id = company.Id,
                    CustomerId = customerId,
                    Name = company.Name,
                    Address = company.Address,
                    Code = company.Code,
                    Actions = company.Actions,
                    StatusWeb = company.StatusWeb,
                    Phone = company.Phone,
                    Email = company.Email,
                    StatutCapital = company.StatutCapital,
                    RegistrationDate = company.RegistrationDate,
                    Status = status,
                    Chief = chief
                };
                await _database.ExecuteAsync(query, parameters);
            }
        }
    }
}
