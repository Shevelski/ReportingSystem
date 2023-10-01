using System.Data;
using System.Data.SqlClient;

namespace ReportingSystem.Data
{
    public static class Context
    {
        public static IDbConnection Connect 
        {
            get
            {
                return new SqlConnection("Server=localhost\\SQLEXPRESS;Database=ReportingSystem;Trusted_Connection=True;");
            }
        }
    }
}
