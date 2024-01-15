using Bogus;
using Bogus.DataSets;
using Dapper;
using Newtonsoft.Json;
using ReportingSystem.Data.JSON;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Collections.Generic;
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
            var query = "UPDATE [dbo].[Companies]" +
                        "SET [Name] = @Name" +
                        ",[Address] = @Address" +
                        ",[Actions] = @Actions" +
                        ",[Phone] = @Phone" +
                        ",[Email] = @Email " +
                        "WHERE Id = @Id AND CustomerId = @CustomerId";
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
            var query = "UPDATE [dbo].[Companies]" +
                        "SET [Status] = @Status " +
                        "WHERE Id = @Id AND CustomerId = @CustomerId";
            var para = new
            {
                Id = ar[0],
                CustomerId = ar[1],
                Status = companyStatusId
            };
            var result = await database.QueryAsync(query, para);
        }

        public async Task DeleteCompany(string[] ar)
        {

            var query = "DELETE FROM [dbo].[EmployeePosition] " +
            "WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId " +
            "DELETE FROM [dbo].[Employees] " +
            "WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId " +
            "DELETE FROM [dbo].[CompanyRolls] " +
            "WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId " +
            "DELETE FROM [dbo].[Companies] " +
            "WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId";

            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    customer.Companies.Remove(company);
                }
            }
        }
        public async Task<CompanyModel?> CreateCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return null;
            }

            var company = new CompanyModel
            {
                Name = ar[0],
                Code = ar[1],
                Address = ar[2],
                Actions = ar[3],
                Phone = ar[4],
                Email = ar[5],
                RegistrationDate = DateTime.Today,
                Rolls = DefaultEmployeeRolls.Get(),
                Positions = [],
                Employees = [],
                Status = new CompanyStatusModel
                {
                    companyStatusType = CompanyStatus.Project,
                    companyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (customers != null)
            {
                var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

                if (customer != null && customer.Companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        firstName = customer.FirstName,
                        secondName = customer.SecondName,
                        thirdName = customer.ThirdName,
                        emailWork = customer.Email
                    };

                    company.Chief = chief;
                    customer.Companies.Add(company);
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }
    }
}
