using Dapper;
using ReportingSystem.Enums;
using System.Data.Entity.Migrations.Model;

namespace ReportingSystem.Data.SQL
{
    public class CreateTables
    {
        private async Task CreateTableAndInsertIfNotExistsAsync(string tableName, Func<Task> createTableAsync, Func<Task>? insertDataAsync = null)
        {
            if (!await new TablesIsExist().TableExistsAsync(tableName))
            {
                await createTableAsync();
                if (insertDataAsync != null)
                {
                    await insertDataAsync();
                }
            }
        }

        public async Task Enums()
        {
            await CreateTableAndInsertIfNotExistsAsync("StatusLicence", () => new CreateTable().StatusLicence(), () => new InsertData().StatusLicence());
            await CreateTableAndInsertIfNotExistsAsync("AuthorizeStatus", () => new CreateTable().AuthorizeStatus(), () => new InsertData().AuthorizeStatus());
            await CreateTableAndInsertIfNotExistsAsync("AuthorizeHistory", () => new CreateTable().AuthorizeHistory());
            await CreateTableAndInsertIfNotExistsAsync("CompanyStatus", () => new CreateTable().CompanyStatus(), () => new InsertData().CompanyStatus());
            await CreateTableAndInsertIfNotExistsAsync("EmployeeRolStatus", () => new CreateTable().EmployeeRolStatus(), () => new InsertData().EmployeeRolStatus());
            await CreateTableAndInsertIfNotExistsAsync("EmployeeStatus", () => new CreateTable().EmployeeStatus(), () => new InsertData().EmployeeStatus());
            await CreateTableAndInsertIfNotExistsAsync("ProjectStatus", () => new CreateTable().ProjectStatus(), () => new InsertData().ProjectStatus());
        }

        public async Task Customers()
        {
            await CreateTableAndInsertIfNotExistsAsync("Configure", () => new CreateTable().Configure());
            await CreateTableAndInsertIfNotExistsAsync("HistoryOperations", () => new CreateTable().HistoryOperations());
            await CreateTableAndInsertIfNotExistsAsync("EmployeePosition", () => new CreateTable().EmployeePosition());
            await CreateTableAndInsertIfNotExistsAsync("HolidayDate", () => new CreateTable().HolidayDate());
            await CreateTableAndInsertIfNotExistsAsync("HospitalDate", () => new CreateTable().HospitalDate());
            await CreateTableAndInsertIfNotExistsAsync("AssignmentDate", () => new CreateTable().AssignmentDate());
            await CreateTableAndInsertIfNotExistsAsync("TaketimeoffDate", () => new CreateTable().TaketimeoffDate());
            await CreateTableAndInsertIfNotExistsAsync("Employees", () => new CreateTable().Employees());
            await CreateTableAndInsertIfNotExistsAsync("Projects", () => new CreateTable().Projects());
            await CreateTableAndInsertIfNotExistsAsync("Companies", () => new CreateTable().Companies());
            await CreateTableAndInsertIfNotExistsAsync("Customers", () => new CreateTable().Customers());
            await CreateTableAndInsertIfNotExistsAsync("CompanyRolls", () => new CreateTable().CompanyRolls());
        }

        public async Task Administrators()
        {
            await CreateTableAndInsertIfNotExistsAsync("Administrators", () => new CreateTable().Administrators());
        }
    }
}
