using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using Bogus.DataSets;
using System.Collections.Generic;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {

        //Отримання списку компаній замовника 
        public List<CompanyModel>? GetCompanies(string idCu)
        {
            if (DatabaseMoq.Customers == null || string.IsNullOrEmpty(idCu))
            {
                return null;
            }

            Guid id;
            if (!Guid.TryParse(idCu, out id) || id.Equals(Guid.Empty))
            {
                id = DatabaseMoq.Customers[0].id;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(co => co.id.Equals(id));

            if (customer != null && customer.companies != null)
            {
                return customer.companies;
            }

            return null;
        }

        //Отримання всіх ролей системи в компанії 
        public List<EmployeeRolModel>? GetRolls(string idCu, string idCo)
        {
            if (DatabaseMoq.Customers == null || string.IsNullOrEmpty(idCu) || string.IsNullOrEmpty(idCo) || !Guid.TryParse(idCu, out Guid idCustomer) || !Guid.TryParse(idCo, out Guid idCompany))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer == null || customer.companies == null)
            {
                return null;
            }

            var company = customer.companies.FirstOrDefault(co => co.id.Equals(idCompany));

            if (company != null && company.rolls != null)
            {
                return company.rolls;
            }

            return null;
        }

        //Отримання всіх ролей системи в компанії 
        public List<EmployeeRolModel>? GetDevRolls()
        {
            List<EmployeeRolModel> devRols = new List<EmployeeRolModel>();
            EmployeeRolModel devRol = new EmployeeRolModel()
            {
                rolType = EmployeeRolStatus.Developer,
                rolName = EmployeeRolStatus.Developer.GetDisplayName()
            };
            devRols.Add(devRol);
            devRol = new EmployeeRolModel()
            {
                rolType = EmployeeRolStatus.DevAdministrator,
                rolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
            };
            devRols.Add(devRol);
            return devRols;
        }


        //отримання списку компаній з статусом актуальні
        public List<CompanyModel>? GetActualCompanies(string idCu)
        {
            if (DatabaseMoq.Customers == null || !Guid.TryParse(idCu, out Guid id))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(co => co.id.Equals(id));

            if (customer != null && customer.companies != null)
            {
                return customer.companies
                    .Where(item => item.status != null && item.status.companyStatusType.Equals(CompanyStatus.Actual))
                    .ToList();
            }

            return null;
        }


        //редагування компанії
        public CompanyModel? EditCompany(string[] ar)
        {
            if (DatabaseMoq.Customers == null || ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    company.name = ar[1];
                    company.address = ar[2];
                    company.actions = ar[3];
                    company.phone = ar[4];
                    company.email = ar[5];
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }

        //архівування компанії
        public CompanyModel? ArchiveCompany(string[] ar)
        {
            if (DatabaseMoq.Customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    company.status = new CompanyStatusModel
                    {
                        companyStatusType = CompanyStatus.Archive,
                        companyStatusName = CompanyStatus.Archive.GetDisplayName()
                    };
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }


        //видалення компанії
        public CompanyModel? DeleteCompany(string[] ar)
        {
            if (DatabaseMoq.Customers == null || ar.Length < 2 || !Guid.TryParse(ar[1], out Guid idCustomer))
            {
                return null;
            }

            var customer = DatabaseMoq.Customers.FirstOrDefault(c => c.id.Equals(idCustomer));

            if (customer != null && customer.companies != null && Guid.TryParse(ar[0], out Guid idCompany))
            {
                var company = customer.companies.FirstOrDefault(c => c.id.Equals(idCompany));
                if (company != null)
                {
                    customer.companies.Remove(company);
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }


        private static Dictionary<Guid, CompanyModel> companiesData = new Dictionary<Guid, CompanyModel>();

        //перевірка єдрпу компанії при створенні - повернення даних про компанію
        public void PostCheckCompany(string[] ar)
        {
            if (ar.Length >= 2 && Guid.TryParse(ar[0], out Guid id))
            {
                companiesData[id] = CheckCompanyWeb.ByCode(ar[1]);
            }
        }

        //перевірка єдрпу компанії при створенні
        public CompanyModel? GetCheckCompany(string id)
        {
            if (Guid.TryParse(id, out Guid guid) && companiesData.TryGetValue(guid, out var companyDetails))
            {
                companiesData.Remove(guid);
                DatabaseMoq.UpdateJson();
                return companyDetails;
            }
            return null;
        }


        //створення компанії
        public CompanyModel? CreateCompany(string[] ar)
        {
            if (ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
            {
                return null;
            }

            var company = new CompanyModel
            {
                name = ar[0],
                code = ar[1],
                address = ar[2],
                actions = ar[3],
                phone = ar[4],
                email = ar[5],
                registrationDate = DateTime.Today,
                rolls = DefaultEmployeeRolls.Get(),
                positions = new List<EmployeePositionModel>(),
                employees = new List<EmployeeModel>(),
                status = new CompanyStatusModel
                {
                    companyStatusType = CompanyStatus.Project,
                    companyStatusName = CompanyStatus.Project.GetDisplayName()
                }
            };

            if (DatabaseMoq.Customers != null)
            {
                var customers = DatabaseMoq.Customers;
                var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

                if (customer != null && customer.companies != null)
                {
                    var chief = new EmployeeModel
                    {
                        firstName = customer.firstName,
                        secondName = customer.secondName,
                        thirdName = customer.thirdName,
                        emailWork = customer.email
                    };

                    company.chief = chief;
                    customer.companies.Add(company);
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }

            return null;
        }
    }
}
