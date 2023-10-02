using Dapper;
using ReportingSystem.Data;

namespace ReportingSystem.Data
{
    public class TablesIsExist
    {
        public bool Customers()
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
        public bool Administrators()
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
        public bool Configuration()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Configuration'";
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
        public bool Configure()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Configure'";
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
        public bool StatusLicence()
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
        public bool AuthorizeStatus()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuthorizeStatus'";
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
        public bool CompanyStatus()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompanyStatus'";
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
        public bool EmployeeRolStatus()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeRolStatus'";
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
        public bool EmployeeStatus()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeStatus'";
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
        public bool ProjectStatus()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProjectStatus'";
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
        public bool Status()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Status'";
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
        public bool HistoryOperations()
        {
            using (var database = Context.Connect)
            {
                var tableExistsQuery = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HistoryOperations'";
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
