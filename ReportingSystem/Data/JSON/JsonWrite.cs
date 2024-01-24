﻿using Newtonsoft.Json;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Configuration;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

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

        public async Task DeleteAdministrator(string id)
        {
            var administrators = await new JsonRead().LoadAdministratorsFromJson();

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

        public async Task DeleteEmployee(string[] ar)
        {
            var customers = await new JsonRead().LoadCustomersFromJson();

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

        public async Task FromArchiveAdministrator(string id)
        {
            var administrators = await new JsonRead().LoadAdministratorsFromJson();

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

        public async Task FromArchiveEmployee(string[] ar)
        {
            var customers = await new JsonRead().LoadCustomersFromJson();

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

        public async Task CreateAdministrator(string[] ar)
        {
            var administrators = await new JsonRead().LoadAdministratorsFromJson();

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

        public async Task CreateEmployee(string[] ar)
        {
            var customers = await new JsonRead().LoadCustomersFromJson();

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
            List<CustomerModel>? customers = await new JsonRead().LoadCustomersFromJson();

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

        public async Task ArchiveAdministrator(string[] ar)
        {
            List<EmployeeModel>? administrators = await new JsonRead().LoadAdministratorsFromJson();

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

                List<CustomerModel>? customers = await new JsonRead().LoadCustomersFromJson();

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

                List<EmployeeModel>? administrators = await new JsonRead().LoadAdministratorsFromJson();

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
            Random random = new ();
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
        public async Task EditCompanyJson(string[] ar)
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


        private void UpdateJsonCustomers(List<CustomerModel> customers)
        {
            var data = new DatabaseData
            {
                Customers = customers,
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Context.Json, jsonData);
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
    }
}
