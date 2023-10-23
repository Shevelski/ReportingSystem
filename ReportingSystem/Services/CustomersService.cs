using Microsoft.AspNetCore.Mvc;
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

        public List<CustomerModel>? GetCustomers()
        {
            var customers = DatabaseMoq.Customers;
            //if (customers != null)
            //{
            //    foreach (var customer in customers)
            //    {
            //        if (customer.password != null)
            //        {
            //            customer.password = EncryptionHelper.Decrypt(customer.password);
            //        }
            //    }
            //}

            return customers;
        }
        
        public CustomerModel? GetCustomer(string idCu)
        {
            if (DatabaseMoq.Customers == null)
            {
                return null;
            }
            Guid id;
            if (!Guid.TryParse(idCu, out id) || id.Equals(Guid.Empty))
            {
                id = DatabaseMoq.Customers[0].id;
            }

            var customer = DatabaseMoq.Customers.First(cu=>cu.id.Equals(id));
            //if (customer.password != null)
            //{
            //    customer.password = EncryptionHelper.Decrypt(customer.password);
            //}
            return customer;
        }

        //реєстрація замовника
        public CustomerModel? RegistrationCustomer(string[] ar)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            Random random = new Random();
            var customer = new CustomerModel
            {
                id = Guid.NewGuid(),
                email = ar[0],
                firstName = ar[1],
                secondName = ar[2],
                thirdName = ar[3],
                phone = ar[4],
                dateRegistration = DateTime.Today,
                //дилема з паролем, ввід при реєстрації чи відправка на пошту
                password = ar[5],//EncryptionHelper.Encrypt(ar[5]),/*new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray()),*/
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
                id = Guid.NewGuid(),
                idCustomer = customer.id,
                oldEndDateLicence = DateTime.Today,
                newEndDateLicence = customer.endTimeLicense,
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = customer.statusLicence
            };

            customer.historyOperations.Add(history);
            var a = DatabaseMoq.Customers;
            var customers = DatabaseMoq.Customers;
            customers.Add(customer);
            DatabaseMoq.UpdateJson();
            return customer;

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

            licence.statusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Archive,
                licenceName = LicenceType.Archive.GetDisplayName()
            };

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


        public async Task<CustomerModel?> DeleteLicence(string[] ar)
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

        public async Task<CustomerModel?> EditCustomer(string[] ar)
        {
            await Task.Delay(10);

            if (ar.Length < 1 || !Guid.TryParse(ar[0], out Guid id) || DatabaseMoq.Customers == null)
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

            if (customer == null || customer.statusLicence == null)
            {
                return null;
            }

            if (ar[1] != customer.firstName)
            {
                customer.firstName = ar[1];
            }

            if (ar[2] != customer.secondName)
            {
                customer.secondName = ar[2];
            }

            if (ar[3] != customer.thirdName)
            {
                customer.thirdName = ar[3];
            }

            if (ar[4] != customer.phone)
            {
                customer.phone = ar[4];
            }

            if (ar[5] != customer.email)
            {
                customer.email = ar[5];
            }

            if (ar[6] != customer.password)
            {
                customer.password = ar[6];
            }


            return customer;
        }


    }
}
