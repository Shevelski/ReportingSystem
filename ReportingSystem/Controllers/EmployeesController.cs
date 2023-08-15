using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Controllers
{

    

    public class EmployeesController : Controller
    {
        EmployeeModel user = new EmployeeModel();
        //Замінити Customers[0] на id авторизованого директора, здіснивши пошук за id
        //Замінити Customers[0] на id авторизованого директора, здіснивши пошук за id
        //Замінити Customers[0] на id авторизованого директора, здіснивши пошук за id

        //[HttpGet]
        //public async Task<IActionResult> GetEmployees()
        //{

        //    await Task.Delay(10);
        //    var x = DatabaseMoq.Customers[0].companies[0].employees;
        //    return Json(x);
        //}

        [HttpGet]
        public async Task<IActionResult> GetEmployees(string id)
        {
            await Task.Delay(10);

            if (Guid.TryParse(id, out Guid idCompany))
            {
                var x = DatabaseMoq.Customers[0].companies.FirstOrDefault(company => company.id == idCompany);

                if (x != null)
                {
                    var employees = x.employees;
                    return Json(employees);
                }
            }
            else
            {
                return Json(DatabaseMoq.Customers[0].companies[0].employees);
            }

            return NoContent();
        }



        [HttpPost]
        public async Task<IActionResult> EditCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);

            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }

            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            CompanyModel company = DatabaseMoq.Customers[0].companies.FirstOrDefault(c => c.id.Equals(id));
            company.name = ar[1];
            company.address = ar[2];
            company.actions = ar[3];
            company.phone = ar[4];
            company.email = ar[5];
            return Json(company);
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);

            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }

            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            CompanyModel company = DatabaseMoq.Customers[0].companies.FirstOrDefault(c => c.id.Equals(id));
            CompanyStatusModel status = new CompanyStatusModel();
            company.status = new CompanyStatusModel()
            {
                companyStatusType = Enum.CompanyStatus.Archive,
                companyStatusName = Enum.CompanyStatus.Archive.GetDisplayName(),
            };
            return Json(DatabaseMoq.Customers[0].companies);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);

            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }

            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            // тут замінити на id авторизованого директора
            CompanyModel company = DatabaseMoq.Customers[0].companies.FirstOrDefault(c => c.id.Equals(id));
            DatabaseMoq.Customers[0].companies.Remove(company);

            return Json(DatabaseMoq.Customers[0].companies);
        }

        
        private static Dictionary<Guid, CompanyModel> companiesData = new Dictionary<Guid, CompanyModel>();

        [HttpPost]
        public async Task<IActionResult> PostCheckCompany([FromBody] string[] ar)
        {
            await Task.Delay(10);

            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }

            companiesData.Add(id, CheckCompanyWeb.ByCode(ar[1]));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckCompany(string id)
        {
            await Task.Delay(10);

            Guid guid = new Guid();
            if (Guid.TryParse(id, out Guid result))
            {
                guid = result;
            }

            if (companiesData.TryGetValue(guid, out var companyDetails))
            {
                companiesData.Remove(guid);
                return Json(companyDetails);
            }
            else
            {
                return NotFound();
            }

        }


        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] string[] ar)
        {
            CompanyModel company = new CompanyModel();
            company.name = ar[0];
            company.code = ar[1];
            company.address = ar[2];
            company.actions = ar[3];
            company.phone = ar[4];
            company.email = ar[5];
            company.registrationDate = DateTime.Today;
            company.rolls = DefaultEmployeeRolls.Get();
            company.positions = new List<EmployeePositionModel>();
            company.employees = new List<EmployeeModel>();
            company.status = new CompanyStatusModel()
            {
                companyStatusType = CompanyStatus.Project,
                companyStatusName = CompanyStatus.Project.GetDisplayName(),
            };

            EmployeeModel chief = new EmployeeModel()
            {
                firstName = DatabaseMoq.Customers[0].firstName,
                secondName = DatabaseMoq.Customers[0].secondName,
                thirdName = DatabaseMoq.Customers[0].thirdName,
                emailWork = DatabaseMoq.Customers[0].email,

            };
            company.chief = chief;
            //поточний кастомер - змінити на id кастомера
            DatabaseMoq.Customers[0].companies.Add(company);

            return NoContent();
            //return Json(DatabaseMoq.Customers[0].companies);
           
        }

    }
}
