using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Utils;
using System.Collections.Generic;

namespace ReportingSystem.Services
{
    public class CustomersService
    {

        public async Task<List<CustomerModel>?> GetCustomers()
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCustomers() :
                      await new SQLRead().GetCustomers();
            return result;
        }
        
        public async Task<CustomerModel?> GetCustomer(string idCu)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCustomer(idCu) :
                      await new SQLRead().GetCustomer(new Guid(idCu));
            return result;
        }

        //реєстрація замовника
        public async Task RegistrationCustomer(string[] ar)
        {
            await new JsonWrite().RegistrationCustomer(ar);
            await new SQLWrite().RegistrationCustomer(ar);
        }

        //продовження ліцензії
        public async Task RenewalLicense(string[] ar)
        {
            await new JsonWrite().RenewalLicense(ar);
            await new SQLWrite().RenewalLicense(ar);
        }


        public async Task<CustomerModel?> ArchivingLicence(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.Id.Equals(id));

            if (licence == null || licence.StatusLicence == null)
            {
                return null;
            }

            licence.StatusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Archive,
                licenceName = LicenceType.Archive.GetDisplayName()
            };

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = id,
                dateChange = DateTime.Now,
                oldStatus = licence.StatusLicence,
                newStatus = new CustomerLicenceStatusModel
                {
                    licenceType = LicenceType.Archive,
                    licenceName = LicenceType.Archive.GetDisplayName()
                },
                oldEndDateLicence = licence.EndTimeLicense,
                newEndDateLicence = licence.EndTimeLicense,
                price = 0,
                period = "-",
                nameOperation = "Архівування"
            };

            if (licence.HistoryOperations != null)
            {
                licence.HistoryOperations.Add(history);
                DatabaseMoq.UpdateJson();
                return licence;
            }

            return null;
        }


        public async Task<CustomerModel?> DeleteLicence(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.Id.Equals(id));

            if (licence == null || licence.StatusLicence == null)
            {
                return null;
            }

            DatabaseMoq.Customers.Remove(licence);
            DatabaseMoq.UpdateJson();
            return null;
        }


        public async Task<CustomerModel?> CancellationLicence(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var licence = DatabaseMoq.Customers.FirstOrDefault(c => c.Id.Equals(id));

            if (licence == null || licence.StatusLicence == null)
            {
                return null;
            }

            var history = new CustomerLicenseOperationModel
            {
                id = Guid.NewGuid(),
                idCustomer = id,
                dateChange = DateTime.Now,
                oldStatus = licence.StatusLicence,
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
                history.oldEndDateLicence = licence.EndTimeLicense;
                licence.EndTimeLicense = desiredDate;
                history.newEndDateLicence = licence.EndTimeLicense;
            }

            if (licence.HistoryOperations != null)
            {
                licence.HistoryOperations.Add(history);
                DatabaseMoq.UpdateJson();
                return licence;
            }

            return null;
        }

        public async Task<CustomerModel?> EditCustomer(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.Id.Equals(id));

            if (customer == null || customer.StatusLicence == null)
            {
                return null;
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


            return customer;
        }


    }
}
