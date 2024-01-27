using System.Data.SqlClient;

namespace ReportingSystem.Data.SQL
{
    public class ClearDatabase
    {

        public static async Task ClearTables(string[] ar)
        {
            string serverName = ar[0];
            string databaseName = ar[1];
            string login = ar[2];
            string password = ar[3];
            string connectionString = "";

            if (login == "" || login == null || password == "" || password == null)
            {
                connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True";
            }
            else
            {
                connectionString = $"Data Source={serverName};Initial Catalog={databaseName};User Id={login};Password={password};";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    DECLARE @tableName NVARCHAR(MAX)
                    DECLARE tableCursor CURSOR FOR
                    SELECT table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    
                    OPEN tableCursor
                    
                    FETCH NEXT FROM tableCursor INTO @tableName
                    
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX)
                        SET @sql = 'USE ' + @databaseName + '; DROP TABLE ' + @tableName
                        EXEC(@sql)
                    
                        FETCH NEXT FROM tableCursor INTO @tableName
                    END
                    
                    CLOSE tableCursor
                    DEALLOCATE tableCursor
                ";

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
