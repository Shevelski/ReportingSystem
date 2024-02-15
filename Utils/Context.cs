using System.Data;
using System.Data.SqlClient;

namespace Utils
{
    public static class Context
    {
        public static string serverNameDefault = "LENOVONAKULAPTO\\SQLEXPRESS";
        public static string dbNameDefault = "ReportingSystem";

        public static string dbName = dbNameDefault;
        public static string serverName = serverNameDefault;
        public static string connectionName = $"Server={serverName};Trusted_Connection=True;Database={dbName}";


        public static IDbConnection ConnectToSQL
        {
            get
            {
                return new SqlConnection(connectionName);
            }
        }
    }
}
