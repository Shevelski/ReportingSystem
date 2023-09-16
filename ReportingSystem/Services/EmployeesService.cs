using Newtonsoft.Json;
using ReportingSystem.Enums;
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
                                employeeStatusType = EmployeeStatus.Archive,
                                employeeStatusName = EmployeeStatus.Archive.GetDisplayName()
                            };
                            DatabaseMoq.UpdateJson();
                            return (employee);
                        }
                    }
                }
            }
            return null;
        }

        // Перевірка на доступність email
        public bool? IsBusyEmail(string email)
        {
            customers = DatabaseMoq.Customers;

            if (customers != null)
            {
                foreach (CustomerModel customer in customers)
                {
                    if (customer.companies != null)
                    {
                        if (customer.email != null && customer.email.ToLower().Equals(email.ToLower()))
                        {
                            return true;
                        }
                        
                        companies = customer.companies;
                        foreach(CompanyModel company in companies)
                        {
                            if (company.employees != null)
                            {
                                employees = company.employees;
                                foreach(EmployeeModel employee in employees)
                                {
                                   if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Додавання співробітників
        public EmployeeModel? CreateEmployee(string[] ar)
        {
            customers = DatabaseMoq.Customers;
            if (customers != null && Guid.TryParse(ar[0], out Guid customerId))
            {
                customer = customers.First(cu => cu.id.Equals(customerId));
                if (customer.companies != null && Guid.TryParse(ar[1], out Guid companyId))
                {
                    company = customer.companies.First(co => co.id.Equals(companyId));
                    
                    if (company.employees != null)
                    {
                        EmployeeRolStatus rolTypeVar = EmployeeRolStatus.User;
                        if (Enum.TryParse(ar[10], out EmployeeRolStatus rolType))
                        {
                            rolTypeVar = rolType;
                        }


                        employee = new EmployeeModel
                        {
                            id = Guid.NewGuid(),
                            customerId = customerId,
                            companyId = companyId,
                            firstName = ar[2],
                            secondName = ar[3],
                            thirdName = ar[4],
                            birthDate = DateTime.Parse(ar[5]),
                            workStartDate = DateTime.Parse(ar[6]),
                            position = new EmployeePositionModel
                            {
                                namePosition = ar[7]
                            },
                            login = ar[8],
                            rol = new Models.EmployeeRolModel
                            {
                                rolName = ar[9],
                                rolType = rolTypeVar,
                            },
                            password = ar[11],
                            phoneWork = ar[12],
                            phoneSelf = ar[13],
                            emailWork = ar[14],
                            emailSelf = ar[15],
                            addressReg = ar[16],
                            addressFact = ar[17],
                            salary = double.Parse(ar[18]),
                            taxNumber = ar[19],
                            addSalary = double.Parse(ar[20]),
                            status = new EmployeeStatusModel
                            {
                                employeeStatusType = EmployeeStatus.Actual,
                                employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                            },
                        };
                        company.employees.Add(employee);
                        DatabaseMoq.UpdateJson();
                        return (employee); 
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
        public void DeleteEmployee(string idCu, string idCo, string idEm)
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
        }
    }
}
