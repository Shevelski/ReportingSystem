using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using System;
using ReportingSystem.Models.Company;

namespace ReportingSystem.Controllers
{

    public class CustomersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllLicence()
        {
            await Task.Delay(10);
            var x = DatabaseMoq.Customers;
            Console.WriteLine(x);
            return Json(DatabaseMoq.Customers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] string[] ar)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

           

            CustomerModel model = new CustomerModel();
            model.email = ar[0];
            model.password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            model.statusLicence = new CustomerLicenceStatusModel
            {
                licenceType = LicenceType.Test,
                licenceName = LicenceType.Test.GetDisplayName()
            };
            model.companies = new List<CompanyModel>();
            CustomerLicenseOperationModel history = new CustomerLicenseOperationModel()
            {
                oldEndTimeLicence = DateTime.Today,
                newEndTimeLicence = DateTime.Today.AddDays(30),
                oldStatus = new CustomerLicenceStatusModel(),
                newStatus = model.statusLicence,
            };

            model.historyOperations.Add(history);
            DatabaseMoq.Customers.Add(model);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RenewalLicence([FromBody] string[] ar)
        {
            await Task.Delay(10);

            if (Guid.TryParse(ar[0], out Guid id) && DatabaseMoq.Customers != null)
            {
                CustomerModel? licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));
                CustomerLicenseOperationModel history = new CustomerLicenseOperationModel();
                CustomerLicenceStatusModel status = new CustomerLicenceStatusModel();

                history.id = Guid.NewGuid();
                history.idCustomer = id;
                
                if (licence != null && licence.statusLicence != null && licence.statusLicence != null)
                {

                    history.dateChange = DateTime.Now;
                    history.oldStatus = licence.statusLicence;
                    licence.statusLicence = new CustomerLicenceStatusModel()
                    {
                        licenceType = LicenceType.Main,
                        licenceName = LicenceType.Main.GetDisplayName()

                    };
                          
                    history.newStatus = licence.statusLicence;

                    if (ar.Length > 1 && DateTime.TryParse(ar[1], out DateTime desiredDate))
                    {
                        history.oldEndTimeLicence = licence.endTimeLicense;
                        licence.endTimeLicense = desiredDate;
                        history.newEndTimeLicence = licence.endTimeLicense;
                    }
                    history.price = Double.Parse(ar[2].Trim());
                    history.period = ar[3].Trim();
                    history.nameOperation = "Продовження";
                    licence.historyOperations.Add(history);
                    return Json(licence);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ArchivingLicence([FromBody] string[] ar)
        {
            await Task.Delay(10);
            CustomerLicenseOperationModel history = new CustomerLicenseOperationModel();
            CustomerLicenceStatusModel status = new CustomerLicenceStatusModel();

            if (Guid.TryParse(ar[0], out Guid id))
            {
                if (DatabaseMoq.Customers != null)
                {
                    history.id = Guid.NewGuid();
                    history.idCustomer = id;
                    CustomerModel? licence = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(id));

                    if (licence != null && licence.statusLicence != null && licence.statusLicence != null)
                    {
                        history.dateChange = DateTime.Now;
                        history.oldStatus = licence.statusLicence;
                        licence.statusLicence = new CustomerLicenceStatusModel()
                        {
                            licenceType = LicenceType.Archive,
                            licenceName = LicenceType.Main.GetDisplayName()
                        };
                        history.newStatus = licence.statusLicence;
                        history.oldEndTimeLicence = licence.endTimeLicense;
                        history.newEndTimeLicence = licence.endTimeLicense;
                        history.price = 0;
                        history.period = "-";
                        history.nameOperation = "Архівування";
                        licence.historyOperations.Add(history);

                        return Json(licence);
                    }

                    return NotFound();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CancellationLicence([FromBody] string[] ar)
        {
            await Task.Delay(10);
            CustomerLicenseOperationModel history = new CustomerLicenseOperationModel();
            CustomerLicenceStatusModel status = new CustomerLicenceStatusModel();

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
                        history.oldEndTimeLicence = licence.endTimeLicense;
                        licence.endTimeLicense = desiredDate;
                        history.newEndTimeLicence = licence.endTimeLicense;
                    }
                    history.price = 0;
                    history.period = "-";
                    history.nameOperation = "Анулювання";
                    licence.historyOperations.Add(history);

                    return Json(licence);
                }
            }

            return NotFound();
        }


    }
}
