using Dapper;
using ReportingSystem.Enums;

namespace ReportingSystem.Data
{
    public class CreateTables
    {
        public async Task CreateTableCustomers()
        {
           
            if (!new CheckTables().IsExistsStatusLicence())
            {
                await new CreateTables().CreateTableStatusLicence();
                await new InsertData().StatusLicence();
                await new CreateTables().CreateTableAuthorizeStatus();
                await new InsertData().AuthorizeStatus();
                await new CreateTables().CreateTableCompanyStatus();
                await new InsertData().CompanyStatus();
                await new CreateTables().CreateTableEmployeeRolStatus();
                await new InsertData().EmployeeRolStatus();
                await new CreateTables().CreateTableEmployeeStatus();
                await new InsertData().EmployeeStatus();
                await new CreateTables().CreateTableProjectStatus();
                await new InsertData().ProjectStatus();
                await new CreateTables().CreateTableStatus();
                await new InsertData().Status();


                
            }

                


                //database.ExecuteAsync("IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Customers')" +
                //    " BEGIN " +
                //    " CREATE TABLE Customers(" +
                //    " id INT PRIMARY KEY," +
                //    " col1 NVARCHAR(60) NOT NULL" +
                //    ");" +
                //    " END").Wait();
        }
        public void CreateTableAdministrators()
        {

        }
        public void CreateTableConfiguration()
        {

        }
        public async Task CreateTableStatusLicence()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE StatusLicence ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableAuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE AuthorizeStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(200) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableCompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE CompanyStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableEmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeRolStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableEmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE ProjectStatus ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CreateTableStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Status ( " +
                    "id UNIQUEIDENTIFIER PRIMARY KEY, type INT NOT NULL, name NVARCHAR(60) NOT NULL );";

                await database.ExecuteAsync(query);
            }
        }
        
    }
}
