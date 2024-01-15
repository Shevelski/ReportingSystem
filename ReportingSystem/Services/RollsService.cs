using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;

namespace ReportingSystem.Services
{
    public class RollsService
    {

        //Отримання списку посад компанії 
        public List<EmployeeRolModel>? GetAllRolls(string idCu, string idCo, string idEm)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Rolls == null)
                {
                    return null;
                }

                return company.Rolls;

                //if (Guid.TryParse(idEm, out Guid idEmployee))
                //{

                //    if (company == null || company.employees == null)
                //    {
                //        return null;
                //    }

                //    var employee = company.employees.FirstOrDefault(em => em.id.Equals(idEmployee));

                //    if (employee != null)
                //    {
                //        return company.rolls;
                //    }
                //}
            }

            return null;
        }


        //отримати користувачів компанії за ролями
        public List<EmployeeModel>? GetEmployeesByRoll(string idCu, string idCo, string rol)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Employees == null)
                {
                    return null;
                }

                return company.Employees.Where(employee => employee.rol != null && employee.rol.rolName != null && employee.rol.rolName.Equals(rol)).ToList();
            }

            return null;
        }


        //зміна ролі
        public EmployeeModel? EditEmployeeRol(string[] ar)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Employees == null)
                {
                    return null;
                }

                if (Guid.TryParse(ar[2], out Guid idEmployee))
                {
                    var employee = company.Employees.FirstOrDefault(emp => emp.id.Equals(idEmployee));

                    if (employee != null && employee.rol != null && company.Rolls != null)
                    {
                        var newRol = company.Rolls.FirstOrDefault(rol => rol.rolName != null && rol.rolName.Equals(ar[3]));

                        if (newRol != null)
                        {
                            employee.rol = newRol;
                            DatabaseMoq.UpdateJson();
                            return employee;
                        }
                    }
                }
            }

            return null;
        }

    }
}
