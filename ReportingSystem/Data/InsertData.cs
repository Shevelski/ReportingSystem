using Dapper;
using ReportingSystem.Enums.Extensions;

namespace ReportingSystem.Data
{
    public class InsertData
    {
        public async Task StatusLicence()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[StatusLicence] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.LicenceType licence in Enum.GetValues(typeof(Enums.LicenceType)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)licence,
                        Name = licence.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task AuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[AuthorizeStatus] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.AuthorizeStatus authorize in Enum.GetValues(typeof(Enums.AuthorizeStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)authorize,
                        Name = authorize.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task CompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[CompanyStatus] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.CompanyStatus company in Enum.GetValues(typeof(Enums.CompanyStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)company,
                        Name = company.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task EmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[EmployeeRolStatus] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.EmployeeRolStatus rol in Enum.GetValues(typeof(Enums.EmployeeRolStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)rol,
                        Name = rol.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task EmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[EmployeeStatus] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.EmployeeStatus employee in Enum.GetValues(typeof(Enums.EmployeeStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)employee,
                        Name = employee.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task ProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[ProjectStatus] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.ProjectStatus project in Enum.GetValues(typeof(Enums.ProjectStatus)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)project,
                        Name = project.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }
        public async Task Status()
        {
            using (var database = Context.Connect)
            {
                var query = "INSERT INTO [dbo].[Status] " +
                            "([id], [type], [name]) " +
                            "VALUES (@Id, @Type, @Name)";

                foreach (Enums.Status status in Enum.GetValues(typeof(Enums.Status)))
                {
                    var parameters = new
                    {
                        Id = Guid.NewGuid(),
                        Type = (int)status,
                        Name = status.GetDisplayName()
                    };

                    await database.ExecuteAsync(query, parameters);
                }
            }
        }




        

    }
}
