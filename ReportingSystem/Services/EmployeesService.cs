using Newtonsoft.Json;
using ReportingSystem.Data;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.User;

namespace ReportingSystem.Services
{
    public class EmployeesService
    {


        public List<EmployeeModel>? GetEmployees(string idCu, string idCo)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            var companies = customer.companies;

            if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }
            
            var company = companies.FirstOrDefault(comp => comp.id.Equals(idCompany));
            
            if (company == null)
            {
                return null;
            }

            return company.employees;
        }

        public List<EmployeeModel>? GetAdministrators()
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null)
            {
                return null;
            }
            
            return administrators.ToList();
        }

        public async Task<EmployeeModel?> GetEmployee(string idCu, string idCo, string idEm)
        {
            if (Utils.Mode.Read().Equals("json"))
            {
                if (idCu == Guid.Empty.ToString() && idCo == Guid.Empty.ToString() && idEm != Guid.Empty.ToString())
                {

                    var developers = DatabaseMoq.Administrators;

                    if (developers == null || !Guid.TryParse(idEm, out Guid idDeveloper))
                    {
                        return null;
                    }

                    var developer = developers.First(dev => dev.id.Equals(idDeveloper));
                    return developer;
                }

                var customers = DatabaseMoq.Customers;

                if (customers == null || !Guid.TryParse(idCu, out Guid idCustomer))
                {
                    return null;
                }

                var customer = customers.FirstOrDefault(cu => cu.id.Equals(idCustomer));

                if (customer == null || customer.companies == null)
                {
                    return null;
                }

                var companies = customer.companies;

                if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
                {
                    return null;
                }

                var company = companies.FirstOrDefault(comp => comp.id.Equals(idCompany));

                if (company == null)
                {
                    return null;
                }

                var employees = company.employees;

                if (employees == null || !Guid.TryParse(idEm, out Guid idEmployee))
                {
                    return null;
                }

                var employee = employees.FirstOrDefault(comp => comp.id.Equals(idEmployee));

                return employee;
            } else
            {
                if (!Guid.TryParse(idCu, out Guid idCustomer) || !Guid.TryParse(idCo, out Guid idCompany) || !Guid.TryParse(idEm, out Guid idEmployee))
                {
                    return null;
                }

                EmployeeModel employee = await new UserOperations().GetEmployeeData(idCustomer, idCompany, idEmployee);
                return employee;
            }

        }

        //Редагування співробітника
        public EmployeeModel? EditEmployee(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return null;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return null;
                }

                var customers = DatabaseMoq.Customers;

                if (customers == null || !Guid.TryParse(editedEmployee.customerId.ToString(), out Guid customerId))
                {
                    return null;
                }

                var customer = customers.FirstOrDefault(c => c.id.Equals(customerId));

                if (customer == null || customer.companies == null || !Guid.TryParse(editedEmployee.companyId.ToString(), out Guid companyId))
                {
                    return null;
                }

                var company = customer.companies.FirstOrDefault(c => c.id.Equals(companyId));

                if (company == null || company.employees == null || !Guid.TryParse(editedEmployee.id.ToString(), out Guid employeeId))
                {
                    return null;
                }

                var employee = company.employees.FirstOrDefault(e => e.id.Equals(employeeId));

                if (employee != null)
                {
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
                    return employee;
                }
            }
            return null;
        }

        //Редагування співробітника
        public EmployeeModel? EditAdministrator(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return null;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return null;
                }

                var admins = DatabaseMoq.Administrators;

                if (admins == null || !Guid.TryParse(editedEmployee.id.ToString(), out Guid employeeId))
                {
                    return null;
                }

                var employee = admins.FirstOrDefault(e => e.id.Equals(employeeId));

                if (employee != null)
                {
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
                    return employee;
                }
            }
            return null;
        }


        // Архівування співробітників
        public EmployeeModel? ArchiveEmployee(string idCu, string idCo, string idEm)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid customerId))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.id.Equals(customerId));

            if (customer == null || customer.companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return null;
            }

            var company = customer.companies.FirstOrDefault(co => co.id.Equals(companyId));

            if (company == null || company.employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = company.employees.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                employee.status = new EmployeeStatusModel
                {
                    employeeStatusType = EmployeeStatus.Archive,
                    employeeStatusName = EmployeeStatus.Archive.GetDisplayName()
                };
                DatabaseMoq.UpdateJson();
                return employee;
            }

            return null;
        }


        // Архівування співробітників
        public EmployeeModel? ArchiveAdministrator(string idEm)
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = administrators.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                employee.status = new EmployeeStatusModel
                {
                    employeeStatusType = EmployeeStatus.Archive,
                    employeeStatusName = EmployeeStatus.Archive.GetDisplayName()
                };
                DatabaseMoq.UpdateJson();
                return employee;
            }

            return null;
        }


        // Перевірка на доступність email
        public bool IsBusyEmail(string email)
        {

            var administrators = DatabaseMoq.Administrators;

            if (administrators == null)
            {
                return false;
            }

            foreach (var administrator in administrators)
            {
                if (administrator.emailWork != null && administrator.emailWork.ToLower() == email.ToLower())
                {
                    return true;
                }
            }

            var customers = DatabaseMoq.Customers;

            if (customers == null)
            {
                return false;
            }

            foreach (var customer in customers)
            {
                if (customer.email != null && customer.email.ToLower() == email.ToLower())
                {
                    return true;
                }

                if (customer.companies != null)
                {
                    foreach (var company in customer.companies)
                    {
                        if (company.employees != null)
                        {
                            foreach (var employee in company.employees)
                            {
                                if (employee.emailWork != null && employee.emailWork.ToLower() == email.ToLower())
                                {
                                    return true;
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
                var customers = DatabaseMoq.Customers;

                if (customers == null || ar.Length != 21 || !Guid.TryParse(ar[0], out Guid customerId) || !Guid.TryParse(ar[1], out Guid companyId))
                {
                    return null;
                }

                var customer = customers.FirstOrDefault(cu => cu.id.Equals(customerId));

                if (customer == null || customer.companies == null)
                {
                    return null;
                }

                var company = customer.companies.FirstOrDefault(co => co.id.Equals(companyId));

                if (company == null || company.employees == null)
                {
                    return null;
                }

                if (!Enum.TryParse(ar[10], out EmployeeRolStatus rolType))
                {
                    rolType = EmployeeRolStatus.User;
                }

                var employee = new EmployeeModel
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
                        rolType = rolType,
                    },
                    password = ar[11],
                    phoneWork = ar[12],
                    phoneSelf = ar[13],
                    emailWork = ar[14],
                    emailSelf = ar[15],
                    addressReg = ar[16],
                    addressFact = ar[17],
                    salary = double.TryParse(ar[18], out double parsedSalary) ? parsedSalary : 0.0,
                    taxNumber = ar[19],
                    addSalary = double.TryParse(ar[20], out double parsedAddSalary) ? parsedAddSalary : 0.0,
                    status = new EmployeeStatusModel
                    {
                        employeeStatusType = EmployeeStatus.Actual,
                        employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                    },
                };

                company.employees.Add(employee);
                DatabaseMoq.UpdateJson();
                return employee;
        }


        // Додавання співробітників
        public EmployeeModel? CreateAdministrator(string[] ar)
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null)
            {
                return null;
            }

            if (!Enum.TryParse(ar[6], out EmployeeRolStatus rolType))
            {
                rolType = EmployeeRolStatus.DevAdministrator;
            }

            var employee = new EmployeeModel
            {
                id = Guid.NewGuid(),
                customerId = Guid.Empty,
                companyId = Guid.Empty,
                firstName = ar[0],
                secondName = ar[1],
                thirdName = ar[2],
                birthDate = DateTime.Parse(ar[3]),
                login = ar[4],
                rol = new Models.EmployeeRolModel
                {
                    rolName = ar[5],
                    rolType = rolType,
                },
                password = ar[7],
                phoneWork = ar[8],
                emailWork = ar[9],
                status = new EmployeeStatusModel
                {
                    employeeStatusType = EmployeeStatus.Actual,
                    employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                },
            };

            administrators.Add(employee);
                DatabaseMoq.UpdateJson();
                return employee;
        }


        // Відновлення співробітників з архіву
        public EmployeeModel? FromArchiveEmployee(string idCu, string idCo, string idEm)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid customerId))
            {
                return null;
            }

            var customer = customers.FirstOrDefault(cu => cu.id.Equals(customerId));

            if (customer == null || customer.companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return null;
            }

            var company = customer.companies.FirstOrDefault(co => co.id.Equals(companyId));

            if (company == null || company.employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = company.employees.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                employee.status = new EmployeeStatusModel
                {
                    employeeStatusType = Enums.EmployeeStatus.Actual,
                    employeeStatusName = Enums.EmployeeStatus.Actual.GetDisplayName()
                };
                DatabaseMoq.UpdateJson();
                return employee;
            }
            return null;
        }


        // Відновлення співробітників з архіву
        public EmployeeModel? FromArchiveAdministrator(string idEm)
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = administrators.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                employee.status = new EmployeeStatusModel
                {
                    employeeStatusType = EmployeeStatus.Actual,
                    employeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                };
                DatabaseMoq.UpdateJson();
                return employee;
            }
            return null;
        }


        // Видалення співробітників з системи
        public void DeleteEmployee(string idCu, string idCo, string idEm)
        {
            var customers = DatabaseMoq.Customers;

            if (customers == null || !Guid.TryParse(idCu, out Guid customerId))
            {
                return;
            }

            var customer = customers.FirstOrDefault(cu => cu.id.Equals(customerId));

            if (customer == null || customer.companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return;
            }

            var company = customer.companies.FirstOrDefault(co => co.id.Equals(companyId));

            if (company == null || company.employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return;
            }

            var employees = company.employees;
            var employee = employees.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                employees.Remove(employee);
                DatabaseMoq.UpdateJson();
            }
        }


        // Видалення співробітників з системи
        public void DeleteAdministrator(string idEm)
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return;
            }

            var employee = administrators.FirstOrDefault(em => em.id.Equals(employeeId));

            if (employee != null)
            {
                administrators.Remove(employee);
                DatabaseMoq.UpdateJson();
            }
        }
    }
}
