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
    public class PositionsService
    {

        //Отримання списку посад компанії 
        public List<EmployeePositionModel>? GetAllPositions(string idCu, string idCo)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null)
                {
                    return company.positions;
                }
            }

            return null;
        }


        //Отримання списку унікальних посад компанії 
        public List<EmployeePositionModel>? GetUniqPositions(string idCu, string idCo)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.positions != null)
                {
                    var uniquePositions = company.positions
                        .Where(position => !string.IsNullOrEmpty(position.namePosition))
                        .GroupBy(position => position.namePosition)
                        .Select(group => group.First())
                        .ToList();

                    return uniquePositions;
                }
            }

            return null;
        }




        //отримати користувачів компанії за ролями
        public List<EmployeeModel>? GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            List<EmployeeModel> employeesByPosition = new List<EmployeeModel>();
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.employees != null)
                {
                    var employees = company.employees;
                    employeesByPosition = employees
                        .Where(employee => employee.position != null && !string.IsNullOrEmpty(employee.position.namePosition) && employee.position.namePosition.Equals(pos))
                        .ToList();

                    if (employeesByPosition.Any())
                    {
                        return employeesByPosition;
                    }
                }
            }

            return null;
        }


        //створення нової посади
        public List<EmployeePositionModel>? CreatePosition(string[] ar)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.positions != null)
                {
                    EmployeePositionModel newEmployeePosition = new EmployeePositionModel()
                    {
                        namePosition = ar[2]
                    };
                    company.positions.Add(newEmployeePosition);
                    DatabaseMoq.UpdateJson();
                }
            }

            return null;
        }


        //створення нової посади
        public List<EmployeeModel>? EditPosition(string[] ar)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.positions != null)
                {
                    string oldPositionName = ar[2];
                    string newPositionName = ar[3];

                    EmployeePositionModel? position = company.positions.FirstOrDefault(pos => pos.namePosition != null && pos.namePosition.Equals(oldPositionName));

                    if (position != null)
                    {
                        position.namePosition = newPositionName;

                        if (company.employees != null)
                        {
                            foreach (var emp in company.employees.Where(emp => emp.position != null && emp.position.namePosition != null && emp.position.namePosition.Equals(oldPositionName)))
                            {
                                if (emp.position != null)
                                {
                                    emp.position.namePosition = newPositionName;
                                }
                                
                            }
                        }

                        company.positions.RemoveAll(pos => pos.namePosition != null && pos.namePosition.Equals(oldPositionName));
                        DatabaseMoq.UpdateJson();
                    }
                }
            }

            return null;
        }


        //видалення посади
        public List<EmployeeModel>? DeletePosition(string[] ar)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.positions != null)
                {
                    string positionNameToDelete = ar[2];
                    company.positions.RemoveAll(pos => pos.namePosition != null && pos.namePosition.Equals(positionNameToDelete));
                    DatabaseMoq.UpdateJson();
                }
            }

            return null;
        }


        //зміна посади
        public EmployeeModel? EditEmployeePosition(string[] ar)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

                if (company != null && company.employees != null)
                {
                    if (Guid.TryParse(ar[2], out Guid idEmployee))
                    {
                        var employee = company.employees.FirstOrDefault(em => em.id.Equals(idEmployee));

                        if (employee != null && employee.position != null)
                        {
                            employee.position.namePosition = ar[3];
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
