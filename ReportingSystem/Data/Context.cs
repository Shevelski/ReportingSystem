using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SqlClient;

namespace ReportingSystem.Data
{
    public static class Context
    {
        public static string dbName = "ReportingSystem";
        public static string connectionDB = $"Server=LENOVONAKULAPTO\\SQLEXPRESS;Trusted_Connection=True;";
        public static string connectionName = $"Server=LENOVONAKULAPTO\\SQLEXPRESS;Database={dbName};Trusted_Connection=True;";

        public static IDbConnection ConnectToSQL 
        {

            get
            {
                //return new SqlConnection("Server=localhost\\SQLEXPRESS;Database=ReportingSystem;Trusted_Connection=True;");
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
