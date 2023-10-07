using Dapper;
using ReportingSystem.Enums;

namespace ReportingSystem.Data
{
    public class CreateTables
    {
        public async Task Enums()
        {
            if (!new TablesIsExist().StatusLicence())
            {
                await new CreateTable().StatusLicence();
                await new InsertData().StatusLicence();
            }
            if (!new TablesIsExist().AuthorizeStatus())
            {
                await new CreateTable().AuthorizeStatus();
                await new InsertData().AuthorizeStatus();
            }
            if (!new TablesIsExist().AuthorizeHistory())
            {
                await new CreateTable().AuthorizeHistory();
                //await new InsertData().AuthorizeHistory();
            }
            if (!new TablesIsExist().CompanyStatus())
            {
                await new CreateTable().CompanyStatus();
                await new InsertData().CompanyStatus();
            }
            if (!new TablesIsExist().EmployeeRolStatus())
            {
                await new CreateTable().EmployeeRolStatus();
                await new InsertData().EmployeeRolStatus();
            }
            if (!new TablesIsExist().EmployeeStatus())
            {
                await new CreateTable().EmployeeStatus();
                await new InsertData().EmployeeStatus();
            }
            if (!new TablesIsExist().ProjectStatus())
            {
                await new CreateTable().ProjectStatus();
                await new InsertData().ProjectStatus();
            }
            //if (!new TablesIsExist().Status())
            //{
            //    await new CreateTable().Status();
            //    await new InsertData().Status();
            //}
        }

        public async Task Customers()
        {
            if (!new TablesIsExist().Configure())
            {
                await new CreateTable().Configure();
            }
            if (!new TablesIsExist().HistoryOperations())
            {
                await new CreateTable().HistoryOperations();
            }
            if (!new TablesIsExist().EmployeePosition())
            {
                await new CreateTable().EmployeePosition();
            }
            //if (!new TablesIsExist().EmployeeRol())
            //{
            //    await new CreateTable().EmployeeRol();
            //}
            if (!new TablesIsExist().HolidayDate())
            {
                await new CreateTable().HolidayDate();
            }
            if (!new TablesIsExist().HospitalDate())
            {
                await new CreateTable().HospitalDate();
            }
            if (!new TablesIsExist().AssignmentDate())
            {
                await new CreateTable().AssignmentDate();
            }
            if (!new TablesIsExist().TaketimeoffDate())
            {
                await new CreateTable().TaketimeoffDate();
            }
            if (!new TablesIsExist().Employees())
            {
                await new CreateTable().Employee();
            }
            if (!new TablesIsExist().Projects())
            {
                await new CreateTable().Projects();
            }
            if (!new TablesIsExist().Companies())
            {
                await new CreateTable().Companies();
            }
            if (!new TablesIsExist().Customers())
            {
                await new CreateTable().Customers();
            }
        }


        public async Task CreateTableAdministrators()
        {

            if (!new TablesIsExist().Administrators())
            {
                await new CreateTable().Administrators();
            }

            
            
        }
        //public void CreateTableConfiguration()
        //{

        //}
        
        
    }
}
