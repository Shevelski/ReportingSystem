using ReportingSystem.Utils;
using System.Data;
using System.Data.SqlClient;

namespace ReportingSystem.Data
{
    public static class Context
    {
        public static string serverName = Settings.Server();
        public static string dbName = Settings.DB(); 
        
        public static string serverNameDefault = "LENOVONAKULAPTO\\SQLEXPRESS";
        public static string dbNameDefault = "ReportingSystem";
        
        //public static string connectionDB = $"Server={serverName};Trusted_Connection=True;";
        public static string connectionName = $"Server={serverName};Trusted_Connection=True;Database = {dbName}";

        public static IDbConnection ConnectToSQL 
        {

            get
            {
                return new SqlConnection(connectionName);
            }
        }
        
        public static string Json 
        {
            get
            {
                return "data.json";
            }
        }
    }
}
