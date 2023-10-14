using System.Data;
using System.Data.SqlClient;

namespace ReportingSystem.Data
{
    public static class Context
    {
        public static IDbConnection ConnectToSQL 
        {
            get
            {
                return new SqlConnection("Server=localhost\\SQLEXPRESS;Database=ReportingSystem;Trusted_Connection=True;");
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
