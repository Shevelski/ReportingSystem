using ReportingSystem.Data;

namespace ReportingSystem
{
    public static class DatabaseSQL
    {

        public static void Init()
        {
            try
            {
                if (!new CheckTables().IsExistsCustomers())
                {
                    _ = new CreateTables().CreateTableCustomers();
                }
                if (!new CheckTables().IsExistsAdministrators())
                {
                    new CreateTables().CreateTableAdministrators();
                }
                if (!new CheckTables().IsExistsConfiguration())
                {
                    new CreateTables().CreateTableConfiguration();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
        }
    }
}
