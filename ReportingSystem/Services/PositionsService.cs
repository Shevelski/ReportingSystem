using ReportingSystem.Models.User;

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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null)
                {
                    return company.Positions;
                }
            }

            return null;
        }

        //Отримання списку посад компанії 
        public List<EmployeePositionEmpModel>? GetAllPositionsWithEmployee(string idCu, string idCo)
        {
            List<EmployeePositionEmpModel>? positions = new List<EmployeePositionEmpModel>();

            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Positions == null || company.Employees == null)
                {
                    return null;
                }
                int i = 0;
                foreach (var item in company.Positions)
                {
                    
                    EmployeePositionEmpModel employeePositionEmpModel = new EmployeePositionEmpModel();
                    employeePositionEmpModel.NamePosition = item.NamePosition;
                    employeePositionEmpModel.Employee = company.Employees[i];
                    positions.Add(employeePositionEmpModel);
                    i++;
                }
                i = 0;
                return positions;
              
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    var uniquePositions = company.Positions
                        .Where(position => !string.IsNullOrEmpty(position.NamePosition))
                        .GroupBy(position => position.NamePosition)
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(idCo, out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Employees != null)
                {
                    var employees = company.Employees;
                    employeesByPosition = employees
                        .Where(employee => employee.Position != null && !string.IsNullOrEmpty(employee.Position.NamePosition) && employee.Position.NamePosition.Equals(pos))
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    EmployeePositionModel newEmployeePosition = new EmployeePositionModel()
                    {
                        NamePosition = ar[2]
                    };
                    company.Positions.Add(newEmployeePosition);
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    string oldPositionName = ar[2];
                    string newPositionName = ar[3];

                    EmployeePositionModel? position = company.Positions.FirstOrDefault(pos => pos.NamePosition != null && pos.NamePosition.Equals(oldPositionName));

                    if (position != null)
                    {
                        position.NamePosition = newPositionName;

                        if (company.Employees != null)
                        {
                            foreach (var emp in company.Employees.Where(emp => emp.Position != null && emp.Position.NamePosition != null && emp.Position.NamePosition.Equals(oldPositionName)))
                            {
                                if (emp.Position != null)
                                {
                                    emp.Position.NamePosition = newPositionName;
                                }
                                
                            }
                        }

                        company.Positions.RemoveAll(pos => pos.NamePosition != null && pos.NamePosition.Equals(oldPositionName));
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    string positionNameToDelete = ar[2];
                    company.Positions.RemoveAll(pos => pos.NamePosition != null && pos.NamePosition.Equals(positionNameToDelete));
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

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Employees != null)
                {
                    if (Guid.TryParse(ar[2], out Guid idEmployee))
                    {
                        var employee = company.Employees.FirstOrDefault(em => em.Id.Equals(idEmployee));

                        if (employee != null && employee.Position != null)
                        {
                            employee.Position.NamePosition = ar[3];
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
