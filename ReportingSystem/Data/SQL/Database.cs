using System.Data.SqlClient;

namespace ReportingSystem.Data.SQL
{
    public class Database
    {
        public static void Create(string masterConnectionString, string databaseName)
        {
            using SqlConnection connection = new(masterConnectionString);
            connection.Open();

            // SQL-запит для створення бази даних
            string createDatabaseQuery = $"CREATE DATABASE {databaseName};";

            // Виконання SQL-запиту
            using SqlCommand command = new(createDatabaseQuery, connection);
            command.ExecuteNonQuery();
        }

        public static bool IsExist(string masterConnectionString, string databaseName)
        {
            using SqlConnection connection = new(masterConnectionString);
            connection.Open();

            // Перевірка на існування бази даних
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
    }
}
