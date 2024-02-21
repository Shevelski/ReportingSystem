﻿using ReportingSystem.Models.Settings;
using System.Data.SqlClient;

namespace ReportingSystem.Data.SQL
{
    public class Database
    {
        public static void Create(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {

            SqlConnection connection = new();
            if (isUseDatabaseCredential.Equals("True"))
            {
                connection = new SqlConnection($"Server={serverName};User Id={login};Password={password}");
            }
            else
            {
                connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;");
            }
            connection.Open();


            // SQL-запит для створення бази даних
            string createDatabaseQuery = $"CREATE DATABASE {databaseName};";

            // Виконання SQL-запиту
            using SqlCommand command = new(createDatabaseQuery, connection);
            command.ExecuteNonQuery();
        }
        public static void Drop(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            SqlConnection connection = new();
            if (isUseDatabaseCredential.Equals("True"))
            {
                connection = new SqlConnection($"Server={serverName};User Id={login};Password={password}");
            }
            else
            {
                connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;");
            }

            connection.Open();

            // Перевірка наявності бази даних
            string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}';";
            using (SqlCommand checkDatabaseCommand = new(checkDatabaseQuery, connection))
            {
                int databaseCount = (int)checkDatabaseCommand.ExecuteScalar();
                if (databaseCount == 0)
                {
                    Console.WriteLine($"Database '{databaseName}' does not exist.");
                    return;
                }
            }

            // Встановлення бази даних в режим SINGLE_USER
            string stopConnectionsQuery = $@"USE master;
                                            ALTER DATABASE [{databaseName}]
                                            SET SINGLE_USER
                                            WITH ROLLBACK IMMEDIATE;";

            using (SqlCommand stopConnectionsCommand = new(stopConnectionsQuery, connection))
            {
                stopConnectionsCommand.ExecuteNonQuery();
            }

            // SQL-запит для видалення бази даних
            string dropDatabaseQuery = $"USE master; DROP DATABASE {databaseName};";

            // Виконання SQL-запиту
            using SqlCommand command = new(dropDatabaseQuery, connection);
            command.ExecuteNonQuery();
        }


        public static bool IsExist(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            string con = "";
            if (isUseDatabaseCredential.Equals("False"))
            {
                con = $"Server={serverName};Trusted_Connection=True;Database = {databaseName}";
            }
            else
            {
                con = $"Server={serverName};Database={databaseName};User Id={login};Password={password}";
            }

            SqlConnection connection = new SqlConnection(con);
            connection.Open();
            string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
            using SqlCommand checkCommand = new(checkDatabaseQuery, connection);
            int existingDatabaseCount = (int)checkCommand.ExecuteScalar();
            if (existingDatabaseCount > 0)
            {
                Console.WriteLine($"База даних '{databaseName}' вже існує.");
                return true;
            }
            return false;
        }

        public static bool IsServerAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            try
            {
                SqlConnection connection = new();
                if (isUseDatabaseCredential.Equals("True"))
                {
                    connection = new SqlConnection($"Server={serverName};User Id={login};Password={password}");
                }
                else
                {
                    connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;");
                }
                using (connection)
                {
                    connection.Open();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool IsDatabaseAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            try
            {
                SqlConnection connection = new();
                if (isUseDatabaseCredential.Equals("True"))
                {
                    connection = new SqlConnection($"Server={serverName};Database={databaseName};User Id={login};Password={password}");
                }
                else
                {
                    connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;Database = {databaseName}");
                }

                connection.Open();

                // Перевірка на існування бази даних на конкретному сервері
                string checkDatabaseQuery = $"SELECT COUNT(*) FROM [{serverName}].master.sys.databases WHERE name = '{databaseName}'";
                using SqlCommand checkCommand = new SqlCommand(checkDatabaseQuery, connection);
                int existingDatabaseCount = (int)checkCommand.ExecuteScalar();

                if (existingDatabaseCount > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public async Task<bool> IsTablesAvailable(string serverName, string databaseName, string isUseDatabaseCredential, string login, string password)
        {
            try
            {
                SqlConnection connection = new();
                if (isUseDatabaseCredential.Equals("True"))
                {
                    connection = new SqlConnection($"Server={serverName};Database={databaseName};User Id={login};Password={password}");
                }
                else
                {
                    connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;Database = {databaseName}");
                }

                connection.Open();

                TablesIsExist tablesChecker = new TablesIsExist();

                var results = await Task.WhenAll(
                    tablesChecker.AdministratorsAsync(),
                    //tablesChecker.ConfigurationAsync(),
                    tablesChecker.CompanyRollsAsync(),
                    tablesChecker.ConfigureAsync(),
                    tablesChecker.StatusLicenceAsync(),
                    tablesChecker.ProjectsAsync(),
                    tablesChecker.AuthorizeStatusAsync(),
                    tablesChecker.AuthorizeHistoryAsync(),
                    tablesChecker.CompanyStatusAsync(),
                    tablesChecker.EmployeeRolStatusAsync(),
                    tablesChecker.EmployeeStatusAsync(),
                    tablesChecker.ProjectStatusAsync(),
                    tablesChecker.HistoryOperationsAsync(),
                    tablesChecker.EmployeePositionAsync(),
                    //tablesChecker.EmployeeRolAsync(),
                    tablesChecker.HolidayDateAsync(),
                    tablesChecker.HospitalDateAsync(),
                    tablesChecker.AssignmentDateAsync(),
                    tablesChecker.TaketimeoffDateAsync(),
                    tablesChecker.EmployeesAsync(),
                    tablesChecker.CompaniesAsync(),
                    tablesChecker.CustomersAsync()
                );

                Console.WriteLine("Results for each table:");
                for (int i = 0; i < results.Length; i++)
                {
                    Console.WriteLine($"Table {i + 1}: {results[i]}");
                }

                return results.All(result => result);
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public async Task<bool> IsTablesAvailable(string serverName, string databaseName, string login, string password)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};Database={databaseName};User Id={login};Password={password};");
                connection.Open();

                TablesIsExist tablesChecker = new TablesIsExist();

                var results = await Task.WhenAll(
                    tablesChecker.AdministratorsAsync(),
                    tablesChecker.ConfigurationAsync(),
                    tablesChecker.CompanyRollsAsync(),
                    tablesChecker.ConfigureAsync(),
                    tablesChecker.StatusLicenceAsync(),
                    tablesChecker.ProjectsAsync(),
                    tablesChecker.AuthorizeStatusAsync(),
                    tablesChecker.AuthorizeHistoryAsync(),
                    tablesChecker.CompanyStatusAsync(),
                    tablesChecker.EmployeeRolStatusAsync(),
                    tablesChecker.EmployeeStatusAsync(),
                    tablesChecker.ProjectStatusAsync(),
                    tablesChecker.HistoryOperationsAsync(),
                    tablesChecker.EmployeePositionAsync(),
                    tablesChecker.EmployeeRolAsync(),
                    tablesChecker.HolidayDateAsync(),
                    tablesChecker.HospitalDateAsync(),
                    tablesChecker.AssignmentDateAsync(),
                    tablesChecker.TaketimeoffDateAsync(),
                    tablesChecker.EmployeesAsync(),
                    tablesChecker.CompaniesAsync(),
                    tablesChecker.CustomersAsync()
                );

                Console.WriteLine("Results for each table:");
                for (int i = 0; i < results.Length; i++)
                {
                    Console.WriteLine($"Table {i + 1}: {results[i]}");
                }

                return results.All(result => result);
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
