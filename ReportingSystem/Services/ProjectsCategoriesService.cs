using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;

namespace ReportingSystem.Services
{
    public class ProjectsCategoriesService
    {

        public string GetCustomerId()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string ar = MyConfig.GetValue<string>("TempCustomer:id");

            return ar;
        }

        public string GetCategories()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string ar = MyConfig.GetValue<string>("TempCustomer:id");

            return ar;
        }

      
    }
}
