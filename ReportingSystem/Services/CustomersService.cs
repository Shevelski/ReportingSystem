using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;

namespace ReportingSystem.Services
{
    public class CustomersService
    {

        public List<CustomerModel>? GetCustomers()
        {
            var customers = DatabaseMoq.Customers;
            return customers;
        }

        //створення замовника
        public CustomerModel? CreateCustomer(string email)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            Random random = new Random();
            var customer = new CustomerModel
            {
                id = Guid.NewGuid(),
                email = email,
                password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray()),
                statusLicence = new CustomerLicenceStatusModel
                {
                    licenceType = LicenceType.Test,
                    licenceName = LicenceType.Test.GetDisplayName()
                },
                companies = new List<CompanyModel>(),
                endTimeLicense = DateTime.Today.AddDays(30),
                historyOperations = new List<CustomerLicenseOperationModel>()
            };

            var history = new CustomerLicenseOperationModel
            {
                oldEndDateLicence = DateTime.Today,
                newEndDateLicence = customer.endTimeLicense,
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = customer.statusLicence
            };

            customer.historyOperations.Add(history);

            if (DatabaseMoq.Customers != null)
            {
                var customers = DatabaseMoq.Customers;
                customers.Add(customer);
                DatabaseMoq.UpdateJson();
                return customer;
            }

            return null;
        }

        //продовження ліцензії
        public async Task<CustomerModel?> RenewalLicense(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 4 || !Guid.TryParse(ar[0], out Guid id) || !DateTime.TryParse(ar[1], out DateTime desiredDate))
            {
                return null;
            }

            if (DatabaseMoq.Customers == null)
            {
                return null;
            }

            var customers = DatabaseMoq.Customers;
            var customer = customers.FirstOrDefault(c => c.id.Equals(id));

            if (customer == null || customer.statusLicence == null)
            {
                return null;
            }

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = id,
                dateChange = DateTime.Now,
                oldStatus = customer.statusLicence,
                oldEndDateLicence = customer.endTimeLicense
            };

            customer.statusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Main,
                licenceName = LicenceType.Main.GetDisplayName()
            };

            customer.endTimeLicense = desiredDate;
            history.newStatus = customer.statusLicence;
            history.newEndDateLicence = customer.endTimeLicense;

            if (double.TryParse(ar[2].Trim(), out double parsedPrice))
            {
                history.price = parsedPrice;
            }
            else
            {
                history.price = 0.0;
            }

            history.period = ar[3].Trim();
            history.nameOperation = "Продовження";

            if (customer.historyOperations != null)
            {
                customer.historyOperations.Add(history);
                DatabaseMoq.UpdateJson();
                return customer;
            }

            return null;
        }


        public async Task<CustomerModel?> ArchivingLicence(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

            if (licence == null || licence.statusLicence == null)
            {
                return null;
            }

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = id,
                dateChange = DateTime.Now,
                oldStatus = licence.statusLicence,
                newStatus = new CustomerLicenceStatusModel
                {
                    licenceType = LicenceType.Archive,
                    licenceName = LicenceType.Archive.GetDisplayName()
                },
                oldEndDateLicence = licence.endTimeLicense,
                newEndDateLicence = licence.endTimeLicense,
                price = 0,
                period = "-",
                nameOperation = "Архівування"
            };

            if (licence.historyOperations != null)
            {
                licence.historyOperations.Add(history);
                DatabaseMoq.UpdateJson();
                return licence;
            }

            return null;
        }


        public async Task<CustomerModel?> CancellationLicence(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

            if (licence == null || licence.statusLicence == null)
            {
                return null;
            }

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = id,
                dateChange = DateTime.Now,
                oldStatus = licence.statusLicence,
                newStatus = new CustomerLicenceStatusModel
                {
                    licenceType = LicenceType.Nulled,
                    licenceName = LicenceType.Nulled.GetDisplayName()
                },
                price = 0,
                period = "-",
                nameOperation = "Анулювання"
            };

            if (ar.Length > 1 && DateTime.TryParse(ar[1], out DateTime desiredDate))
            {
                history.oldEndDateLicence = licence.endTimeLicense;
                licence.endTimeLicense = desiredDate;
                history.newEndDateLicence = licence.endTimeLicense;
            }

            if (licence.historyOperations != null)
            {
                licence.historyOperations.Add(history);
                DatabaseMoq.UpdateJson();
                return licence;
            }

            return null;
        }


    }
}
