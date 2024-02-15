using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.Settings;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Drawing;

namespace ReportingSystem.Data.JSON
{
    public class JsonWrite
    {
        private class DatabaseData
        {
            public List<CustomerModel>? Customers { get; set; }
            public List<EmployeeModel>? Administrators { get; set; }
            public ConfigurationModel? Configuration { get; set; }
        }

        #region Administrators
        public async Task DeleteAdministrator(string id)
        {
            var administrators = await new JsonRead().GetAdministrators();

            if (administrators == null || !Guid.TryParse(id, out Guid employeeId))
            {
                return;
            }

            var employee = administrators.FirstOrDefault(em => em.Id.Equals(employeeId));

            if (employee != null)
            {
                administrators.Remove(employee);
                UpdateJsonAdministrators(administrators);
            }
        }
        public async Task FromArchiveAdministrator(string id)
        {
            var administrators = await new JsonRead().GetAdministrators();

            if (administrators == null || !Guid.TryParse(id, out Guid employeeId))
            {
                return;
            }

            var employee = administrators.FirstOrDefault(em => em.Id.Equals(employeeId));

            if (employee != null)
            {
                employee.Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = EmployeeStatus.Actual,
                    EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                };
                UpdateJsonAdministrators(administrators);
            }
        }
        public async Task CreateAdministrator(string[] ar)
        {
            var administrators = await new JsonRead().GetAdministrators();

            if (administrators == null)
            {
                return;
            }

            if (!Enum.TryParse(ar[6], out EmployeeRolStatus rolType))
            {
                rolType = EmployeeRolStatus.DevAdministrator;
            }

            var employee = new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Empty,
                CompanyId = Guid.Empty,
                FirstName = ar[0],
                SecondName = ar[1],
                ThirdName = ar[2],
                BirthDate = DateTime.Parse(ar[3]),
                Login = ar[4],
                Rol = new Models.EmployeeRolModel
                {
                    RolName = ar[5],
                    RolType = rolType,
                },
                Password = ar[7],
                PhoneWork = ar[8],
                EmailWork = ar[9],
                Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = EmployeeStatus.Actual,
                    EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                },
            };

            administrators.Add(employee);
            UpdateJsonAdministrators(administrators);
        }
        public async Task ArchiveAdministrator(string[] ar)
        {
            List<EmployeeModel>? administrators = await new JsonRead().GetAdministrators();

            if (administrators == null || !Guid.TryParse(ar[0], out Guid administratorId))
            {
                return;
            }

            var administrator = administrators.FirstOrDefault(ad => ad.Id.Equals(administratorId));

            if (administrator != null)
            {
                administrator.Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = EmployeeStatus.Archive,
                    EmployeeStatusName = EmployeeStatus.Archive.GetDisplayName()
                };
                UpdateJsonAdministrators(administrators);
            }
        }
        public async Task EditAdministrator(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return;
                }

                List<EmployeeModel>? administrators = await new JsonRead().GetAdministrators();

                if (administrators == null || !Guid.TryParse(editedEmployee.Id.ToString(), out Guid id))
                {
                    return;
                }

                var administrator = administrators.FirstOrDefault(c => c.Id.Equals(id));

                if (administrator != null)
                {
                    foreach (var propertyInfo in typeof(EmployeeModel).GetProperties())
                    {
                        var editedValue = propertyInfo.GetValue(editedEmployee);
                        if (editedValue != null)
                        {
                            var employeeProperty = typeof(EmployeeModel).GetProperty(propertyInfo.Name);
                            employeeProperty?.SetValue(administrator, editedValue);
                        }
                    }
                    UpdateJsonAdministrators(administrators);
                    return;
                }
            }
            return;
        }
        public async Task DeleteLicence(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || customers == null)
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(id));

            if (customer == null || customer.StatusLicence == null)
            {
                return;
            }

            customers.Remove(customer);
            UpdateJsonCustomers(customers);
        }
        public async Task ArchivingLicence(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || customers == null)
            {
                return;
            }

            var licence = customers.FirstOrDefault(c => c.Id.Equals(id));

            if (licence == null || licence.StatusLicence == null)
            {
                return;
            }

            licence.StatusLicence = new CustomerLicenceStatusModel
            {
                LicenceType = LicenceType.Archive,
                LicenceName = LicenceType.Archive.GetDisplayName()
            };

            var history = new CustomerLicenseOperationModel
            {
                Id = Guid.NewGuid(),
                IdCustomer = id,
                DateChange = DateTime.Now,
                OldStatus = licence.StatusLicence,
                NewStatus = new CustomerLicenceStatusModel
                {
                    LicenceType = LicenceType.Archive,
                    LicenceName = LicenceType.Archive.GetDisplayName()
                },
                OldEndDateLicence = licence.EndTimeLicense,
                NewEndDateLicence = licence.EndTimeLicense,
                Price = 0,
                Period = "-",
                NameOperation = "Архівування"
            };

            if (licence.HistoryOperations != null && customers != null)
            {
                licence.HistoryOperations.Add(history);
                UpdateJsonCustomers(customers);
            }
        }
        #endregion
        #region Positions
        public async Task CreatePosition(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return;
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
                    UpdateJsonCustomers(customers);
                }
            }

            return;
        }
        public async Task EditPosition(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return;
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
                        UpdateJsonCustomers(customers);
                    }
                }
            }
        }
        public async Task DeletePosition(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company != null && company.Positions != null)
                {
                    string positionNameToDelete = ar[2];
                    company.Positions.RemoveAll(pos => pos.NamePosition != null && pos.NamePosition.Equals(positionNameToDelete));
                    UpdateJsonCustomers(customers);
                }
            }
        }

        #endregion
        #region Rolls
        public async Task EditEmployeeRol(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return;
            }

            if (Guid.TryParse(ar[1], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(idCompany));

                if (company == null || company.Employees == null)
                {
                    return;
                }

                if (Guid.TryParse(ar[2], out Guid idEmployee))
                {
                    var employee = company.Employees.FirstOrDefault(emp => emp.Id.Equals(idEmployee));

                    if (employee != null && employee.Rol != null && company.Rolls != null)
                    {
                        var newRol = company.Rolls.FirstOrDefault(rol => rol.RolName != null && rol.RolName.Equals(ar[3]));

                        if (newRol != null)
                        {
                            employee.Rol = newRol;
                            UpdateJsonCustomers(customers);
                            return;
                        }
                    }
                }
            }

            return;
        }
        #endregion
        #region Customers
        public async Task EditCustomer(string[] ar)
        {
        var customers = await new JsonRead().GetCustomers();

        if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || customers == null)
        {
            return;
        }

        var customer = customers.FirstOrDefault(c => c.Id.Equals(id));

        if (customer == null || customer.StatusLicence == null)
        {
            return;
        }

        if (ar[1] != customer.FirstName)
        {
            customer.FirstName = ar[1];
        }

        if (ar[2] != customer.SecondName)
        {
            customer.SecondName = ar[2];
        }

        if (ar[3] != customer.ThirdName)
        {
            customer.ThirdName = ar[3];
        }

        if (ar[4] != customer.Phone)
        {
            customer.Phone = ar[4];
        }

        if (ar[5] != customer.Email)
        {
            customer.Email = ar[5];
        }

        if (ar[6] != customer.Password)
        {
            customer.Password = ar[6];
        }
        UpdateJsonCustomers(customers);
    }
        public async Task CancellationLicence(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || customers == null)
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(id));

            if (customer == null || customer.StatusLicence == null)
            {
                return;
            }

            var history = new CustomerLicenseOperationModel
            {
                Id = Guid.NewGuid(),
                IdCustomer = id,
                DateChange = DateTime.Now,
                OldStatus = customer.StatusLicence,
                NewStatus = new CustomerLicenceStatusModel
                {
                    LicenceType = LicenceType.Nulled,
                    LicenceName = LicenceType.Nulled.GetDisplayName()
                },
                Price = 0,
                Period = "",
                NameOperation = "Анулювання",
            };

            if (customer.HistoryOperations != null)
            {
                customer.HistoryOperations.Add(history);
                UpdateJsonCustomers(customers);
            }
        }
        public async Task RenewalLicense(string[] ar)
        {
            if (ar.Length < 4 || !Guid.TryParse(ar[0], out Guid id) || !DateTime.TryParse(ar[1], out DateTime desiredDate))
            {
                return;
            }

            var customers = await new JsonRead().GetCustomers();

            if (customers == null)
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(id));

            if (customer == null || customer.StatusLicence == null)
            {
                return;
            }

            var history = new CustomerLicenseOperationModel
            {
                Id = Guid.NewGuid(),
                IdCustomer = id,
                DateChange = DateTime.Now,
                OldStatus = customer.StatusLicence,
                OldEndDateLicence = customer.EndTimeLicense
            };

            customer.StatusLicence = new CustomerLicenceStatusModel
            {
                LicenceType = LicenceType.Main,
                LicenceName = LicenceType.Main.GetDisplayName()
            };

            customer.EndTimeLicense = desiredDate;
            history.NewStatus = customer.StatusLicence;
            history.NewEndDateLicence = customer.EndTimeLicense;

            if (double.TryParse(ar[2].Trim(), out double parsedPrice))
            {
                history.Price = parsedPrice;
            }
            else
            {
                history.Price = 0.0;
            }

            history.Period = ar[3].Trim();
            history.NameOperation = "Продовження";

            if (customer.HistoryOperations != null)
            {
                customer.HistoryOperations.Add(history);
                if (customers != null)
                {
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task RegistrationCustomer(string[] ar)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new();
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                Email = ar[0],
                FirstName = ar[1],
                SecondName = ar[2],
                ThirdName = ar[3],
                Phone = ar[4],
                DateRegistration = DateTime.Today,
                //дилема з паролем, ввід при реєстрації чи відправка на пошту
                Password = EncryptionHelper.Encrypt(ar[5]),/*new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray()),*/
                StatusLicence = new CustomerLicenceStatusModel
                {
                    LicenceType = LicenceType.Test,
                    LicenceName = LicenceType.Test.GetDisplayName()
                },
                Companies = [],
                EndTimeLicense = DateTime.Today.AddDays(30),
                HistoryOperations = []
            };

            var history = new CustomerLicenseOperationModel
            {
                Id = Guid.NewGuid(),
                IdCustomer = customer.Id,
                OldEndDateLicence = DateTime.Today,
                NewEndDateLicence = customer.EndTimeLicense,
                OldStatus = new CustomerLicenceStatusModel(),
                NewStatus = customer.StatusLicence
            };

            customer.HistoryOperations.Add(history);

            var customers = await new JsonRead().GetCustomers();
            if (customers != null)
            {
                customers.Add(customer);
                UpdateJsonCustomers(customers);
            }
        }
        #endregion
        #region Authorization
        #endregion
        #region Employees
        public async Task DeleteEmployee(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid customerId))
            {
                return;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(ar[1], out Guid companyId))
            {
                return;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(ar[2], out Guid employeeId))
            {
                return;
            }

            var employees = company.Employees;
            var employee = employees.FirstOrDefault(em => em.Id.Equals(employeeId));

            if (employee != null)
            {
                employees.Remove(employee);
                UpdateJsonCustomers(customers);
            }
        }
        public async Task FromArchiveEmployee(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid customerId))
            {
                return;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(ar[1], out Guid companyId))
            {
                return;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(ar[2], out Guid employeeId))
            {
                return;
            }

            var employee = company.Employees.FirstOrDefault(em => em.Id.Equals(employeeId));

            if (employee != null)
            {
                employee.Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = Enums.EmployeeStatus.Actual,
                    EmployeeStatusName = Enums.EmployeeStatus.Actual.GetDisplayName()
                };
                UpdateJsonCustomers(customers);
            }
        }
        public async Task CreateEmployee(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length != 21 || !Guid.TryParse(ar[0], out Guid customerId) || !Guid.TryParse(ar[1], out Guid companyId))
            {
                return;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null)
            {
                return;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null)
            {
                return;
            }

            if (!Enum.TryParse(ar[10], out EmployeeRolStatus rolType))
            {
                rolType = EmployeeRolStatus.User;
            }

            var employee = new EmployeeModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CompanyId = companyId,
                FirstName = ar[2],
                SecondName = ar[3],
                ThirdName = ar[4],
                BirthDate = DateTime.Parse(ar[5]),
                WorkStartDate = DateTime.Parse(ar[6]),
                Position = new EmployeePositionModel
                {
                    NamePosition = ar[7]
                },
                Login = ar[8],
                Rol = new Models.EmployeeRolModel
                {
                    RolName = ar[9],
                    RolType = rolType,
                },
                Password = ar[11],
                PhoneWork = ar[12],
                PhoneSelf = ar[13],
                EmailWork = ar[14],
                EmailSelf = ar[15],
                AddressReg = ar[16],
                AddressFact = ar[17],
                Salary = double.TryParse(ar[18], out double parsedSalary) ? parsedSalary : 0.0,
                TaxNumber = ar[19],
                AddSalary = double.TryParse(ar[20], out double parsedAddSalary) ? parsedAddSalary : 0.0,
                Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = EmployeeStatus.Actual,
                    EmployeeStatusName = EmployeeStatus.Actual.GetDisplayName()
                },
            };

            company.Employees.Add(employee);
            UpdateJsonCustomers(customers);
        }
        public async Task ArchiveEmployee(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid customerId))
            {
                return;
            }

            var customer = customers.FirstOrDefault(cu => cu.Id.Equals(customerId));

            if (customer == null || customer.Companies == null || !Guid.TryParse(ar[1], out Guid companyId))
            {
                return;
            }

            var company = customer.Companies.FirstOrDefault(co => co.Id.Equals(companyId));

            if (company == null || company.Employees == null || !Guid.TryParse(ar[2], out Guid employeeId))
            {
                return;
            }

            var employee = company.Employees.FirstOrDefault(em => em.Id.Equals(employeeId));

            if (employee != null)
            {
                employee.Status = new EmployeeStatusModel
                {
                    EmployeeStatusType = EmployeeStatus.Archive,
                    EmployeeStatusName = EmployeeStatus.Archive.GetDisplayName()
                };
                UpdateJsonCustomers(customers);
            }
        }
        public async Task EditEmployee(Object employeeInput)
        {
            if (employeeInput == null)
            {
                return;
            }

            var obj = employeeInput.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(obj);
                if (editedEmployee == null)
                {
                    return;
                }

                List<CustomerModel>? customers = await new JsonRead().GetCustomers();

                if (customers == null || !Guid.TryParse(editedEmployee.CustomerId.ToString(), out Guid customerId))
                {
                    return;
                }

                var customer = customers.FirstOrDefault(c => c.Id.Equals(customerId));

                if (customer == null || customer.Companies == null || !Guid.TryParse(editedEmployee.CompanyId.ToString(), out Guid companyId))
                {
                    return;
                }

                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(companyId));

                if (company == null || company.Employees == null || !Guid.TryParse(editedEmployee.Id.ToString(), out Guid employeeId))
                {
                    return;
                }

                var employee = company.Employees.FirstOrDefault(e => e.Id.Equals(employeeId));

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
                    UpdateJsonCustomers(customers);
                    return;
                }
            }
            return;
        }
        public async Task EditEmployeePosition(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(co => co.Id.Equals(idCustomer));

            if (customer == null || customer.Companies == null)
            {
                return;
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
                            UpdateJsonCustomers(customers);
                        }
                    }
                }
            }
        }
        #endregion
        #region Companies
        public async Task EditCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    company.Name = ar[1];
                    company.Address = ar[2];
                    company.Actions = ar[3];
                    company.Phone = ar[4];
                    company.Email = ar[5];
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task ArchiveCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    company.Status = new CompanyStatusModel
                    {
                        CompanyStatusType = CompanyStatus.Archive,
                        CompanyStatusName = CompanyStatus.Archive.GetDisplayName()
                    };
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task DeleteCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

            if (customer != null && customer.Companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.Companies.FirstOrDefault(c => c.Id.Equals(idCompany));
                if (company != null)
                {
                    customer.Companies.Remove(company);
                    UpdateJsonCustomers(customers);
                }
            }
        }
        public async Task CreateCompany(string[] ar)
        {
            List<CustomerModel>? customers = await new JsonRead().GetCustomers();

            if (ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return;
            }

            var company = new CompanyModel
            {
                Name = ar[0],
                Code = ar[1],
                Address = ar[2],
                Actions = ar[3],
                Phone = ar[4],
                Email = ar[5],
                RegistrationDate = DateTime.Today,
                Rolls = DefaultEmployeeRolls.Get(),
                Positions = [],
                Employees = [],
                Status = new CompanyStatusModel
                {
                    CompanyStatusType = CompanyStatus.Project,
                    CompanyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (customers != null)
            {
                var customer = customers.FirstOrDefault(c => c.Id.Equals(idCustomer));

                if (customer != null && customer.Companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        FirstName = customer.FirstName,
                        SecondName = customer.SecondName,
                        ThirdName = customer.ThirdName,
                        EmailWork = customer.Email
                    };

                    company.Chief = chief;
                    customer.Companies.Add(company);
                    UpdateJsonCustomers(customers);
                }
            }
        }
        #endregion
        #region Categories
        public async Task CreateCategory(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //-----------------------------------------------------------------------------0

            if (ar[2] == null && company.Categories != null)
            {
                ProjectCategoryModel categoryModel = new ProjectCategoryModel();
                categoryModel.Id = Guid.NewGuid();
                categoryModel.Name = ar[6];
                categoryModel.Projects = new List<Guid>();
                categoryModel.CategoriesLevel1 = new List<ProjectCategoryModel1>();
                company.Categories.Add(categoryModel);
                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));

                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }
                ProjectCategoryModel1 categoryModel1 = new ProjectCategoryModel1();
                categoryModel1.Id = Guid.NewGuid();
                categoryModel1.Name = ar[6];
                categoryModel1.Projects = new List<Guid>();
                categoryModel1.CategoriesLevel2 = new List<ProjectCategoryModel2>();
                cat0.CategoriesLevel1.Add(categoryModel1);
                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }
                ProjectCategoryModel2 categoryModel2 = new ProjectCategoryModel2();
                categoryModel2.Id = Guid.NewGuid();
                categoryModel2.Name = ar[6];
                categoryModel2.Projects = new List<Guid>();
                categoryModel2.CategoriesLevel3 = new List<ProjectCategoryModel3>();
                cat1.CategoriesLevel2.Add(categoryModel2);
                UpdateJsonCustomers(customers);
                return;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return;
                }

                ProjectCategoryModel3 categoryModel3 = new ProjectCategoryModel3();
                categoryModel3.Id = Guid.NewGuid();
                categoryModel3.Name = ar[6];
                categoryModel3.Projects = new List<Guid>();
                cat2.CategoriesLevel3.Add(categoryModel3);
                UpdateJsonCustomers(customers);
                return;
            }
        }
        public async Task DeleteCategory(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();
            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                company.Categories.Remove(cat0);
                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                cat0.CategoriesLevel1.Remove(cat1);

                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                cat1.CategoriesLevel2.Remove(cat2);
                UpdateJsonCustomers(customers);
                return;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return;
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return;
                }
                cat2.CategoriesLevel3.Remove(cat3);
                UpdateJsonCustomers(customers);
                return;

            }

            return;
        }
        public async Task EditNameCategory(string[] ar)
        {
            var customers = await new JsonRead().GetCustomers();

            if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
            {
                return;
            }

            var customer = customers.First(cu => cu.Id.Equals(idCustomer));

            if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
            {
                return;
            }

            var company = customer.Companies.First(co => co.Id.Equals(idCompany));

            //---------------------------------------------------------

            if (ar[2] != null && ar[3] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                cat0.Name = ar[6];
                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------1

            if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
            {

                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));


                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                cat1.Name = ar[6];
                UpdateJsonCustomers(customers);
                return;
            }

            //-----------------------------------------------------------------------------2

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                cat2.Name = ar[6];
                UpdateJsonCustomers(customers);
                return;

            }

            //-----------------------------------------------------------------------------3

            if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] != null && company.Categories != null)
            {
                if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3) || !Guid.TryParse(ar[5], out Guid idCatLevel4))
                {
                    return;
                }

                var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
                if (cat0.CategoriesLevel1 == null)
                {
                    return;
                }

                var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
                if (cat1.CategoriesLevel2 == null)
                {
                    return;
                }

                var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
                if (cat2.CategoriesLevel3 == null)
                {
                    return;
                }

                var cat3 = cat2.CategoriesLevel3.First(ca1 => ca1.Id.Equals(idCatLevel4));
                if (cat3 == null)
                {
                    return;
                }
                cat3.Name = ar[6];
                UpdateJsonCustomers(customers);
                return;
            }
            return;
        }
        #endregion
        #region Projects
        public async Task CreateProject(string[] ar)
        {
        var customers = await new JsonRead().GetCustomers();

        if (customers == null || !Guid.TryParse(ar[0], out Guid idCustomer))
        {
            return;
        }

        var customer = customers.First(cu => cu.Id.Equals(idCustomer));

        if (customer.Companies == null || !Guid.TryParse(ar[1], out Guid idCompany))
        {
            return;
        }

        var company = customer.Companies.First(co => co.Id.Equals(idCompany));

        //-----------------------------------------------------------------------------0

        if (ar[2] == null && company.Categories != null)
        {
            ProjectCategoryModel categoryModel = new()
            {
                Id = Guid.NewGuid(),
                Name = ar[6],
                Projects = [],
                CategoriesLevel1 = []
            };
            company.Categories.Add(categoryModel);
            UpdateJsonCustomers(customers);
            return;
        }

        //-----------------------------------------------------------------------------1

        if (ar[2] != null && ar[3] == null && company.Categories != null)
        {

            if (!Guid.TryParse(ar[2], out Guid idCatLevel1))
            {
                return;
            }

            var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));

            if (cat0.CategoriesLevel1 == null)
            {
                return;
            }
            ProjectCategoryModel1 categoryModel1 = new()
            {
                Id = Guid.NewGuid(),
                Name = ar[6],
                Projects = [],
                CategoriesLevel2 = []
            };
            cat0.CategoriesLevel1.Add(categoryModel1);
            UpdateJsonCustomers(customers);
        }

        //-----------------------------------------------------------------------------

        if (ar[2] != null && ar[3] != null && ar[4] == null && company.Categories != null)
        {
            if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2))
            {
                return;
            }

            var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
            if (cat0.CategoriesLevel1 == null)
            {
                return;
            }

            var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));

            if (cat1.CategoriesLevel2 == null)
            {
                return;
            }
            ProjectCategoryModel2 categoryModel2 = new()
            {
                Id = Guid.NewGuid(),
                Name = ar[6],
                Projects = [],
                CategoriesLevel3 = []
            };
            cat1.CategoriesLevel2.Add(categoryModel2);
            UpdateJsonCustomers(customers);


        }

        //-----------------------------------------------------------------------------

        if (ar[2] != null && ar[3] != null && ar[4] != null && ar[5] == null && company.Categories != null)
        {
            if (!Guid.TryParse(ar[2], out Guid idCatLevel1) || !Guid.TryParse(ar[3], out Guid idCatLevel2) || !Guid.TryParse(ar[4], out Guid idCatLevel3))
            {
                return;
            }

            var cat0 = company.Categories.First(ca1 => ca1.Id.Equals(idCatLevel1));
            if (cat0.CategoriesLevel1 == null)
            {
                return;
            }

            var cat1 = cat0.CategoriesLevel1.First(ca1 => ca1.Id.Equals(idCatLevel2));
            if (cat1.CategoriesLevel2 == null)
            {
                return;
            }

            var cat2 = cat1.CategoriesLevel2.First(ca1 => ca1.Id.Equals(idCatLevel3));
            if (cat2.CategoriesLevel3 == null)
            {
                return;
            }

            ProjectCategoryModel3 categoryModel3 = new()
            {
                Id = Guid.NewGuid(),
                Name = ar[6],
                Projects = []
            };
            cat2.CategoriesLevel3.Add(categoryModel3);
            UpdateJsonCustomers(customers);
        }
    }
        #endregion
        #region Save
        private void UpdateJsonCustomers(List<CustomerModel> customers)
        {
            var data = new DatabaseData
            {
                Customers = customers,
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        public void UpdateJsonAppsettings(string[] ar)
        {
            string json = File.ReadAllText("appsettings.json");
            RootObject? root = JsonConvert.DeserializeObject<RootObject>(json);
            if (root!=null && root.ConnectionSettings != null)
            {
                root.ConnectionSettings.Server = ar[0];
                root.ConnectionSettings.DB = ar[1];
                root.ConnectionSettings.Login = ar[2];
                root.ConnectionSettings.Password = ar[3];
                string newJson = JsonConvert.SerializeObject(root, Formatting.Indented);
                File.WriteAllText("appsettings.json", newJson);
            }
        }
        private static void UpdateJsonAdministrators(List<EmployeeModel> administrators)
        {
            var data = new DatabaseData
            {
                Administrators = administrators
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        private void UpdateJsonConfiguration(ConfigurationModel configuration)
        {
            var data = new DatabaseData
            {
                Configuration = configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        private static void UpdateJsonAll(DatabaseData DatabaseData)
        {
            var data = new DatabaseData
            {
                Customers = DatabaseData.Customers,
                Administrators = DatabaseData.Administrators,
                Configuration = DatabaseData.Configuration
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
        }
        #endregion
    }
}
