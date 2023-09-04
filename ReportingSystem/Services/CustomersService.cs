using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;

namespace ReportingSystem.Services
{
    public class CustomersService
    {

        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();
        Random random = new Random();

        public List<CustomerModel>? GetCustomers()
        {
            var customers = DatabaseMoq.Customers;
            return customers;
        }

        public CustomerModel? CreateCustomer(string email)
        {
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            customer.id = Guid.NewGuid();
            customer.email = email;
            customer.password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            customer.statusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Test,
                licenceName = LicenceType.Test.GetDisplayName()
            };
            customer.companies = new List<CompanyModel>();
            customer.endTimeLicense = DateTime.Today.AddDays(30);

            CustomerLicenseOperationModel history = new CustomerLicenseOperationModel()
            {
                oldEndDateLicence = DateTime.Today,
                newEndDateLicence = customer.endTimeLicense,
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = customer.statusLicence,
            };
            if (customer.historyOperations != null)
            {
                customer.historyOperations.Add(history);
                if (DatabaseMoq.Customers != null)
                {
                    customers = DatabaseMoq.Customers;
                    customers.Add(customer);
                    DatabaseMoq.UpdateJson();
                    return customer;
                }
                
            }
            return null;
        }

        public async Task<CustomerModel?> RenewalLicense(string[] ar)
        {
            await Task.Delay(10);

            if (DatabaseMoq.Customers != null && Guid.TryParse(ar[0], out Guid id))
            {
                customers = DatabaseMoq.Customers;
                var licence = customers.FirstOrDefault(c => c.id.Equals(id));

                if (licence != null && licence.statusLicence != null)
                {
                    var history = new CustomerLicenseOperationModel
                    {
                        id = Guid.NewGuid(),
                        idCustomer = id,
                        dateChange = DateTime.Now,
                        oldStatus = licence.statusLicence
                    };

                    licence.statusLicence = new CustomerLicenceStatusModel
                    {
                        licenceType = LicenceType.Main,
                        licenceName = LicenceType.Main.GetDisplayName()
                    };

                    history.newStatus = licence.statusLicence;

                    if (DateTime.TryParse(ar[1], out DateTime desiredDate))
                    {
                        history.oldEndDateLicence = licence.endTimeLicense;
                        licence.endTimeLicense = desiredDate;
                        history.newEndDateLicence = licence.endTimeLicense;
                    }

                    history.price = double.TryParse(ar[2].Trim(), out double parsedPrice) ? parsedPrice : 0.0;
                    history.period = ar[3].Trim();
                    history.nameOperation = "Продовження";
                    if (licence.historyOperations != null)
                    {
                        licence.historyOperations.Add(history);
                        DatabaseMoq.UpdateJson();
                        return licence;
                    }
                    
                }
            }
            return null;
        }

        public async Task<CustomerModel?> ArchivingLicence(string[] ar)
        {
            await Task.Delay(10);

            if (Guid.TryParse(ar[0], out Guid id))
            {
                var history = new CustomerLicenseOperationModel();
                var status = new CustomerLicenceStatusModel();

                if (DatabaseMoq.Customers != null)
                {
                    history.id = Guid.NewGuid();
                    history.idCustomer = id;
                    var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

                    if (licence != null && licence.statusLicence != null && licence.statusLicence != null)
                    {
                        history.dateChange = DateTime.Now;
                        history.oldStatus = licence.statusLicence;
                        licence.statusLicence = new CustomerLicenceStatusModel()
                        {
                            licenceType = LicenceType.Archive,
                            licenceName = LicenceType.Archive.GetDisplayName()
                        };
                        history.newStatus = licence.statusLicence;
                        history.oldEndDateLicence = licence.endTimeLicense;
                        history.newEndDateLicence = licence.endTimeLicense;
                        history.price = 0;
                        history.period = "-";
                        history.nameOperation = "Архівування";
                        if (licence.historyOperations != null)
                        {
                            licence.historyOperations.Add(history);
                            DatabaseMoq.UpdateJson();
                            return licence;
                        }
                    }
                }  
            }
            return null;
        }

        //Зберегти замовника за використання за замовчуванням 
        public CustomerModel? SavePermanentCustomer(string idCu)
        {
            Guid idCustomer = new Guid();
            if (Guid.TryParse(idCu, out Guid result))
            {
                idCustomer = result;
            }

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (customers != null)
                {
                    customer = customers.First(c => c.id.Equals(idCustomer));
                    if (customer != null && customer.configure != null)
                    {
                        if (idCustomer == Guid.Empty)
                        {
                            customer.configure.IdSavedCustomer = Guid.Empty;
                            customer.configure.IsSaveCustomer = false;
                        }
                        else
                        {
                            customer.configure.IdSavedCustomer = idCustomer;
                            customer.configure.IsSaveCustomer = true;
                        }
                        DatabaseMoq.UpdateJson();
                    }
                }
            }
            return customer;
        }

        //Перевірка чи є збережений замовник для входу за замовчуванням 
        public string? CheckSave(string idCu)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(idCu, out Guid id))
                {
                    CustomerConfigModel? conf = customers.First(co => co.id.Equals(id)).configure;
                    if (conf != null)
                    {
                        return conf.IdSavedCustomer.ToString();
                    }
                }
            }
            return null;
        }

        public async Task<CustomerModel?> CancellationLicence(string[] ar)
        {
            await Task.Delay(10);
            var history = new CustomerLicenseOperationModel();
            var status = new CustomerLicenceStatusModel();

            if (Guid.TryParse(ar[0], out Guid id) && DatabaseMoq.Customers != null)
            {
                history.id = Guid.NewGuid();
                history.idCustomer = id;
                CustomerModel? licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

                if (licence != null && licence.statusLicence != null)
                {
                    history.dateChange = DateTime.Now;
                    history.oldStatus = licence.statusLicence;
                    licence.statusLicence = new CustomerLicenceStatusModel()
                    {
                        licenceType = LicenceType.Nulled,
                        licenceName = LicenceType.Nulled.GetDisplayName()

                    };
                    history.newStatus = licence.statusLicence;

                    if (ar.Length > 1 && DateTime.TryParse(ar[1], out DateTime desiredDate))
                    {
                        history.oldEndDateLicence = licence.endTimeLicense;
                        licence.endTimeLicense = desiredDate;
                        history.newEndDateLicence = licence.endTimeLicense;
                    }
                    history.price = 0;
                    history.period = "-";
                    history.nameOperation = "Анулювання";
                    if (licence.historyOperations != null)
                    {
                        licence.historyOperations.Add(history);
                        DatabaseMoq.UpdateJson();
                        return licence;
                    }
                }
            }
            return null;
        }
    }


}
