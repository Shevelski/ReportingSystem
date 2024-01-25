using System.Data.SqlClient;

namespace ReportingSystem.Data.SQL
{
    public class Database
    {
        public static void Create(string serverName, string databaseName)
        {
            //if (!IsServerAvailable(serverName))
            //{
            //    serverName = Context.serverNameDefault;
            //}
            //if (!IsDatabaseAvailable(serverName, databaseName))
            //{
            //    serverName = Context.dbNameDefault;
            //}
            using SqlConnection connection = new($"Server={serverName};Trusted_Connection=True;");
            connection.Open();

            // SQL-запит для створення бази даних
            string createDatabaseQuery = $"CREATE DATABASE {databaseName};";

            // Виконання SQL-запиту
            using SqlCommand command = new(createDatabaseQuery, connection);
            command.ExecuteNonQuery();
        }

        public static bool IsExist(string serverName, string databaseName)
        {
            using SqlConnection connection = new($"Server={serverName};Trusted_Connection=True;");
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

        public static bool IsExist(string serverName, string databaseName, string login, string password)
        {

            using SqlConnection connection = new SqlConnection($"Server={serverName};User Id={login};Password={password};");
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

        public static bool IsServerAvailable(string serverName)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;");
                connection.Open();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool IsServerAvailable(string serverName, string login, string password)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};User Id={login};Password={password};");
                connection.Open();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool IsDatabaseAvailable(string serverName, string databaseName)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};Trusted_Connection=True;");
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

        public static bool IsDatabaseAvailable(string serverName, string databaseName, string login, string password)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};User Id={login};Password={password};");
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
        public async Task<bool> IsTablesAvailable(string serverName, string databaseName)
        {
            try
            {
                using SqlConnection connection = new SqlConnection($"Server={serverName};Database={databaseName};Trusted_Connection=True;");
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

        //public async Task SetConnectionString(string serverName, string databaseName)
        //{

        //}

        //public async Task SetConnectionString(string serverName, string databaseName, string login, string password)
        //{
        //}

    }
}
