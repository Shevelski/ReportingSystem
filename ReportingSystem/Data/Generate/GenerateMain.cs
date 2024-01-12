using ReportingSystem.Models.User;
using Newtonsoft.Json;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Data.SQL;
using System.Diagnostics;
using ReportingSystem.Models.Configuration;
using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Hubs;

namespace ReportingSystem.Data.Generate
{
    public class GenerateMain
    {
        private readonly IHubContext<StatusHub> _hubContext;

        public GenerateMain(IHubContext<StatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        private async Task UpdateStatusOnClient(string status, int percent)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveStatus", status, percent);
        }

        public static List<CustomerModel>? Customers { get; set; }
        public static CompanyModel? Configuration { get; set; }
        public static List<EmployeeModel>? Administrators { get; set; }

        public async Task Data()
        {
            await UpdateStatusOnClient("Початок операції", 10);
            var databaseMoqData = new
            {

                Administrators = new GenerateAdministrators().Administrators(),
                Customers = new GenerateCustomers().Customers(),
                Configuration = new ConfigurationModel()
            };
            await UpdateStatusOnClient("Дані згенеровані", 15);

            try
            {
                await UpdateStatusOnClient("Запис в SQL", 20);
                //write SQL
                Debug.WriteLine($"SQL write Start " + DateTime.Now);
                Database.Create(Context.connectionDB, Context.dbName);
                await UpdateStatusOnClient("База даних створена", 25);
                await new CreateTables().Enums();
                await UpdateStatusOnClient("Таблиці з статичними даними створені", 30);
                await new CreateTables().Customers();
                await UpdateStatusOnClient("Таблиця замовників створена", 35);
                await new CreateTables().Administrators();
                await UpdateStatusOnClient("Таблиця адміністраторів створена", 40);
                await UpdateStatusOnClient("Основні таблиці створені", 45);
                await new InsertData().Administrators(databaseMoqData.Administrators);

                foreach (var customer in databaseMoqData.Customers)
                {
                    await UpdateStatusOnClient("Вставлення даних по замовникам ...", 50);
                    await new InsertData().Customer(customer);
                    if (customer.companies != null)
                    {
                        foreach (var company in customer.companies)
                        {
                            await new InsertData().Company(company, customer.id);

                            if (company.positions != null)
                            {
                                int i = 0;
                                foreach (var position in company.positions)
                                {
                                    await new InsertData().EmployeePosition(position, customer.id, company.id, i);
                                    i++;
                                    if (company.employees != null)
                                    {
                                        foreach (var employee in company.employees)
                                        {
                                            await new InsertData().Employee(employee);
                                        }
                                    }
                                }
                            }
                            if (company.rolls != null)
                            {
                                foreach (var rol in company.rolls)
                                {
                                    var idRol = await new SQLRead().GetRolIdByType(rol);
                                    await new InsertData().CompanyRolls(idRol, customer.id, company.id);
                                }
                            }
                        }
                    }
                }
                Debug.WriteLine($"SQL write End " + DateTime.Now);
                await UpdateStatusOnClient("База заповнена", 70);

                //write Json
                Debug.WriteLine($"Json write Start " + DateTime.Now);
                await UpdateStatusOnClient("Заповнення Json", 85);
                string jsonData = JsonConvert.SerializeObject(databaseMoqData, Formatting.Indented);
                File.WriteAllText(Context.Json, jsonData);
                Debug.WriteLine($"Json write End " + DateTime.Now);
                await UpdateStatusOnClient("Операція завершена, перехід на авторизацію", 100);
             
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
           
        }
    }
}
