﻿using Bogus.DataSets;
using Dapper;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
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
            var query = $"INSERT INTO [dbo].[{tableName}] ([Id], [Type], [Name]) VALUES (@Id, @Type, @Name)";

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
            var query = "INSERT INTO [dbo].[EmployeePosition] ([Id], [CustomerId], [CompanyId], [Type], [Name]) VALUES (@Id, @CustomerId, @CompanyId, @Type, @Name)";
            var parameters = new
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CompanyId = companyId,
                Type = type,
                Name = position.namePosition
            };

            await _database.ExecuteAsync(query, parameters);
        }

        public async Task CompanyRolls(Guid rolId, Guid customerId, Guid companyId)
        {
            var query = "INSERT INTO [dbo].[CompanyRolls] ([Id], [CustomerId], [CompanyId], [RolId]) VALUES (@Id, @CustomerId, @CompanyId, @RolId)";
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
                             "FROM[ReportingSystem].[dbo].[Administrators]" +
                             "Where EmailWork = @EmailWork";
            var checkQuery2 = "SELECT COUNT(*) " +
                             "FROM[ReportingSystem].[dbo].[Employees]" +
                             "Where EmailWork = @EmailWork";
            var paraCheck = new
            {
                EmailWork = employee.emailWork,
            };

            var checkResult1 = await _database.QueryAsync<int>(checkQuery1, paraCheck);
            var checkResult2 = await _database.QueryAsync<int>(checkQuery2, paraCheck);

            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
            {
                if (employee.status == null || employee.rol == null || employee.password == null || employee.position == null)
                {
                    return;
                }
                var paraStatus = new
                {
                    Type = employee.status.employeeStatusType,
                };
                var statusQuery = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[EmployeeStatus]" +
                              "Where Type = @Type";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery, paraStatus);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                var paraStatus1 = new
                {
                    Type = employee.rol.rolType,
                };
                var rolQuery = "SELECT [id]" +
                          "FROM [ReportingSystem].[dbo].[EmployeeRolStatus]" +
                          "Where Type = @Type";
                var rolResult = await _database.QueryAsync<Guid>(rolQuery, paraStatus1);

                Guid rol = Guid.Empty;

                if (rolResult.Any())
                {
                    rol = rolResult.First();
                }

                var paraPosition = new
                {
                    Name = employee.position.namePosition,
                };
                var statusPosition = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[EmployeePosition]" +
                              "Where Name = @Name";
                var positionResult = await _database.QueryAsync<Guid>(statusPosition, paraPosition);

                Guid statusPos = Guid.Empty;

                if (positionResult.Any())
                {
                    statusPos = positionResult.First();
                }

                var query = "INSERT INTO [dbo].[Employees]" +
                    "([Id],[CompanyId],[CustomerId],[FirstName],[SecondName],[ThirdName],[PhoneWork],[PhoneSelf],[EmailWork],[EmailSelf],[TaxNumber],[AddressReg],[AddressFact],[Photo],[Login],[Password],[Salary],[AddSalary],[Status],[BirthDate],[WorkStartDate],[WorkEndDate],[Position],[Rol])" +
                    "VALUES (@Id,@CompanyId,@CustomerId,@FirstName,@SecondName,@ThirdName,@PhoneWork,@PhoneSelf,@EmailWork,@EmailSelf,@TaxNumber,@AddressReg,@AddressFact,@Photo,@Login,@Password,@Salary,@AddSalary,@Status,@BirthDate,@WorkStartDate,@WorkEndDate,@Position,@Rol)";

                var parameters = new
                {
                    Id = employee.id,
                    CompanyId = employee.companyId,
                    CustomerId = employee.customerId,
                    FirstName = employee.firstName,
                    SecondName = employee.secondName,
                    ThirdName = employee.thirdName,
                    PhoneWork = employee.phoneWork,
                    PhoneSelf = employee.phoneSelf,
                    EmailWork = employee.emailWork,
                    EmailSelf = employee.emailSelf,
                    TaxNumber = employee.taxNumber,
                    AddressReg = employee.addressReg,
                    AddressFact = employee.addressFact,
                    Photo = employee.photo,
                    Login = employee.login,
                    Password = employee.password,//EncryptionHelper.Encrypt(employee.password),
                    Salary = employee.salary,
                    AddSalary = employee.addSalary,
                    Status = status,
                    BirthDate = employee.birthDate,
                    WorkStartDate = employee.workStartDate,
                    WorkEndDate = employee.workEndDate,
                    Position = statusPos,
                    Rol = rol
                };

                await _database.ExecuteAsync(query, parameters);
            }
        }

        public async Task Administrators(List<EmployeeModel> employees)
        {
            foreach (EmployeeModel employee in employees)
            {
                await Employee(employee);
            }
        }

        public async Task Administrator(EmployeeModel employee)
        {
            // Your Administrator insertion logic
            var checkQuery1 = "SELECT COUNT(*) " +
                             "FROM[ReportingSystem].[dbo].[Administrators]" +
                             "Where EmailWork = @EmailWork";
            var checkQuery2 = "SELECT COUNT(*) " +
                             "FROM[ReportingSystem].[dbo].[Employees]" +
                             "Where EmailWork = @EmailWork";
            var paraCheck = new
            {
                EmailWork = employee.emailWork,
            };

            var checkResult1 = await _database.QueryAsync<int>(checkQuery1, paraCheck);
            var checkResult2 = await _database.QueryAsync<int>(checkQuery2, paraCheck);

            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
            {
                var statusQuery = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[EmployeeStatus]" +
                              "Where Type = 1";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                var rol0 = employee.rol;

                var paraStatus1 = new
                {
                    Type = rol0 != null ? rol0.rolType : Enums.EmployeeRolStatus.Developer,
                };

                var rolQuery = "SELECT [id]" +
                          "FROM [ReportingSystem].[dbo].[EmployeeRolStatus]" +
                          "Where Type = @Type";
                var rolResult = await _database.QueryAsync<Guid>(rolQuery, paraStatus1);

                Guid rol = Guid.Empty;

                if (rolResult.Any())
                {
                    rol = rolResult.First();
                }

                if (employee.password == null)
                {
                    return;
                }

                var query = "INSERT INTO [dbo].[Administrators] " +
                            "([Id],[FirstName],[SecondName],[ThirdName],[PhoneWork],[EmailWork],[Login],[Password],[Status],[BirthDate],[Rol])" +
                            "VALUES (@Id,@FirstName,@SecondName,@ThirdName,@PhoneWork,@EmailWork,@Login,@Password,@Status,@BirthDate,@Rol)";

                var parameters = new
                {
                    Id = employee.id,
                    FirstName = employee.firstName,
                    SecondName = employee.secondName,
                    ThirdName = employee.thirdName,
                    PhoneWork = employee.phoneWork,
                    EmailWork = employee.emailWork,
                    Login = employee.login,
                    Password = employee.password,//EncryptionHelper.Encrypt(employee.password),
                    Status = status,
                    BirthDate = employee.birthDate,
                    Rol = rol
                };

                await _database.ExecuteAsync(query, parameters);
            }
        }

        public async Task Customer(CustomerModel customer)
        {
            // Your Customer insertion logic
            using (var database = Context.ConnectToSQL)
            {
                // Створення нового Configure і отримання Id
                var configureQuery = "INSERT INTO [dbo].[Configure] " +
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
                var customersQuery = "INSERT INTO [dbo].[Customers] " +
                                     "([Id],[FirstName],[SecondName],[ThirdName],[StatusLicenceId],[ConfigureId],[Phone],[Email],[Password],[EndTimeLicense],[DateRegistration]) " +
                                     "VALUES (@Id, @FirstName, @SecondName, @ThirdName, @StatusLicenceId, @ConfigureId, @Phone, @Email, @Password, @EndTimeLicense, @DateRegistration);";


                int type = -1;
                if (customer != null && customer.statusLicence != null)
                {
                    type = (int)customer.statusLicence.licenceType;
                }

                var parametersStatus = new
                {
                    Type = type,
                };

                var statusQuery = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[StatusLicence]" +
                              "Where Type = @Type";
                var statusResult = await _database.QueryAsync<Guid>(statusQuery, parametersStatus);

                Guid status = Guid.Empty;

                if (statusResult.Any())
                {
                    status = statusResult.First();
                }

                if (customer != null && customer.password != null)
                {
                    var parameters = new
                    {
                        Id = customer.id,
                        FirstName = customer.firstName,
                        SecondName = customer.secondName,
                        ThirdName = customer.thirdName,
                        StatusLicenceId = status,
                        ConfigureId = configureParameters.Id,
                        Phone = customer.phone,
                        Email = customer.email,
                        Password = customer.password,//EncryptionHelper.Encrypt(customer.password),
                        EndTimeLicense = customer.endTimeLicense,
                        DateRegistration = customer.dateRegistration,
                    };

                    await _database.ExecuteAsync(customersQuery, parameters);
                }
            }
        }

        public async Task Company(CompanyModel company, Guid customerId)
        {
            // Your Company insertion logic
            var query = "INSERT INTO [dbo].[Companies]" +
                "([Id],[CustomerId],[Name],[Address],[Code],[Actions],[StatusWeb],[Phone],[Email],[StatutCapital],[RegistrationDate],[Status],[Chief])" +
                "VALUES (@Id, @CustomerId, @Name, @Address, @Code, @Actions, @StatusWeb, @Phone, @Email, @StatutCapital, @RegistrationDate, @Status, @Chief)";

            int type = -1;
            if (company != null && company.status != null && company.status != null)
            {
                company.status.companyStatusName = Enums.CompanyStatus.Actual.GetDisplayName();
                company.status.companyStatusType = Enums.CompanyStatus.Actual;
                type = (int)company.status.companyStatusType;
            }

            var parametersStatus = new
            {
                Type = type,
            };

            var statusQuery = "SELECT [id]" +
                          "FROM [ReportingSystem].[dbo].[CompanyStatus]" +
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
                if (company.chief != null)
                {
                    chief = company.chief.id;
                }
                var parameters = new
                {
                    Id = company.id,
                    CustomerId = customerId,
                    Name = company.name,
                    Address = company.address,
                    Code = company.code,
                    Actions = company.actions,
                    StatusWeb = company.statusWeb,
                    Phone = company.phone,
                    Email = company.email,
                    StatutCapital = company.statutCapital,
                    RegistrationDate = company.registrationDate,
                    Status = status,
                    Chief = chief
                };
                await _database.ExecuteAsync(query, parameters);
            }
        }
    }

    //public class InsertData
    //{
    //    public async Task StatusLicence()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[StatusLicence] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (LicenceType licence in Enum.GetValues(typeof(LicenceType)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)licence,
    //                    Name = licence.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task AuthorizeStatus()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[AuthorizeStatus] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (AuthorizeStatus authorize in Enum.GetValues(typeof(AuthorizeStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)authorize,
    //                    Name = authorize.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task AuthorizeHistory()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {

    //            var query = "INSERT INTO [dbo].[AuthorizeHistory] " +
    //                        "([Id],[EmployeeId], [RolId], [AuthorizeStatusId]) " +
    //                        "VALUES (@Id, @Type, @Name)";

    //            foreach (AuthorizeStatus authorize in Enum.GetValues(typeof(AuthorizeStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)authorize,
    //                    Name = authorize.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task CompanyStatus()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[CompanyStatus] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (CompanyStatus company in Enum.GetValues(typeof(CompanyStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)company,
    //                    Name = company.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task EmployeeRolStatus()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[EmployeeRolStatus] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (EmployeeRolStatus rol in Enum.GetValues(typeof(EmployeeRolStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)rol,
    //                    Name = rol.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //            foreach (EmployeeRolStatus rol in Enum.GetValues(typeof(EmployeeRolStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)rol,
    //                    Name = rol.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task EmployeeStatus()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[EmployeeStatus] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (EmployeeStatus employee in Enum.GetValues(typeof(EmployeeStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)employee,
    //                    Name = employee.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //    public async Task ProjectStatus()
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[ProjectStatus] " +
    //                        "([Id],[Type], [Name]) " +
    //                        "VALUES (@Id,@Type, @Name)";

    //            foreach (ProjectStatus project in Enum.GetValues(typeof(ProjectStatus)))
    //            {
    //                var parameters = new
    //                {
    //                    Id = Guid.NewGuid(),
    //                    Type = (int)project,
    //                    Name = project.GetDisplayName()
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }

    //    public async Task EmployeePosition(EmployeePositionModel position, Guid customerId, Guid companyId, int type)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[EmployeePosition]" +
    //                        "([Id],[CustomerId],[CompanyId],[Type],[Name])" +
    //                        "VALUES (@Id, @CustomerId, @CompanyId, @Type, @Name)";

    //            var parameters = new
    //            {
    //                Id = Guid.NewGuid(),
    //                CustomerId = customerId,
    //                CompanyId = companyId,
    //                Type = type,
    //                Name = position.namePosition
    //            };

    //            await database.ExecuteAsync(query, parameters);

    //        }
    //    }
    //    public async Task CompanyRolls(Guid rolId, Guid customerId, Guid companyId)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[CompanyRolls]" +
    //                        "([Id],[CustomerId],[CompanyId],[RolId])" +
    //                        "VALUES (@Id, @CustomerId, @CompanyId, @RolId)";
    //            var parameters = new
    //            {
    //                Id = Guid.NewGuid(),
    //                CustomerId = customerId,
    //                CompanyId = companyId,
    //                RolId = rolId
    //            };

    //            await database.ExecuteAsync(query, parameters);

    //        }
    //    }

    //    public async Task Employee(EmployeeModel employee)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var checkQuery1 = "SELECT COUNT(*) " +
    //                             "FROM[ReportingSystem].[dbo].[Administrators]" +
    //                             "Where EmailWork = @EmailWork";
    //            var checkQuery2 = "SELECT COUNT(*) " +
    //                             "FROM[ReportingSystem].[dbo].[Employees]" +
    //                             "Where EmailWork = @EmailWork";
    //            var paraCheck = new
    //            {
    //                EmailWork = employee.emailWork,
    //            };

    //            var checkResult1 = await database.QueryAsync<int>(checkQuery1, paraCheck);
    //            var checkResult2 = await database.QueryAsync<int>(checkQuery2, paraCheck);

    //            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
    //            {
    //                if (employee.status == null || employee.rol == null || employee.password == null || employee.position == null)
    //                {
    //                    return;
    //                }
    //                var paraStatus = new
    //                {
    //                    Type = employee.status.employeeStatusType,
    //                };
    //                var statusQuery = "SELECT [id]" +
    //                              "FROM [ReportingSystem].[dbo].[EmployeeStatus]" +
    //                              "Where Type = @Type";
    //                var statusResult = await database.QueryAsync<Guid>(statusQuery, paraStatus);

    //                Guid status = Guid.Empty;

    //                if (statusResult.Any())
    //                {
    //                    status = statusResult.First();
    //                }

    //                var paraStatus1 = new
    //                {
    //                    Type = employee.rol.rolType,
    //                };
    //                var rolQuery = "SELECT [id]" +
    //                          "FROM [ReportingSystem].[dbo].[EmployeeRolStatus]" +
    //                          "Where Type = @Type";
    //                var rolResult = await database.QueryAsync<Guid>(rolQuery, paraStatus1);

    //                Guid rol = Guid.Empty;

    //                if (rolResult.Any())
    //                {
    //                    rol = rolResult.First();
    //                }


    //                var paraPosition = new
    //                {
    //                    Name = employee.position.namePosition,
    //                };
    //                var statusPosition = "SELECT [id]" +
    //                              "FROM [ReportingSystem].[dbo].[EmployeePosition]" +
    //                              "Where Name = @Name";
    //                var positionResult = await database.QueryAsync<Guid>(statusPosition, paraPosition);

    //                Guid statusPos = Guid.Empty;

    //                if (positionResult.Any())
    //                {
    //                    statusPos = positionResult.First();
    //                }


    //                var query = "INSERT INTO [dbo].[Employees]" +
    //                    "([Id],[CompanyId],[CustomerId],[FirstName],[SecondName],[ThirdName],[PhoneWork],[PhoneSelf],[EmailWork],[EmailSelf],[TaxNumber],[AddressReg],[AddressFact],[Photo],[Login],[Password],[Salary],[AddSalary],[Status],[BirthDate],[WorkStartDate],[WorkEndDate],[Position],[Rol])" +
    //                    "VALUES (@Id,@CompanyId,@CustomerId,@FirstName,@SecondName,@ThirdName,@PhoneWork,@PhoneSelf,@EmailWork,@EmailSelf,@TaxNumber,@AddressReg,@AddressFact,@Photo,@Login,@Password,@Salary,@AddSalary,@Status,@BirthDate,@WorkStartDate,@WorkEndDate,@Position,@Rol)";

    //                var parameters = new
    //                {
    //                    Id = employee.id,
    //                    CompanyId = employee.companyId,
    //                    CustomerId = employee.customerId,
    //                    FirstName = employee.firstName,
    //                    SecondName = employee.secondName,
    //                    ThirdName = employee.thirdName,
    //                    PhoneWork = employee.phoneWork,
    //                    PhoneSelf = employee.phoneSelf,
    //                    EmailWork = employee.emailWork,
    //                    EmailSelf = employee.emailSelf,
    //                    TaxNumber = employee.taxNumber,
    //                    AddressReg = employee.addressReg,
    //                    AddressFact = employee.addressFact,
    //                    Photo = employee.photo,
    //                    Login = employee.login,
    //                    Password = EncryptionHelper.Encrypt(employee.password),
    //                    Salary = employee.salary,
    //                    AddSalary = employee.addSalary,
    //                    Status = status,
    //                    BirthDate = employee.birthDate,
    //                    WorkStartDate = employee.workStartDate,
    //                    WorkEndDate = employee.workEndDate,
    //                    Position = statusPos,
    //                    Rol = rol
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }

    //    public async Task Administrators(List<EmployeeModel> employees)
    //    {
    //        foreach (EmployeeModel employee in employees)
    //        {
    //            await new InsertData().Administrator(employee);
    //        }
    //    }

    //    public async Task Administrator(EmployeeModel employee)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var checkQuery1 = "SELECT COUNT(*) " +
    //                             "FROM[ReportingSystem].[dbo].[Administrators]" +
    //                             "Where EmailWork = @EmailWork";
    //            var checkQuery2 = "SELECT COUNT(*) " +
    //                             "FROM[ReportingSystem].[dbo].[Employees]" +
    //                             "Where EmailWork = @EmailWork";
    //            var paraCheck = new
    //            {
    //                EmailWork = employee.emailWork,
    //            };

    //            var checkResult1 = await database.QueryAsync<int>(checkQuery1, paraCheck);
    //            var checkResult2 = await database.QueryAsync<int>(checkQuery2, paraCheck);

    //            if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
    //            {
    //                var statusQuery = "SELECT [id]" +
    //                              "FROM [ReportingSystem].[dbo].[EmployeeStatus]" +
    //                              "Where Type = 1";
    //                var statusResult = await database.QueryAsync<Guid>(statusQuery);

    //                Guid status = Guid.Empty;

    //                if (statusResult.Any())
    //                {
    //                    status = statusResult.First();
    //                }

    //                var rol0 = employee.rol;

    //                var paraStatus1 = new
    //                {
    //                    Type = rol0 != null ? rol0.rolType : Enums.EmployeeRolStatus.Developer,
    //                };

    //                var rolQuery = "SELECT [id]" +
    //                          "FROM [ReportingSystem].[dbo].[EmployeeRolStatus]" +
    //                          "Where Type = @Type";
    //                var rolResult = await database.QueryAsync<Guid>(rolQuery, paraStatus1);

    //                Guid rol = Guid.Empty;

    //                if (rolResult.Any())
    //                {
    //                    rol = rolResult.First();
    //                }

    //                if (employee.password == null)
    //                {
    //                    return;
    //                }

    //                var query = "INSERT INTO [dbo].[Administrators] " +
    //                            "([Id],[FirstName],[SecondName],[ThirdName],[PhoneWork],[EmailWork],[Login],[Password],[Status],[BirthDate],[Rol])" +
    //                            "VALUES (@Id,@FirstName,@SecondName,@ThirdName,@PhoneWork,@EmailWork,@Login,@Password,@Status,@BirthDate,@Rol)";

    //                var parameters = new
    //                {
    //                    Id = employee.id,
    //                    FirstName = employee.firstName,
    //                    SecondName = employee.secondName,
    //                    ThirdName = employee.thirdName,
    //                    PhoneWork = employee.phoneWork,
    //                    EmailWork = employee.emailWork,
    //                    Login = employee.login,
    //                    Password = employee.password,//EncryptionHelper.Encrypt(employee.password),
    //                    Status = status,
    //                    BirthDate = employee.birthDate,
    //                    Rol = rol
    //                };

    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }

    //    public async Task Customer(CustomerModel customer)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            // Створення нового Configure і отримання Id
    //            var configureQuery = "INSERT INTO [dbo].[Configure] " +
    //                                "([Id],[Param1],[Param2],[Param3],[Param4],[Param5],[Param6],[Param7],[Param8],[Param9],[Param10],[Param11],[Param12]) " +
    //                                "VALUES (@Id, @Param1, @Param2, @Param3, @Param4, @Param5, @Param6, @Param7, @Param8, @Param9, @Param10, @Param11, @Param12); ";

    //            var configureParameters = new
    //            {
    //                Id = Guid.NewGuid(),
    //                Param1 = -1,
    //                Param2 = -1,
    //                Param3 = -1,
    //                Param4 = -1,
    //                Param5 = "string",
    //                Param6 = "string",
    //                Param7 = "string",
    //                Param8 = "string",
    //                Param9 = Guid.Empty,
    //                Param10 = Guid.Empty,
    //                Param11 = Guid.Empty,
    //                Param12 = Guid.Empty,
    //            };

    //            await database.ExecuteAsync(configureQuery, configureParameters);

    //            // Вставка запису в таблицю Customers з використанням отриманого newConfigId
    //            var customersQuery = "INSERT INTO [dbo].[Customers] " +
    //                                 "([Id],[FirstName],[SecondName],[ThirdName],[StatusLicenceId],[ConfigureId],[Phone],[Email],[Password],[EndTimeLicense],[DateRegistration]) " +
    //                                 "VALUES (@Id, @FirstName, @SecondName, @ThirdName, @StatusLicenceId, @ConfigureId, @Phone, @Email, @Password, @EndTimeLicense, @DateRegistration);";


    //            int type = -1;
    //            if (customer != null && customer.statusLicence != null)
    //            {
    //                type = (int)customer.statusLicence.licenceType;
    //            }

    //            var parametersStatus = new
    //            {
    //                Type = type,
    //            };

    //            var statusQuery = "SELECT [id]" +
    //                          "FROM [ReportingSystem].[dbo].[StatusLicence]" +
    //                          "Where Type = @Type";
    //            var statusResult = await database.QueryAsync<Guid>(statusQuery, parametersStatus);

    //            Guid status = Guid.Empty;

    //            if (statusResult.Any())
    //            {
    //                status = statusResult.First();
    //            }

    //            if (customer != null && customer.password != null)
    //            {
    //                var parameters = new
    //                {
    //                    Id = customer.id,
    //                    FirstName = customer.firstName,
    //                    SecondName = customer.secondName,
    //                    ThirdName = customer.thirdName,
    //                    StatusLicenceId = status,
    //                    ConfigureId = configureParameters.Id,
    //                    Phone = customer.phone,
    //                    Email = customer.email,
    //                    Password = EncryptionHelper.Encrypt(customer.password),
    //                    EndTimeLicense = customer.endTimeLicense,
    //                    DateRegistration = customer.dateRegistration,
    //                };

    //                await database.ExecuteAsync(customersQuery, parameters);
    //            }


    //        }
    //    }

    //    public async Task Company(CompanyModel company, Guid customerId)
    //    {
    //        using (var database = Context.ConnectToSQL)
    //        {
    //            var query = "INSERT INTO [dbo].[Companies]" +
    //                "([Id],[CustomerId],[Name],[Address],[Code],[Actions],[StatusWeb],[Phone],[Email],[StatutCapital],[RegistrationDate],[Status],[Chief])" +
    //                "VALUES (@Id, @CustomerId, @Name, @Address, @Code, @Actions, @StatusWeb, @Phone, @Email, @StatutCapital, @RegistrationDate, @Status, @Chief)";

    //            int type = -1;
    //            if (company != null && company.status != null && company.status != null)
    //            {
    //                company.status.companyStatusName = Enums.CompanyStatus.Actual.GetDisplayName();
    //                company.status.companyStatusType = Enums.CompanyStatus.Actual;
    //                type = (int)company.status.companyStatusType;

    //            }

    //            var parametersStatus = new
    //            {
    //                Type = type,
    //            };

    //            var statusQuery = "SELECT [id]" +
    //                          "FROM [ReportingSystem].[dbo].[CompanyStatus]" +
    //                          "Where Type = @Type";
    //            var statusResult = await database.QueryAsync<Guid>(statusQuery, parametersStatus);

    //            Guid status = Guid.Empty;

    //            if (statusResult.Any())
    //            {
    //                status = statusResult.First();
    //            }

    //            if (company != null)
    //            {
    //                Guid? chief = Guid.Empty;
    //                if (company.chief != null)
    //                {
    //                    chief = company.chief.id;
    //                }
    //                var parameters = new
    //                {
    //                    Id = company.id,
    //                    CustomerId = customerId,
    //                    Name = company.name,
    //                    Address = company.address,
    //                    Code = company.code,
    //                    Actions = company.actions,
    //                    StatusWeb = company.statusWeb,
    //                    Phone = company.phone,
    //                    Email = company.email,
    //                    StatutCapital = company.statutCapital,
    //                    RegistrationDate = company.registrationDate,
    //                    Status = status,
    //                    Chief = chief
    //                };
    //                await database.ExecuteAsync(query, parameters);
    //            }
    //        }
    //    }
    //}
}
