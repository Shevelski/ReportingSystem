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
            try
            {
                await CreateTableAndInsertIfNotExistsAsync("Configure", () => new CreateTable().Configure());
                await CreateTableAndInsertIfNotExistsAsync("HistoryOperations", () => new CreateTable().HistoryOperations());
                await CreateTableAndInsertIfNotExistsAsync("EmployeePosition", () => new CreateTable().EmployeePosition());
                await CreateTableAndInsertIfNotExistsAsync("HolidayDate", () => new CreateTable().HolidayDate());
                await CreateTableAndInsertIfNotExistsAsync("HospitalDate", () => new CreateTable().HospitalDate());
                await CreateTableAndInsertIfNotExistsAsync("AssignmentDate", () => new CreateTable().AssignmentDate());
                await CreateTableAndInsertIfNotExistsAsync("TaketimeoffDate", () => new CreateTable().TaketimeoffDate());

                await CreateTableAndInsertIfNotExistsAsync("CompanyCategory3", () => new CreateTable().CompanyCategory3());
                await CreateTableAndInsertIfNotExistsAsync("CompanyCategory2", () => new CreateTable().CompanyCategory2());
                await CreateTableAndInsertIfNotExistsAsync("CompanyCategory1", () => new CreateTable().CompanyCategory1());
                await CreateTableAndInsertIfNotExistsAsync("CompanyCategory0", () => new CreateTable().CompanyCategory0());

                await CreateTableAndInsertIfNotExistsAsync("Steps", () => new CreateTable().Steps());
                await CreateTableAndInsertIfNotExistsAsync("ProjectPositions", () => new CreateTable().ProjectPositions());
                await CreateTableAndInsertIfNotExistsAsync("ProjectSteps", () => new CreateTable().ProjectSteps());
                await CreateTableAndInsertIfNotExistsAsync("ProjectMembers", () => new CreateTable().ProjectMembers());
                await CreateTableAndInsertIfNotExistsAsync("StepPositions", () => new CreateTable().StepPositions());

                await CreateTableAndInsertIfNotExistsAsync("StepMembers", () => new CreateTable().StepMembers());
                await CreateTableAndInsertIfNotExistsAsync("Reports", () => new CreateTable().Reports());
                await CreateTableAndInsertIfNotExistsAsync("Employees", () => new CreateTable().Employees());

                await CreateTableAndInsertIfNotExistsAsync("Projects", () => new CreateTable().Projects());
                await CreateTableAndInsertIfNotExistsAsync("Companies", () => new CreateTable().Companies());
                await CreateTableAndInsertIfNotExistsAsync("Customers", () => new CreateTable().Customers());
                await CreateTableAndInsertIfNotExistsAsync("CompanyRolls", () => new CreateTable().CompanyRolls());
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Administrators()
        {
            await CreateTableAndInsertIfNotExistsAsync("Administrators", () => new CreateTable().Administrators());
        }
    }
}
