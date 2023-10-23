﻿using Dapper;

namespace ReportingSystem.Data.SQL
{
    public class TablesIsExist
    {
        public async Task<bool> TableExistsAsync(string tableName)
        {
            using (var database = Context.ConnectToSQL)
            {
                var tableExistsQuery = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
                var tableExists = await database.QueryFirstOrDefaultAsync<int>(tableExistsQuery);

                return tableExists == 1;
            }
        }

        public async Task<bool> AdministratorsAsync() => await TableExistsAsync("Administrators");
        public async Task<bool> ConfigurationAsync() => await TableExistsAsync("Configuration");
        public async Task<bool> CompanyRollsAsync() => await TableExistsAsync("CompanyRolls");
        public async Task<bool> ConfigureAsync() => await TableExistsAsync("Configure");
        public async Task<bool> StatusLicenceAsync() => await TableExistsAsync("StatusLicence");
        public async Task<bool> ProjectsAsync() => await TableExistsAsync("Projects");
        public async Task<bool> AuthorizeStatusAsync() => await TableExistsAsync("AuthorizeStatus");
        public async Task<bool> AuthorizeHistoryAsync() => await TableExistsAsync("AuthorizeHistory");
        public async Task<bool> CompanyStatusAsync() => await TableExistsAsync("CompanyStatus");
        public async Task<bool> EmployeeRolStatusAsync() => await TableExistsAsync("EmployeeRolStatus");
        public async Task<bool> EmployeeStatusAsync() => await TableExistsAsync("EmployeeStatus");
        public async Task<bool> ProjectStatusAsync() => await TableExistsAsync("ProjectStatus");
        public async Task<bool> HistoryOperationsAsync() => await TableExistsAsync("HistoryOperations");
        public async Task<bool> EmployeePositionAsync() => await TableExistsAsync("EmployeePosition");
        public async Task<bool> EmployeeRolAsync() => await TableExistsAsync("EmployeeRol");
        public async Task<bool> HolidayDateAsync() => await TableExistsAsync("HolidayDate");
        public async Task<bool> HospitalDateAsync() => await TableExistsAsync("HospitalDate");
        public async Task<bool> AssignmentDateAsync() => await TableExistsAsync("AssignmentDate");
        public async Task<bool> TaketimeoffDateAsync() => await TableExistsAsync("TaketimeoffDate");
        public async Task<bool> EmployeesAsync() => await TableExistsAsync("Employees");
        public async Task<bool> CompaniesAsync() => await TableExistsAsync("Companies");
        public async Task<bool> CustomersAsync() => await TableExistsAsync("Customers");
    }
}
