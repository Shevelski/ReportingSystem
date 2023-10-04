using ReportingSystem.Data;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using ReportingSystem.Utils;
using Bogus;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Test.Generate;
using ReportingSystem.Test.GenerateData;
using System.Diagnostics;
using System;

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
