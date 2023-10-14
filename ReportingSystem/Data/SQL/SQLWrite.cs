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
        public async Task EditCompany(string[] ar)
        {
            var CompanyId = ar[0];
            var CustomerId = ar[6];
            var Name = ar[1];
            var Address = ar[2];
            var Actions = ar[3];
            var Phone = ar[4];  
            var Email = ar[5];

            using (var database = Context.ConnectToSQL)
            {
                var query = "UPDATE [dbo].[Companies]" +
                            "SET [Name] = @Name" +
                            ",[Address] = @Address" +
                            ",[Actions] = @Actions" +
                            ",[Phone] = @Phone" +
                            ",[Email] = @Email " +
                            "WHERE Id = @Id AND CustomerId = @CustomerId";
                var para = new
                {
                    Id = CompanyId,
                    CustomerId = CustomerId,
                    Name = Name,
                    Address = Address,
                    Actions = Actions,
                    Phone = Phone,
                    Email = Email
                };
                var result = await database.QueryAsync(query, para);
            }
        }
        public async Task ArchiveCompany(string[] ar)
        {
            var CompanyId = ar[0];
            var CustomerId = ar[1];

            CompanyStatusModel companyStatusModel = new CompanyStatusModel();
            companyStatusModel.companyStatusType = CompanyStatus.Archive;
            companyStatusModel.companyStatusName = companyStatusModel.companyStatusType.GetDisplayName();
            var companyStatusId = await new SQLRead().GetCompanyStatusId(companyStatusModel);

            using (var database = Context.ConnectToSQL)
            {
                var query = "UPDATE [dbo].[Companies]" +
                            "SET [Status] = @Status " +
                            "WHERE Id = @Id AND CustomerId = @CustomerId";
                var para = new
                {
                    Id = CompanyId,
                    CustomerId = CustomerId,
                    Status = companyStatusId
                };
                var result = await database.QueryAsync(query, para);
            }
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

            var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    customer.companies.Remove(company);
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
                name = ar[0],
                code = ar[1],
                address = ar[2],
                actions = ar[3],
                phone = ar[4],
                email = ar[5],
                registrationDate = DateTime.Today,
                rolls = DefaultEmployeeRolls.Get(),
                positions = new List<EmployeePositionModel>(),
                employees = new List<EmployeeModel>(),
                status = new CompanyStatusModel
                {
                    companyStatusType = CompanyStatus.Project,
                    companyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (customers != null)
            {
                var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

                if (customer != null && customer.companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        firstName = customer.firstName,
                        secondName = customer.secondName,
                        thirdName = customer.thirdName,
                        emailWork = customer.email
                    };

                    company.chief = chief;
                    customer.companies.Add(company);
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }
    }
}
