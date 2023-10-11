using Dapper;

namespace ReportingSystem.Data.SQL
{
    public class DataIsExist
    {
        public bool Administrators()
        {
            using (var database = Context.Connect)
            {
                var dataExistsQuery = "SELECT COUNT(*) FROM [ReportingSystem].[dbo].[Administrators]";
                var tableExists = database.QueryFirstOrDefault<int>(dataExistsQuery);

                if (tableExists > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool Customers()
        {
            using (var database = Context.Connect)
            {
                var dataExistsQuery = "SELECT COUNT(*) FROM [ReportingSystem].[dbo].[Customers]";
                var tableExists = database.QueryFirstOrDefault<int>(dataExistsQuery);

                if (tableExists > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
