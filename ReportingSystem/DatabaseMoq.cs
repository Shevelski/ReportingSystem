using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Project;
using ReportingSystem.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ReportingSystem
{
    public static class DatabaseMoq
    {
        public static CompanyModel? Configuration { get; set; }
        public static List<CustomerModel>? Customers { get; set; }
        public static List<EmployeeModel>? Administrators { get; set; }
     
        private class DatabaseMoqData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public CompanyModel? Configuration { get; set; }
        }

        private const string DataFilePath = "data.json";
       
        static DatabaseMoq()
        {

            if (File.Exists(DataFilePath) && new FileInfo(DataFilePath).Length > 0)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(DataFilePath))
                {
                    jsonData = reader.ReadToEnd();
                }
                var data = JsonConvert.DeserializeObject<DatabaseMoqData>(jsonData);
                if (data != null)
                {
                    Customers = data.Customers;
                    Administrators = data.Administrators;
                    Configuration = data.Configuration;
                }
            }
            else
            {
                // Генерування даних, якщо файл не існує
                Customers = DatabaseMoqGenerate.Customers;
                Administrators = DatabaseMoqGenerate.Administrators;
                Configuration = DatabaseMoqGenerate.Configuration;
                SaveDataToFile();
                Debug.WriteLine("DataJson було записано");
            }
        }

        public static void UpdateJson()
        {
            var data = new DatabaseMoqData
            {
                Customers = Customers,
                Administrators = Administrators,
                Configuration = Configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DataFilePath, jsonData);
        }

        private static void SaveDataToFile()
        {
            var data = new DatabaseMoqData
            {
                Customers = Customers,
                Administrators = Administrators,
                Configuration = Configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DataFilePath, jsonData);
        }
    }
}
