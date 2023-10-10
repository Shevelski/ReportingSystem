using ReportingSystem.Data;

namespace ReportingSystem
{
    public class DatabaseSQL
    {
        public async Task Init()
        {
            try
            {
                await new CreateTables().Enums();
                await new CreateTables().Customers();
                await new CreateTables().CreateTableAdministrators();
                await new DatabaseGenerateSQL().GenerateAdministrators();
                await new DatabaseGenerateSQL().GenerateCustomers();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
        }
    }
}
