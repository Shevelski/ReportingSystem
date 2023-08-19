using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
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

        public CustomerModel CreateCustomer(string email)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            CustomerModel model = new CustomerModel();
            model.id = Guid.NewGuid();
            model.email = email;
            model.password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            model.statusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Test,
                licenceName = LicenceType.Test.GetDisplayName()
            };
            model.companies = new List<CompanyModel>();
            model.endTimeLicense = DateTime.Today.AddDays(30);

            CustomerLicenseOperationModel history = new CustomerLicenseOperationModel()
            {
                oldEndDateLicence = DateTime.Today,
                newEndDateLicence = model.endTimeLicense,
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = model.statusLicence,
            };

            model.historyOperations.Add(history);

            // Додавання клієнта до джерела даних
            DatabaseMoq.Customers.Add(model);
            DatabaseMoq.UpdateJson();

            return model;
        }

        public async Task<CustomerModel?> RenewalLicense(string[] ar)
        {
            await Task.Delay(10);

            if (Guid.TryParse(ar[0], out Guid id))
            {
                var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

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

                    licence.historyOperations.Add(history);
                    DatabaseMoq.UpdateJson();

                    return licence;
                }
            }
            return null;
        }

        public async Task<CustomerModel> ArchivingLicence(string[] ar)
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
                        licence.historyOperations.Add(history);
                        DatabaseMoq.UpdateJson();

                        return licence;
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
                    licence.historyOperations.Add(history);
                    DatabaseMoq.UpdateJson();

                    return licence;
                }
            }
            return null;
        }
    }


}
