using ReportingSystem.Data;

namespace ReportingSystem
{
    public static class DatabaseSQL
    {

        public static async Task Init()
        {
            try
            {

                await new CreateTables().CreateEnumsTables();
              
                //if (!new TablesIsExist().Customers())
                //{
                //    _ = new CreateTables().CreateTableCustomers();
                //}
                //if (!new TablesIsExist().Administrators())
                //{
                //    new CreateTables().CreateTableAdministrators();
                //}
                //if (!new TablesIsExist().Configuration())
                //{
                //    new CreateTables().CreateTableConfiguration();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
        }
    }
}
