using Bogus.DataSets;
using Dapper;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.ComponentModel.Design;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace ReportingSystem.Data
{
    public class InsertData
    {
        public async Task StatusLicence()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[StatusLicence] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (LicenceType licence in Enum.GetValues(typeof(LicenceType)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)licence,
                        Name = licence.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task AuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[AuthorizeStatus] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.AuthorizeStatus authorize in Enum.GetValues(typeof(Enums.AuthorizeStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)authorize,
                        Name = authorize.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task CompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[CompanyStatus] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.CompanyStatus company in Enum.GetValues(typeof(Enums.CompanyStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)company,
                        Name = company.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task EmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[EmployeeRolStatus] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.EmployeeRolStatus rol in Enum.GetValues(typeof(Enums.EmployeeRolStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)rol,
                        Name = rol.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task EmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[EmployeeStatus] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.EmployeeStatus employee in Enum.GetValues(typeof(Enums.EmployeeStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)employee,
                        Name = employee.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task ProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[ProjectStatus] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.ProjectStatus project in Enum.GetValues(typeof(Enums.ProjectStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)project,
                        Name = project.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task Status()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[Status] " +
                            "([Id], [Type], [Name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.Status status in Enum.GetValues(typeof(Enums.Status)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)status,
                        Name = status.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task EmployeePosition(EmployeePositionModel position, Guid customerId, Guid companyId, int type)
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[EmployeePosition]" +
                            "([Id],[CustomerId],[CompanyId],[Type],[Name])" +
                            "VALUES (@Id, @CustomerId, @CompanyId, @Type, @Name)";

                var parameters = new
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    CompanyId = companyId,
                    Type = type,
                    Name = position.namePosition
                };

                await database.ExecuteAsync(query, parameters);
                
            }
        }

        public async Task Employee(EmployeeModel employee)
        {
            using (var database = Context.Connect)
            {
                var checkQuery1 = "SELECT COUNT(*) " +
                                 "FROM[ReportingSystem].[dbo].[Administrators]" +
                                 "Where EmailWork = @EmailWork";
                var checkQuery2 = "SELECT COUNT(*) " +
                                 "FROM[ReportingSystem].[dbo].[Employee]" +
                                 "Where EmailWork = @EmailWork";
                var paraCheck = new
                {
                    EmailWork = employee.emailWork,
                };

                var checkResult1 = await database.QueryAsync<int>(checkQuery1, paraCheck);
                var checkResult2 = await database.QueryAsync<int>(checkQuery2, paraCheck);

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
                    var statusResult = await database.QueryAsync<Guid>(statusQuery, paraStatus);

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
                    var rolResult = await database.QueryAsync<Guid>(rolQuery, paraStatus1);

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
                    var positionResult = await database.QueryAsync<Guid>(statusPosition, paraPosition);

                    Guid statusPos = Guid.Empty;

                    if (positionResult.Any())
                    {
                        statusPos = positionResult.First();
                    }


                    var query = "INSERT INTO [dbo].[Employee]" +
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
                        Password = EncryptionHelper.Encrypt(employee.password),
                        Salary = employee.salary,
                        AddSalary = employee.addSalary,
                        Status = status,
                        BirthDate = employee.birthDate,
                        WorkStartDate = employee.workStartDate,
                        WorkEndDate = employee.workEndDate,
                        Position= statusPos,
                        Rol = rol
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }

        public async Task Developer(EmployeeModel employee)
        {
            using (var database = Context.Connect)
            {
                var checkQuery1 = "SELECT COUNT(*) " +
                                 "FROM[ReportingSystem].[dbo].[Administrators]" +
                                 "Where EmailWork = @EmailWork";
                var checkQuery2 = "SELECT COUNT(*) " +
                                 "FROM[ReportingSystem].[dbo].[Employee]" +
                                 "Where EmailWork = @EmailWork";
                var paraCheck = new
                {
                    EmailWork = employee.emailWork,
                };

                var checkResult1 = await database.QueryAsync<int>(checkQuery1, paraCheck);
                var checkResult2 = await database.QueryAsync<int>(checkQuery2, paraCheck);

                if (checkResult1.First().Equals(0) && checkResult2.First().Equals(0))
                {
                    var statusQuery = "SELECT [id]" +
                                  "FROM [ReportingSystem].[dbo].[EmployeeStatus]" +
                                  "Where Type = 1";
                    var statusResult = await database.QueryAsync<Guid>(statusQuery);

                    Guid status = Guid.Empty;

                    if (statusResult.Any())
                    {
                        status = statusResult.First();
                    }

                    var rolQuery = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[EmployeeRolStatus]" +
                              "Where Type = 1";
                    var rolResult = await database.QueryAsync<Guid>(rolQuery);

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
                        Password = EncryptionHelper.Encrypt(employee.password),
                        Status = status,
                        BirthDate = employee.birthDate,
                        Rol = rol
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }

        public async Task Customer(CustomerModel customer)
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[Customers]" +
                    "([Id],[FirstName],[SecondName],[ThirdName],[StatusLicenceId],[Phone],[Email],[Password],[EndTimeLicense],[DateRegistration])" +
                    "VALUES (@Id, @FirstName, @SecondName, @ThirdName, @StatusLicenceId, @Phone, @Email, @Password, @EndTimeLicense, @DateRegistration)";

                int type = -1;
                if (customer != null && customer.statusLicence != null)
                {
                    //customer.statusLicence.licenceName = LicenceType.Test.GetDisplayName();
                    //customer.statusLicence.licenceType = LicenceType.Test;
                    type = (int)customer.statusLicence.licenceType;

                }

                var parametersStatus = new
                {
                    Type = type,
                };

                var statusQuery = "SELECT [id]" +
                              "FROM [ReportingSystem].[dbo].[StatusLicence]" +
                              "Where Type = @Type";
                var statusResult = await database.QueryAsync<Guid>(statusQuery, parametersStatus);

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
                        Phone = customer.phone,
                        Email = customer.email,
                        Password = EncryptionHelper.Encrypt(customer.password),
                        EndTimeLicense = customer.endTimeLicense,
                        DateRegistration = customer.dateRegistration,
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        
        public async Task Company(CompanyModel company, Guid customerId)
        {
            using (var database = Context.Connect)
            {
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
                var statusResult = await database.QueryAsync<Guid>(statusQuery, parametersStatus);

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
                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
    }
}
