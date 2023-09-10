using Newtonsoft.Json;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;

namespace ReportingSystem.Services
{
    public class EmployeesService
    {
        List<CustomerModel>? customers = DatabaseMoq.Customers;
        CustomerModel? customer = new CustomerModel();
        CompanyModel? company = new CompanyModel();
        List<CompanyModel>? companies = new List<CompanyModel>();
        EmployeeModel? employee = new EmployeeModel();
        List<EmployeeModel>? employees = new List<EmployeeModel>();

        public List<EmployeeModel>? GetEmployees(string idCu, string idCo)
        {

            if (customers != null && Guid.TryParse(idCu, out Guid idCustomer))
            {
                customer = customers.First(cu => cu.id.Equals(idCustomer));
                companies = customer.companies;
                
                if (companies != null && Guid.TryParse(idCo, out Guid companyIdGuid))
                {
                    company = companies.First(company => company.id == companyIdGuid);
                    if (company != null)
                    {
                        return company.employees;
                    }
                }
                else
                {
                    if (companies != null)
                    {
                        company = companies[0];
                        return company.employees;
                    }
                }
            }
            return null;
        }

        //Редагування співробітника
        public EmployeeModel? EditEmployee(Object employeeInput)
        {
            if (employeeInput != null)
            {
                var obj = employeeInput.ToString();
                EmployeeModel? editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);

                if (editedEmployee != null)
                {
                    customers = DatabaseMoq.Customers;
                    if (customers != null)
                    {
                        customer = customers.First(c => c.id.Equals(editedEmployee.customerId));
                        if (customer != null)
                        {
                            companies = customer.companies;
                            if (companies != null)
                            {
                                company = companies.First(c => c.id.Equals(editedEmployee.companyId));
                                if (company.employees != null)
                                {
                                    employee = company.employees.First(e => e.id.Equals(editedEmployee.id));

                                    foreach (var propertyInfo in typeof(EmployeeModel).GetProperties())
                                    {
                                        var editedValue = propertyInfo.GetValue(editedEmployee);
                                        if (editedValue != null)
                                        {
                                            var employeeProperty = typeof(EmployeeModel).GetProperty(propertyInfo.Name);
                                            if (employeeProperty != null)
                                            {
                                                employeeProperty.SetValue(employee, editedValue);
                                            }
                                        }
                                    }
                                    DatabaseMoq.UpdateJson();
                                    return (employee);
                                }
                            }
                        }
                        return employee;
                    }
                }
            }
            return null;
        }

        // Архівування співробітників
        public EmployeeModel? ArchiveEmployee(string idCu, string idCo, string idEm)
        {
            customers = DatabaseMoq.Customers;
            if (customers != null && Guid.TryParse(idCu, out Guid customerId))
            {
                customer = customers.First(cu => cu.id.Equals(customerId));
                if (customer.companies != null && Guid.TryParse(idCo, out Guid companyId))
                {
                    company = customer.companies.First(co => co.id.Equals(companyId));
                    if (company.employees != null && Guid.TryParse(idEm, out Guid employeeId))
                    {
                        employee = company.employees.First(em => em.id.Equals(employeeId));
                        if (employee != null)
                        {
                            employee.status = new EmployeeStatusModel
                            {
                                employeeStatusType = Enums.EmployeeStatus.Archive,
                                employeeStatusName = Enums.EmployeeStatus.Archive.GetDisplayName()
                            };
                            DatabaseMoq.UpdateJson();
                            return (employee);
                        }
                        
                    }
                }
            }
            return null;
        }

        // Відновлення співробітників з архіву
        public EmployeeModel? FromArchiveEmployee(string idCu, string idCo, string idEm)
        {
            customers = DatabaseMoq.Customers;
            if (customers != null && Guid.TryParse(idCu, out Guid customerId))
            {
                customer = customers.First(cu => cu.id.Equals(customerId));
                if (customer.companies != null && Guid.TryParse(idCo, out Guid companyId))
                {
                    company = customer.companies.First(co => co.id.Equals(companyId));
                    if (company.employees != null && Guid.TryParse(idEm, out Guid employeeId))
                    {
                        employee = company.employees.First(em => em.id.Equals(employeeId));
                        if (employee != null)
                        {
                            employee.status = new EmployeeStatusModel
                            {
                                employeeStatusType = Enums.EmployeeStatus.Actual,
                                employeeStatusName = Enums.EmployeeStatus.Actual.GetDisplayName()
                            };
                            DatabaseMoq.UpdateJson();
                            return (employee);
                        }
                        
                    }
                }
            }
            return null;
        }
        
        // Видалення співробітників з системи
        public EmployeeModel? DeleteEmployee(string idCu, string idCo, string idEm)
        {
            customers = DatabaseMoq.Customers;
            if (customers != null && Guid.TryParse(idCu, out Guid customerId))
            {
                customer = customers.First(cu => cu.id.Equals(customerId));
                if (customer.companies != null && Guid.TryParse(idCo, out Guid companyId))
                {
                    company = customer.companies.First(co => co.id.Equals(companyId));
                    if (company.employees != null && Guid.TryParse(idEm, out Guid employeeId))
                    {
                        employees = company.employees;

                        employee = company.employees.First(em => em.id.Equals(employeeId));
                        employees.Remove(employee);
                        DatabaseMoq.UpdateJson();
                    }
                }
            }
            return null;
        }
    }
}
