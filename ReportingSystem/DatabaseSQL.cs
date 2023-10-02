using ReportingSystem.Data;
using ReportingSystem.Models.Authorize;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project.Step;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using System.Data.Entity;

namespace ReportingSystem
{
    public static class DatabaseSQL
    {

        public static async Task Init()
        {
            try
            {
               
                await new CreateTables().Enums();
                await new CreateTables().Customers();

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
