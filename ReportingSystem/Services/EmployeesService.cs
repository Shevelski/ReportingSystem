using Newtonsoft.Json;
using ReportingSystem.Data.SQL;
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

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return null;
            }

            var companies = customer.Companies;

            if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }
            
            var company = companies.FirstOrDefault(comp => comp.Id.Equals(idCompany));
            
            if (company == null)
            {
                return null;
            }
            List<EmployeeModel> list  = [];
            if (company.Employees == null)
            {
                return null;
            }
                list = company.Employees;

            //foreach (var employee in list)
            //{
            //    if (employee.password != null)
            //    {
            //        employee.password = EncryptionHelper.Decrypt(employee.password);
            //    }
            //}       
            
            return list;
        }

        public List<EmployeeModel>? GetAdministrators()
        {
            var administrators = DatabaseMoq.Administrators;

            if (administrators == null)
            {
                return null;
            }

            List<EmployeeModel> list = administrators;

            //foreach (var employee in list)
            //{
            //    if (employee.password != null)
            //    {
            //        employee.password = EncryptionHelper.Decrypt(employee.password);
            //    }
            //}

            return list;
        }

        public async Task<EmployeeModel?> GetEmployee(string idCu, string idCo, string idEm)
        {
            if (Utils.Settings.Source().Equals("json"))
            {
                if (idCu == Guid.Empty.ToString() && idCo == Guid.Empty.ToString() && idEm != Guid.Empty.ToString())
                {

                    var developers = DatabaseMoq.Administrators;

                    if (developers == null || !Guid.TryParse(idEm, out Guid idDeveloper))
                    {
                        return null;
                    }

                    var developer = developers.First(dev => dev.id.Equals(idDeveloper));

                    //var dev = developer;

                    //if (dev != null && dev.password != null)
                    //{
                    //    dev.password = EncryptionHelper.Decrypt(dev.password);
                    //}

                    //return dev;
                    return developer;
                }

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

                var companies = customer.Companies;

                if (companies == null || !Guid.TryParse(idCo, out Guid idCompany))
                {
                    return null;
                }

                var company = companies.FirstOrDefault(comp => comp.Id.Equals(idCompany));

                if (company == null)
                {
                    return null;
                }

                var employees = company.Employees;

                if (employees == null || !Guid.TryParse(idEm, out Guid idEmployee))
                {
                    return null;
                }

                var employee = employees.FirstOrDefault(comp => comp.id.Equals(idEmployee));

                return employee;
                //var empl = employee;

                //if (empl != null && empl.password != null)
                //{
                //    empl.password = EncryptionHelper.Decrypt(empl.password);
                //}

                //return empl;
            } else
            {
                if (!Guid.TryParse(idCu, out Guid idCustomer) || !Guid.TryParse(idCo, out Guid idCompany) || !Guid.TryParse(idEm, out Guid idEmployee))
                {
                    return null;
                }

                EmployeeModel employee = await new SQLRead().GetEmployee(idCustomer, idCompany, idEmployee);
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

                var customer = customers.FirstOrDefault(c => c.Id.Equals(customerId));

                if (customer == null || customer.Companies == null || !Guid.TryParse(editedEmployee.companyId.ToString(), out Guid companyId))
                {
                    return null;
                }

                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(companyId));

                if (company == null || company.Employees == null || !Guid.TryParse(editedEmployee.id.ToString(), out Guid employeeId))
                {
                    return null;
                }

                var employee = company.Employees.FirstOrDefault(e => e.id.Equals(employeeId));

                if (employee != null)
                {
                    foreach (var propertyInfo in typeof(EmployeeModel).GetProperties())
                    {
                        var editedValue = propertyInfo.GetValue(editedEmployee);
                        if (editedValue != null)
                        {
                            var employeeProperty = typeof(EmployeeModel).GetProperty(propertyInfo.Name);
                            employeeProperty?.SetValue(employee, editedValue);
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
                            employeeProperty?.SetValue(employee, editedValue);
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

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return null;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = company.Employees.FirstOrDefault(em => em.id.Equals(employeeId));

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
                if (administrator.emailWork != null && administrator.emailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
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
                if (customer.Email != null && customer.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                if (customer.Companies != null)
                {
                    foreach (var company in customer.Companies)
                    {
                        if (company.Employees != null)
                        {
                            foreach (var employee in company.Employees)
                            {
                                if (employee.emailWork != null && employee.emailWork.Equals(email, StringComparison.CurrentCultureIgnoreCase))
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

                var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

                if (customer == null || customer.Companies == null)
                {
                    return null;
                }

                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

                if (company == null || company.Employees == null)
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

                company.Employees.Add(employee);
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

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return null;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return null;
            }

            var employee = company.Employees.FirstOrDefault(em => em.id.Equals(employeeId));

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

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(idCo, out Guid companyId))
            {
                return;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(idEm, out Guid employeeId))
            {
                return;
            }

            var employees = company.Employees;
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
