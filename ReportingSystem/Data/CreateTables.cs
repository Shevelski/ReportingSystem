using Dapper;
using ReportingSystem.Enums;

namespace ReportingSystem.Data
{
    public class CreateTables
    {
        //----------------------------- для Customers
        public async Task CreateEnumsTables()
        {
            if (!new TablesIsExist().StatusLicence())
            {
                await new CreateTableForCustomers().StatusLicence();
                await new InsertData().StatusLicence();
            }
            if (!new TablesIsExist().AuthorizeStatus())
            {
                await new CreateTableForCustomers().AuthorizeStatus();
                await new InsertData().AuthorizeStatus();
            }
            if (!new TablesIsExist().CompanyStatus())
            {
                await new CreateTableForCustomers().CompanyStatus();
                await new InsertData().CompanyStatus();
            }
            if (!new TablesIsExist().EmployeeRolStatus())
            {
                await new CreateTableForCustomers().EmployeeRolStatus();
                await new InsertData().EmployeeRolStatus();
            }
            if (!new TablesIsExist().EmployeeStatus())
            {
                await new CreateTableForCustomers().EmployeeStatus();
                await new InsertData().EmployeeStatus();
            }
            if (!new TablesIsExist().ProjectStatus())
            {
                await new CreateTableForCustomers().ProjectStatus();
                await new InsertData().ProjectStatus();
            }
            if (!new TablesIsExist().Status())
            {
                await new CreateTableForCustomers().Status();
                await new InsertData().Status();
            }
        }

        

        public async Task CreateTableCustomers()
        {
            if (!new TablesIsExist().Configure())
            {
                await new CreateTableForCustomers().Configure();
            }
        }

        //----------------------------- для Customers

        public void CreateTableAdministrators()
        {

        }
        public void CreateTableConfiguration()
        {

        }
        
        
    }
}
