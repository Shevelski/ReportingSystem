using Dapper;

namespace ReportingSystem.Data
{
    public class CreateTable
    {
        // --------------- enums --------------------------
        public async Task StatusLicence()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE StatusLicence ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task AuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE AuthorizeStatus ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task CompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE CompanyStatus ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeRolStatus ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeStatus ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task ProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE ProjectStatus ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Status()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Status ( " +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY, " +
                                "Type INT NOT NULL, " +
                                "Name NVARCHAR(MAX) NOT NULL " +
                            ");";

                await database.ExecuteAsync(query);
            }
        }

        //----------------------------------------------------------------
        public async Task Configure()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Configure(" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "Param1 INT NOT NULL," +
                                "Param2 INT NOT NULL," +
                                "Param3 INT NOT NULL," +
                                "Param4 INT NOT NULL," +
                                "Param5 NVARCHAR(MAX) NOT NULL," +
                                "Param6 NVARCHAR(MAX) NOT NULL," +
                                "Param7 NVARCHAR(MAX) NOT NULL," +
                                "Param8 NVARCHAR(MAX) NOT NULL," +
                                "Param9 UNIQUEIDENTIFIER NOT NULL," +
                                "Param10 UNIQUEIDENTIFIER NOT NULL," +
                                "Param11 UNIQUEIDENTIFIER NOT NULL," +
                                "Param12 UNIQUEIDENTIFIER NOT NULL" +
                             ")";

                await database.ExecuteAsync(query);
            }
        }

        public async Task HistoryOperations()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE HistoryOperations (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "CustomerId UNIQUEIDENTIFIER," +
                                "DateChange DATETIME," +
                                "OldEndDateLicence DATETIME," +
                                "NewEndDateLicence DATETIME," +
                                "OldStatus UNIQUEIDENTIFIER," +
                                "NewStatus UNIQUEIDENTIFIER," +
                                "Price FLOAT," +
                                "Period NVARCHAR(MAX)," +
                                "NameOperation NVARCHAR(MAX)," +
                                "FOREIGN KEY (OldStatus) REFERENCES StatusLicence(id)," +
                                "FOREIGN KEY (NewStatus) REFERENCES StatusLicence(id)" +
                            ")";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeePosition()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeePosition (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "CustomerId UNIQUEIDENTIFIER," +
                                "CompanyId UNIQUEIDENTIFIER," +
                                "Type INT NOT NULL," +
                                "Name NVARCHAR(MAX) NOT NULL" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task EmployeeRol()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE EmployeeRol (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "Type UNIQUEIDENTIFIER NOT NULL," +
                                "Name NVARCHAR(MAX) NOT NULL," +
                                "FOREIGN KEY(Type) REFERENCES EmployeePosition(id)" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Project()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Project (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "CustomerId UNIQUEIDENTIFIER," +
                                "CompanyId UNIQUEIDENTIFIER," +
                                "Name NVARCHAR(MAX) NOT NULL," +
                                "Description NVARCHAR(MAX)," +
                                "ProjectCostsForCompany FLOAT," +
                                "ProjectCostsForCustomer FLOAT," +
                                "StartDate DATETIME," +
                                "EndDate DATETIME," +
                                "Status UNIQUEIDENTIFIER," +
                                "Head UNIQUEIDENTIFIER," +
                                "CategoryModel UNIQUEIDENTIFIER," +
                                "CategoryModel2 UNIQUEIDENTIFIER, " +
                                "CategoryModel3 UNIQUEIDENTIFIER," +
                                "FOREIGN KEY(Status) REFERENCES ProjectStatus(id)" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task HolidayDate()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE HolidayDate (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "IdEmployee UNIQUEIDENTIFIER NOT NULL," +
                                "Date DATETIME" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task HospitalDate()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE HospitalDate (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "IdEmployee UNIQUEIDENTIFIER NOT NULL," +
                                "Date DATETIME" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task AssignmentDate()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE AssignmentDate (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "IdEmployee UNIQUEIDENTIFIER NOT NULL," +
                                "Date DATETIME" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task TaketimeoffDate()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE TaketimeoffDate (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "IdEmployee UNIQUEIDENTIFIER NOT NULL," +
                                "Date DATETIME" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Employee()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Employee (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "CompanyId UNIQUEIDENTIFIER NOT NULL," +
                                "CustomerId UNIQUEIDENTIFIER NOT NULL," +
                                "FirstName NVARCHAR(MAX) NOT NULL," +
                                "SecondName NVARCHAR(MAX) NOT NULL," +
                                "ThirdName NVARCHAR(MAX)," +
                                "PhoneWork NVARCHAR(MAX)," +
                                "PhoneSelf NVARCHAR(MAX)," +
                                "EmailWork NVARCHAR(MAX)," +
                                "EmailSelf NVARCHAR(MAX)," +
                                "TaxNumber NVARCHAR(MAX)," +
                                "AddressReg NVARCHAR(MAX)," +
                                "AddressFact NVARCHAR(MAX)," +
                                "Photo NVARCHAR(MAX)," +
                                "Login NVARCHAR(MAX)," +
                                "Password NVARCHAR(MAX)," +
                                "Salary FLOAT," +
                                "AddSalary FLOAT," +
                                "Status UNIQUEIDENTIFIER NOT NULL," +
                                "BirthDate DATETIME," +
                                "WorkStartDate DATETIME," +
                                "WorkEndDate DATETIME," +
                                "Position UNIQUEIDENTIFIER NOT NULL," +
                                "Rol UNIQUEIDENTIFIER NOT NULL," +
                                "FOREIGN KEY(Status) REFERENCES EmployeeStatus(id)" +
                           "); ";

                await database.ExecuteAsync(query);
            }
        }
        

        public async Task Companies()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Companies (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "CustomerId UNIQUEIDENTIFIER NOT NULL," +
                                "Name NVARCHAR(MAX) NOT NULL," +
                                "Address NVARCHAR(MAX) NOT NULL," +
                                "Code NVARCHAR(MAX) NOT NULL," +
                                "Actions NVARCHAR(MAX) NOT NULL," +
                                "StatusWeb NVARCHAR(MAX) NOT NULL," +
                                "Phone NVARCHAR(MAX) NOT NULL," +
                                "Email NVARCHAR(MAX) NOT NULL," +
                                "StatutCapital NVARCHAR(MAX) NOT NULL," +
                                "RegistrationDate NVARCHAR(MAX) NOT NULL," +
                                "Status UNIQUEIDENTIFIER NOT NULL," +
                                "Chief UNIQUEIDENTIFIER NOT NULL," +
                                "FOREIGN KEY(Status) REFERENCES CompanyStatus(id)," +
                             "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Customers()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Customers (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "FirstName NVARCHAR(MAX) NOT NULL," +
                                "SecondName NVARCHAR(MAX) NOT NULL," +
                                "ThirdName NVARCHAR(MAX) NOT NULL," +
                                "StatusLicenceId UNIQUEIDENTIFIER NOT NULL," +
                                "Phone NVARCHAR(MAX) NOT NULL," +
                                "Email NVARCHAR(MAX) NOT NULL," +
                                "Password NVARCHAR(MAX) NOT NULL," +
                                "EndTimeLicense DATETIME NOT NULL," +
                                "DateRegistration DATETIME NOT NULL," +
                                "FOREIGN KEY(StatusLicenceId) REFERENCES StatusLicence(id)," +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }

        public async Task Administrators()
        {
            using (var database = Context.Connect)
            {
                var query = "CREATE TABLE Administrators (" +
                                "Id UNIQUEIDENTIFIER PRIMARY KEY," +
                                "FirstName NVARCHAR(MAX) NOT NULL," +
                                "SecondName NVARCHAR(MAX) NOT NULL," +
                                "ThirdName NVARCHAR(MAX)," +
                                "PhoneWork NVARCHAR(MAX)," +
                                "EmailWork NVARCHAR(MAX)," +
                                "Login NVARCHAR(MAX)," +
                                "Password NVARCHAR(MAX)," +
                                "Status UNIQUEIDENTIFIER NOT NULL," +
                                "BirthDate DATETIME," +
                                "Rol UNIQUEIDENTIFIER NOT NULL," +
                                "FOREIGN KEY(Status) REFERENCES EmployeeStatus(id)" +
                            "); ";

                await database.ExecuteAsync(query);
            }
        }
    }
}
