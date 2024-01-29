using Dapper;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.Xml;

namespace ReportingSystem.Data.SQL
{

    public class CreateTable
    {
        private async Task CreateAsync(string tableName, string columns)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = $"CREATE TABLE {tableName} ({columns});";
                await database.ExecuteAsync(query);
            }
        }

        private async Task AlterTableAsync(string tableName, string alterStatement)
        {
            using (var database = Context.ConnectToSQL)
            {
                var query = $"ALTER TABLE {tableName} {alterStatement};";
                await database.ExecuteAsync(query);
            }
        }

        public async Task StatusLicence()
        {
            await CreateAsync("StatusLicence", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task AuthorizeStatus()
        {
            await CreateAsync("AuthorizeStatus", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task AuthorizeHistory()
        {
            await CreateAsync("AuthorizeHistory", "Id UNIQUEIDENTIFIER PRIMARY KEY, EmployeeId UNIQUEIDENTIFIER, RolId UNIQUEIDENTIFIER, AuthorizeStatusId UNIQUEIDENTIFIER NOT NULL, FOREIGN KEY(AuthorizeStatusId) REFERENCES AuthorizeStatus(Id)");
        }

        public async Task CompanyStatus()
        {
            await CreateAsync("CompanyStatus", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task EmployeeRolStatus()
        {
            await CreateAsync("EmployeeRolStatus", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
            await AlterTableAsync("AuthorizeHistory", "ADD FOREIGN KEY(RolId) REFERENCES EmployeeRolStatus(Id)");
        }

        public async Task EmployeeStatus()
        {
            await CreateAsync("EmployeeStatus", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task ProjectStatus()
        {
            await CreateAsync("ProjectStatus", "Id UNIQUEIDENTIFIER PRIMARY KEY, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task Configure()
        {
            await CreateAsync("Configure", "Id UNIQUEIDENTIFIER PRIMARY KEY, Param1 INT NOT NULL, Param2 INT NOT NULL, Param3 INT NOT NULL, Param4 INT NOT NULL, Param5 NVARCHAR(MAX) NOT NULL, Param6 NVARCHAR(MAX) NOT NULL, Param7 NVARCHAR(MAX) NOT NULL, Param8 NVARCHAR(MAX) NOT NULL, Param9 UNIQUEIDENTIFIER NOT NULL, Param10 UNIQUEIDENTIFIER NOT NULL, Param11 UNIQUEIDENTIFIER NOT NULL, Param12 UNIQUEIDENTIFIER NOT NULL");
        }

        public async Task HistoryOperations()
        {
            await CreateAsync("HistoryOperations", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, DateChange DATETIME, OldEndDateLicence DATETIME, NewEndDateLicence DATETIME, OldStatus UNIQUEIDENTIFIER, NewStatus UNIQUEIDENTIFIER, Price FLOAT, Period NVARCHAR(MAX), NameOperation NVARCHAR(MAX), FOREIGN KEY (OldStatus) REFERENCES StatusLicence(Id), FOREIGN KEY (NewStatus) REFERENCES StatusLicence(Id)");
        }

        public async Task EmployeePosition()
        {
            await CreateAsync("EmployeePosition", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, Type INT NOT NULL, Name NVARCHAR(MAX) NOT NULL");
        }

        public async Task CompanyRolls()
        {
            await CreateAsync("CompanyRolls", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER NOT NULL, CompanyId UNIQUEIDENTIFIER NOT NULL, RolId UNIQUEIDENTIFIER NOT NULL");
            await AlterTableAsync("CompanyRolls", "ADD FOREIGN KEY(CustomerId) REFERENCES Customers(Id)");
            await AlterTableAsync("CompanyRolls", "ADD FOREIGN KEY(CompanyId) REFERENCES Companies(Id)");
            await AlterTableAsync("CompanyRolls", "ADD FOREIGN KEY(RolId) REFERENCES EmployeeRolStatus(Id)");
        }

        public async Task CompanyCategory3()
        {
            await CreateAsync("CompanyCategory3", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, CompanyCategory2 UNIQUEIDENTIFIER, Name NVARCHAR(MAX) NOT NULL");
        }
        public async Task CompanyCategory2()
        {
            await CreateAsync("CompanyCategory2", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, CompanyCategory1 UNIQUEIDENTIFIER, Name NVARCHAR(MAX) NOT NULL");
            await AlterTableAsync("CompanyCategory3", "ADD FOREIGN KEY(CompanyCategory2) REFERENCES CompanyCategory2(Id)");
        }

        public async Task CompanyCategory1()
        {
            await CreateAsync("CompanyCategory1", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, CompanyCategory0 UNIQUEIDENTIFIER, Name NVARCHAR(MAX) NOT NULL");
            await AlterTableAsync("CompanyCategory2", "ADD FOREIGN KEY(CompanyCategory1) REFERENCES CompanyCategory1(Id)");
        }
        public async Task CompanyCategory0()
        {
            await CreateAsync("CompanyCategory0", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, Name NVARCHAR(MAX) NOT NULL");
            await AlterTableAsync("CompanyCategory1", "ADD FOREIGN KEY(CompanyCategory0) REFERENCES CompanyCategory0(Id)");
        }

        public async Task Projects()
        {
            await CreateAsync("Projects", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER, CompanyId UNIQUEIDENTIFIER, Name NVARCHAR(MAX) NOT NULL, Description NVARCHAR(MAX), ProjectCostsForCompany FLOAT, ProjectCostsForCustomer FLOAT, StartDate DATETIME, PlanDate DATETIME, EndDate DATETIME, Status UNIQUEIDENTIFIER, Head UNIQUEIDENTIFIER, CategoryModel0 UNIQUEIDENTIFIER, CategoryModel1 UNIQUEIDENTIFIER, CategoryModel2 UNIQUEIDENTIFIER, CategoryModel3 UNIQUEIDENTIFIER");
            await AlterTableAsync("Projects", "ADD FOREIGN KEY(Status) REFERENCES ProjectStatus(Id)");
            //await AlterTableAsync("Projects", "ADD FOREIGN KEY(CategoryModel0) REFERENCES ProjectCategory0(Id)");
            //await AlterTableAsync("Projects", "ADD FOREIGN KEY(CategoryModel1) REFERENCES ProjectCategory1(Id)");
            //await AlterTableAsync("Projects", "ADD FOREIGN KEY(CategoryModel2) REFERENCES ProjectCategory2(Id)");
            //await AlterTableAsync("Projects", "ADD FOREIGN KEY(CategoryModel3) REFERENCES ProjectCategory3(Id)");
            //await AlterTableAsync("Steps", "ADD FOREIGN KEY(ProjectId) REFERENCES Projects(Id)");
            //await AlterTableAsync("ProjectPositions", "ADD FOREIGN KEY(ProjectId) REFERENCES Projects(Id)");
            //await AlterTableAsync("StepSteps", "ADD FOREIGN KEY(ProjectId) REFERENCES Projects(Id)");
            //await AlterTableAsync("ProjectSteps", "ADD FOREIGN KEY(ProjectId) REFERENCES Projects(Id)");
        }

        public async Task HolidayDate()
        {
            await CreateAsync("HolidayDate", "Id UNIQUEIDENTIFIER PRIMARY KEY, EmployeeId UNIQUEIDENTIFIER NOT NULL, Date DATETIME");
        }

        public async Task HospitalDate()
        {
            await CreateAsync("HospitalDate", "Id UNIQUEIDENTIFIER PRIMARY KEY, EmployeeId UNIQUEIDENTIFIER NOT NULL, Date DATETIME");
        }

        public async Task AssignmentDate()
        {
            await CreateAsync("AssignmentDate", "Id UNIQUEIDENTIFIER PRIMARY KEY, EmployeeId UNIQUEIDENTIFIER NOT NULL, Date DATETIME");
        }
        public async Task TaketimeoffDate()
        {
            await CreateAsync("TaketimeoffDate", "Id UNIQUEIDENTIFIER PRIMARY KEY, EmployeeId UNIQUEIDENTIFIER NOT NULL, Date DATETIME");
        }
        public async Task Steps()
        {
            await CreateAsync("Steps", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER NOT NULL, CompanyId UNIQUEIDENTIFIER NOT NULL, ProjectId UNIQUEIDENTIFIER NOT NULL, Name NVARCHAR(MAX) NOT NULL, Description NVARCHAR(MAX), DateStart DATETIME, DatePlan DATETIME, DateEnd DATETIME");
        }

        public async Task ProjectPositions()
        {
            await CreateAsync("ProjectPositions", "Id UNIQUEIDENTIFIER PRIMARY KEY, ProjectId UNIQUEIDENTIFIER NOT NULL,  EmployeePositionId UNIQUEIDENTIFIER NOT NULL");
            await AlterTableAsync("ProjectPositions", "ADD FOREIGN KEY(EmployeePositionId) REFERENCES EmployeePosition(Id)");
        }
        public async Task ProjectSteps()
        {
            await CreateAsync("ProjectSteps", "Id UNIQUEIDENTIFIER PRIMARY KEY, ProjectId UNIQUEIDENTIFIER, StepsId UNIQUEIDENTIFIER");
            //await AlterTableAsync("ProjectSteps", "ADD FOREIGN KEY(StepsId) REFERENCES Steps(Id)");

        }
        public async Task ProjectMembers()
        {
            await CreateAsync("ProjectMembers", "Id UNIQUEIDENTIFIER PRIMARY KEY, ProjectId UNIQUEIDENTIFIER, EmployeeId UNIQUEIDENTIFIER");
        }
        //public async Task StepPositions()
        //{
        //    await CreateAsync("StepPositions", "Id UNIQUEIDENTIFIER PRIMARY KEY, StepId UNIQUEIDENTIFIER, ProjectId UNIQUEIDENTIFIER,  EmployeePositionId UNIQUEIDENTIFIER NOT NULL");
        //    await AlterTableAsync("StepPositions", "ADD FOREIGN KEY(StepId) REFERENCES Steps(Id)");
        //    await AlterTableAsync("StepPositions", "ADD FOREIGN KEY(EmployeePositionId) REFERENCES EmployeePosition(Id)");
        //}
        //public async Task StepSteps()
        //{
        //    await CreateAsync("StepSteps", "Id UNIQUEIDENTIFIER PRIMARY KEY, StepId UNIQUEIDENTIFIER, ProjectId UNIQUEIDENTIFIER, StepsId UNIQUEIDENTIFIER");
        //    await AlterTableAsync("StepSteps", "ADD FOREIGN KEY(StepId) REFERENCES Steps(Id)");
        //}
        //public async Task StepMembers()
        //{
        //    await CreateAsync("StepMembers", "Id UNIQUEIDENTIFIER PRIMARY KEY, StepId UNIQUEIDENTIFIER, ProjectId UNIQUEIDENTIFIER, EmployeeId UNIQUEIDENTIFIER");
        //}


        public async Task Employees()
        {
            await CreateAsync("Employees", "Id UNIQUEIDENTIFIER PRIMARY KEY, CompanyId UNIQUEIDENTIFIER NOT NULL, CustomerId UNIQUEIDENTIFIER NOT NULL, FirstName NVARCHAR(MAX) NOT NULL, SecondName NVARCHAR(MAX) NOT NULL, ThirdName NVARCHAR(MAX), PhoneWork NVARCHAR(MAX), PhoneSelf NVARCHAR(MAX), EmailWork NVARCHAR(MAX), EmailSelf NVARCHAR(MAX), TaxNumber NVARCHAR(MAX), AddressReg NVARCHAR(MAX), AddressFact NVARCHAR(MAX), Photo NVARCHAR(MAX), Login NVARCHAR(MAX), Password NVARCHAR(MAX), Salary FLOAT, AddSalary FLOAT, Status UNIQUEIDENTIFIER NOT NULL, BirthDate DATETIME, WorkStartDate DATETIME, WorkEndDate DATETIME, Position UNIQUEIDENTIFIER NOT NULL, Rol UNIQUEIDENTIFIER NOT NULL");
            await AlterTableAsync("Employees", "ADD FOREIGN KEY(Status) REFERENCES EmployeeStatus(Id)");
            await AlterTableAsync("Employees", "ADD FOREIGN KEY(Rol) REFERENCES EmployeeRolStatus(Id)");
            await AlterTableAsync("AuthorizeHistory", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            await AlterTableAsync("HolidayDate", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            await AlterTableAsync("HospitalDate", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            await AlterTableAsync("AssignmentDate", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            await AlterTableAsync("TaketimeoffDate", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            //await AlterTableAsync("ProjectMembers", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
            //await AlterTableAsync("StepMembers", "ADD FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)");
        }

        public async Task Companies()
        {
            await CreateAsync("Companies", "Id UNIQUEIDENTIFIER PRIMARY KEY, CustomerId UNIQUEIDENTIFIER NOT NULL, Name NVARCHAR(MAX) NOT NULL, Address NVARCHAR(MAX) NOT NULL, Code NVARCHAR(MAX) NOT NULL, Actions NVARCHAR(MAX) NOT NULL, StatusWeb NVARCHAR(MAX) NOT NULL, Phone NVARCHAR(MAX) NOT NULL, Email NVARCHAR(MAX) NOT NULL, StatutCapital NVARCHAR(MAX) NOT NULL, RegistrationDate NVARCHAR(MAX) NOT NULL, Status UNIQUEIDENTIFIER NOT NULL, Chief UNIQUEIDENTIFIER");
            await AlterTableAsync("Companies", "ADD FOREIGN KEY(Status) REFERENCES CompanyStatus(Id)");
            await AlterTableAsync("EmployeePosition", "ADD FOREIGN KEY(CompanyId) REFERENCES Companies(Id)");
            await AlterTableAsync("Projects", "ADD FOREIGN KEY(CompanyId) REFERENCES Companies(Id)");
            await AlterTableAsync("Employees", "ADD FOREIGN KEY(CompanyId) REFERENCES Companies(Id)");
            await AlterTableAsync("CompanyCategory0", "ADD FOREIGN KEY(CompanyId) REFERENCES Companies(Id)");
        }

        public async Task Customers()
        {
            await CreateAsync("Customers", "Id UNIQUEIDENTIFIER PRIMARY KEY, FirstName NVARCHAR(MAX) NOT NULL, SecondName NVARCHAR(MAX) NOT NULL, ThirdName NVARCHAR(MAX) NOT NULL, StatusLicenceId UNIQUEIDENTIFIER NOT NULL, ConfigureId UNIQUEIDENTIFIER NOT NULL, Phone NVARCHAR(MAX) NOT NULL, Email NVARCHAR(MAX) NOT NULL, Password NVARCHAR(MAX) NOT NULL, EndTimeLicense DATETIME NOT NULL, DateRegistration DATETIME NOT NULL");

            await AlterTableAsync("Customers", "ADD FOREIGN KEY(StatusLicenceId) REFERENCES StatusLicence(Id)");
            await AlterTableAsync("Customers", "ADD FOREIGN KEY(ConfigureId) REFERENCES Configure(Id)");
            await AlterTableAsync("Companies", "ADD FOREIGN KEY(CustomerId) REFERENCES Customers(Id)");
            await AlterTableAsync("AuthorizeHistory", "ADD FOREIGN KEY(EmployeeId) REFERENCES Customers(Id)");
            await AlterTableAsync("EmployeePosition", "ADD FOREIGN KEY(CustomerId) REFERENCES Customers(Id)");
            await AlterTableAsync("Employees", "ADD FOREIGN KEY(CustomerId) REFERENCES Customers(Id)");
        }

        public async Task Administrators()
        {
            await CreateAsync("Administrators", "Id UNIQUEIDENTIFIER PRIMARY KEY, FirstName NVARCHAR(MAX) NOT NULL, SecondName NVARCHAR(MAX) NOT NULL, ThirdName NVARCHAR(MAX), PhoneWork NVARCHAR(MAX), EmailWork NVARCHAR(MAX), Login NVARCHAR(MAX), Password NVARCHAR(MAX), Status UNIQUEIDENTIFIER NOT NULL, BirthDate DATETIME, Rol UNIQUEIDENTIFIER NOT NULL");
            await AlterTableAsync("Administrators", "ADD FOREIGN KEY(Status) REFERENCES EmployeeStatus(Id)");
            await AlterTableAsync("AuthorizeHistory", "ADD FOREIGN KEY(EmployeeId) REFERENCES Administrators(Id)");
        }
    }
}
