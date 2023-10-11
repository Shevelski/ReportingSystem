using ReportingSystem.Models.User;
using Newtonsoft.Json;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Data.SQL;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GenerateMain
    {
        public static List<CustomerModel>? Customers { get; set; }
        public static CompanyModel? Configuration { get; set; }
        public static List<EmployeeModel>? Administrators { get; set; }

        public async Task Data()
        {
            var databaseMoqData = new
            {
                Administrators = new GenerateAdministrators().Administrators(),
                Customers = new GenerateCustomers().Customers(),
                //Configuration = configuration
            };


            try
            {
                //write SQL
                Debug.WriteLine($"SQL write Start " + DateTime.Now);
                await new CreateTables().Enums();
                await new CreateTables().Customers();
                await new CreateTables().Administrators();
                await new InsertData().Administrators(databaseMoqData.Administrators);

                foreach (var customer in databaseMoqData.Customers)
                {
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
                        }
                    }
                }
                Debug.WriteLine($"SQL write End " + DateTime.Now);

                //write Json
                Debug.WriteLine($"Json write Start " + DateTime.Now);
                string jsonData = JsonConvert.SerializeObject(databaseMoqData, Formatting.Indented);
                File.WriteAllText(Context.Json, jsonData);
                Debug.WriteLine($"Json write End " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час роботи з базою даних: " + ex.Message);
            }
           
        }
    }
}
