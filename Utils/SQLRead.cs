using System.Diagnostics;
using System.Linq.Expressions;
using Dapper;

namespace Utils
{
    public class SQLRead
    {
        private class DatabaseData
        {
            public string? email { get; set; }
            public string? password { get; set; }
        }

        public async Task<List<string>> GetData() {
            List<string> list = new List<string>();

            for (int i = 1; i < 8; i++)
            {
                Guid rolId = await GetEmployeeRoleIdByType(i);
                switch (i)
                {
                    case 1:
                        list.AddRange(await GetAdministratorData(rolId));
                        break;
                    case 2:
                        list.AddRange(await GetAdministratorData(rolId));
                        break;
                    case 3:
                        list.AddRange(await GetCustomerData());
                        break;
                    case 4:
                        list.AddRange(await GetEmployeeData(rolId));
                        break;
                    case 5:
                        list.AddRange(await GetEmployeeData(rolId));
                        break;
                    case 6:
                        list.AddRange(await GetEmployeeData(rolId));
                        break;
                    case 7:
                        list.AddRange(await GetEmployeeData(rolId));
                        break;
                }
            }
            return list;
        }

        public async Task<List<string>> GetAdministratorData(Guid rolId)
        {
            var query = @$"SELECT TOP (1) [EmailWork]
                              ,[Password]
                          FROM [{Context.dbName}].[dbo].[Administrators]
                          WHERE [Rol] = @Rol";
            var para = new
            {
                Rol = rolId
            };
            using var database = Context.ConnectToSQL;
            List<string> val = new List<string>();
            var result = await database.QueryAsync<AuthModel>(query, para);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    val.Add(item.EmailWork);
                    val.Add(EncryptionHelper.Decrypt(item.Password));
                }
            }
            
            return val;
        } 

        public async Task<List<string>> GetCustomerData()
        {
            var query = @$"SELECT TOP (1) [Email]
                              ,[Password]
                          FROM [{Context.dbName}].[dbo].[Customers]";
            using var database = Context.ConnectToSQL;
            List<string> val = new List<string>();
            var result = await database.QueryAsync<AuthCustModel>(query);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    val.Add(item.Email);
                    val.Add(EncryptionHelper.Decrypt(item.Password));
                }
            }

            return val;
        } 
        public async Task<List<string>> GetEmployeeData(Guid rolId)
        {
            var query = @$"SELECT TOP (1) [EmailWork]
                              ,[Password]
                          FROM [{Context.dbName}].[dbo].[Employees]
                          WHERE [Rol] = @Rol";
            var para = new
            {
                Rol = rolId
            };
            using var database = Context.ConnectToSQL;
            List<string> val = new List<string>();
            var result = await database.QueryAsync<AuthModel>(query, para);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    val.Add(item.EmailWork);
                    val.Add(EncryptionHelper.Decrypt(item.Password));
                }
            }

            return val;
        } 

        public async Task<Guid> GetEmployeeRoleIdByType(int type)
        {
           
            var query = $"SELECT [Id] FROM [{Context.dbName}].[dbo].[EmployeeRolStatus] Where Type = @type";
            var para = new
            {
                Type = type,
            };
            using var database = Context.ConnectToSQL;
            var result = await database.QueryAsync<Guid>(query, para);
            return result.First();
        }


    }
}
