using Dapper;

namespace ReportingSystem.Data
{
    public class CreateTableForCustomers
    {
        public async Task StatusLicence()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE StatusLicence ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task AuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE AuthorizeStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(200) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE CompanyStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeRolStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task ProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE ProjectStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Status()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Status ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Configure()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Configure(" +
                    "id UNIQUEIDENTIFIER PRIMARY KEY," +
                    "param1 INT NOT NULL," +
                    "param2 INT NOT NULL," +
                    "param3 INT NOT NULL," +
                    "param4 INT NOT NULL," +
                    "param5 NVARCHAR(60) NOT NULL," +
                    "param6 NVARCHAR(60) NOT NULL," +
                    "param7 NVARCHAR(60) NOT NULL," +
                    "param8 NVARCHAR(60) NOT NULL)," +
                    "param9 UNIQUEIDENTIFIER NOT NULL," +
                    "param10 UNIQUEIDENTIFIER NOT NULL," +
                    "param11 UNIQUEIDENTIFIER NOT NULL," +
                    "param12 UNIQUEIDENTIFIER NOT NULL)";

                await database.ExecuteAsync(query);
            }
        }



        

    }
}
