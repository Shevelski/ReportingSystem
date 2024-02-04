using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Report;

namespace ReportingSystem.Services
{
    public class ReportService
    {
        public async Task SendReport(string[] ar)
        {
               //return mode ? await new JsonWrite().SendReport(idCu, idCo) : 
                await new SQLWrite().SendReport(ar);
        }
        public async Task<List<ReportModel>> GetReports(string idCu, string idCo, string idEm, string startDate, string endDate)
        {
            return await new SQLRead().GetReports(idCu, idCo, idEm, startDate, endDate);
        }

            ////Отримання списку посад компанії 
            //public List<EmployeeRolModel>? GetAllRolls(string idCu, string idCo, string idEm)
            //{
            //    var customers = DatabaseMoq.Customers;

            //    if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            //    {
            //        return null;
            //    }

            //    var customer = customers.FirstOrDefault(cu => cu.id.Equals(idCustomer));

            //    if (customer == null || customer.companies == null)
            //    {
            //        return null;
            //    }

            //    if (Guid.TryParse(idCo, out Guid idCompany))
            //    {
            //        var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

            //        if (company == null || company.rolls == null)
            //        {
            //            return null;
            //        }

            //        return company.rolls;

            //        //if (Guid.TryParse(idEm, out Guid idEmployee))
            //        //{

            //        //    if (company == null || company.employees == null)
            //        //    {
            //        //        return null;
            //        //    }

            //        //    var employee = company.employees.FirstOrDefault(em => em.id.Equals(idEmployee));

            //        //    if (employee != null)
            //        //    {
            //        //        return company.rolls;
            //        //    }
            //        //}
            //    }

            //    return null;
            //}


            ////отримати користувачів компанії за ролями
            //public List<EmployeeModel>? GetEmployeesByRoll(string idCu, string idCo, string rol)
            //{
            //    var customers = DatabaseMoq.Customers;

            //    if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            //    {
            //        return null;
            //    }

            //    var customer = customers.FirstOrDefault(cu => cu.id.Equals(idCustomer));

            //    if (customer == null || customer.companies == null)
            //    {
            //        return null;
            //    }

            //    if (Guid.TryParse(idCo, out Guid idCompany))
            //    {
            //        var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

            //        if (company == null || company.employees == null)
            //        {
            //            return null;
            //        }

            //        return company.employees.Where(employee => employee.rol != null && employee.rol.rolName != null && employee.rol.rolName.Equals(rol)).ToList();
            //    }

            //    return null;
            //}


            ////зміна ролі
            //public EmployeeModel? EditEmployeeRol(string[] ar)
            //{
            //    var customers = DatabaseMoq.Customers;

            //    if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            //    {
            //        return null;
            //    }

            //    var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            //    if (customer == null || customer.companies == null)
            //    {
            //        return null;
            //    }

            //    if (Guid.TryParse(ar[1], out Guid idCompany))
            //    {
            //        var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

            //        if (company == null || company.employees == null)
            //        {
            //            return null;
            //        }

            //        if (Guid.TryParse(ar[2], out Guid idEmployee))
            //        {
            //            var employee = company.employees.FirstOrDefault(emp => emp.id.Equals(idEmployee));

            //            if (employee != null && employee.rol != null && company.rolls != null)
            //            {
            //                var newRol = company.rolls.FirstOrDefault(rol => rol.rolName != null && rol.rolName.Equals(ar[3]));

            //                if (newRol != null)
            //                {
            //                    employee.rol = newRol;
            //                    DatabaseMoq.UpdateJson();
            //                    return employee;
            //                }
            //            }
            //        }
            //    }

            //    return null;
            //}

        }
}
