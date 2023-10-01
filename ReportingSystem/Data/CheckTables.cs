using Dapper;

namespace ReportingSystem.Data
{
    public class CheckTables
    {
        public bool IsExistsCustomers()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Customers'";
                var tableExists = database.QueryFirstOrDefault<int>(tableExistsQuery);

                if (tableExists == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsExistsAdministrators()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Administrators'";
                var tableExists = database.QueryFirstOrDefault<int>(tableExistsQuery);

                if (tableExists == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsExistsConfiguration()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Administrators'";
                var tableExists = database.QueryFirstOrDefault<int>(tableExistsQuery);

                if (tableExists == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsExistsStatusLicence()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StatusLicence'";
                var tableExists = database.QueryFirstOrDefault<int>(tableExistsQuery);

                if (tableExists == 1)
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
