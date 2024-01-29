using ReportingSystem.Models.User;
using Newtonsoft.Json;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Data.SQL;
using System.Diagnostics;
using ReportingSystem.Models.Configuration;
using Microsoft.AspNetCore.SignalR;
using ReportingSystem.Hubs;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static ReportingSystem.Data.SQL.TableTypeSQL;

namespace ReportingSystem.Data.Generate
{
    public class GenerateMain(IHubContext<StatusHub> hubContext)
    {
        private readonly IHubContext<StatusHub> _hubContext = hubContext;

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

            List<EmployeeModel> c1 = [];
            List<CustomerModel> c2 = [];
            ConfigurationModel c3 = new();

            try
            {
                c1 = new GenerateAdministrators().Administrators();
                c2 = await new GenerateCustomers(hubContext).Customers();
                c3 = new ConfigurationModel();
            }
            catch (Exception)
            {
                throw;
            }
            
            var databaseMoqData = new
            {
                Administrators = c1,
                Customers = c2,
                Configuration = c3
            };
            await new StatusSignalR(hubContext).UpdateStatusGenerateData(""); ;
            await UpdateStatusOnClient("Дані згенеровані", 15);

            try
            {
                await UpdateStatusOnClient("Запис в SQL", 20);
                //write SQL
                Debug.WriteLine($"SQL write Start " + DateTime.Now);
                Database.Drop(Context.serverName, Context.dbName);
                Database.Create(Context.serverName, Context.dbName);
                await UpdateStatusOnClient("База даних створена", 25);
                Debug.WriteLine($"База даних створена " + DateTime.Now);
                string[] ar = [Context.serverName, Context.dbName];

                await new CreateTables().Enums();
                await UpdateStatusOnClient("Таблиці з статичними даними створені", 30);
                Debug.WriteLine($"Таблиці з статичними даними створені " + DateTime.Now);
                await new CreateTables().Customers();
                await UpdateStatusOnClient("Таблиця замовників створена", 35);
                Debug.WriteLine($"Таблиця замовників створена " + DateTime.Now);
                await new CreateTables().Administrators();
                await UpdateStatusOnClient("Таблиця адміністраторів створена", 40);
                await UpdateStatusOnClient("Основні таблиці створені", 45);
                await new InsertData().Administrators(databaseMoqData.Administrators);

                foreach (var customer in databaseMoqData.Customers)
                {
                    await UpdateStatusOnClient("Вставлення даних по замовникам ...", 50);
                    await new InsertData().Customer(customer);
                    if (customer.Companies != null)
                    {
                        foreach (var company in customer.Companies)
                        {
                            await UpdateStatusOnClient("Вставлення даних по компаніям ...", 55);
                            await new InsertData().Company(company, customer.Id);

                            if (company.Positions != null)
                            {
                                int i = 0;
                                foreach (var position in company.Positions)
                                {
                                    await UpdateStatusOnClient("Вставлення даних по співробітникам ...", 60);
                                    await new InsertData().EmployeePosition(position, customer.Id, company.Id, i);
                                    i++;
                                    if (company.Employees != null)
                                    {
                                        foreach (var employee in company.Employees)
                                        {
                                            await new InsertData().Employee(employee);
                                        }
                                    }
                                }
                            }
                            if (company.Rolls != null)
                            {
                                foreach (var rol in company.Rolls)
                                {
                                    await UpdateStatusOnClient("Оновлення ролей ...", 65);
                                    var idRol = await new SQLRead().GetRolIdByType(rol);
                                    await new InsertData().CompanyRolls(idRol, customer.Id, company.Id);
                                    
                                }
                            }
                            foreach (var category in company.Categories)
                            {
                                await UpdateStatusOnClient("Оновлення категорій ...", 66);
                                await new InsertData().CompanyCategories(category, customer.Id, company.Id);
                            }

                            if (company.Projects != null)
                            {
                                await UpdateStatusOnClient("Оновлення проектів ...", 67);
                                foreach (var project in company.Projects)
                                {
                                    await new InsertData().CompanyProjects(project);
                                    await new InsertData().Steps(project);
                                    await new InsertData().ProjectPositions(project);
                                    await new InsertData().ProjectMembers(project);
                                }
                                await new InsertData().UpdateProjectCategories(company);
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
                await UpdateStatusOnClient("Операція завершена, перейдіть до авторизації", 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
           
        }
    }
}
