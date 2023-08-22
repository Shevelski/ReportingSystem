﻿using ReportingSystem.Enum;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {

        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();

        public string GetCustomerId()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string ar = MyConfig.GetValue<string>("TempCustomer:id");

            return ar;
        }

        public List<CompanyModel>? GetCompanies()
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(GetCustomerId(), out Guid id))
                {
                    var companies = customers.First(co => co.id.Equals(id)).companies;
                    return companies;
                }
            }
            return null;
        }



        public List<CompanyModel>? GetActualCompanies()
        {
            List<CompanyModel> actual = new List<CompanyModel>();

            if (DatabaseMoq.Customers != null && Guid.TryParse(GetCustomerId(), out Guid id))
            {
                customers = DatabaseMoq.Customers;
                var customer = customers.First(co => co.id.Equals(id));

                if (customer.companies != null)
                {
                    companies = customer.companies;
                  
                    foreach (var item in companies)
                    {
                        if (item.status.companyStatusType.Equals(CompanyStatus.Actual))
                        {
                            actual.Add(item);
                        }
                    }
                    Console.WriteLine(actual);
                    return actual;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public CompanyModel? EditCompany(string[] ar)
        {
            Guid idCustomer = new Guid();
            if (Guid.TryParse(GetCustomerId(), out Guid result))
            {
                idCustomer = result;
            }

            Guid idCompany = new Guid();
            if (Guid.TryParse(ar[0], out Guid result1))
            {
                idCompany = result1;
            }

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                customer = customers.First(c => c.id.Equals(idCustomer));
                if (customer.companies != null)
                {
                    companies = customer.companies;
                    company = customer.companies.First(c => c.id.Equals(idCompany));
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

        public CompanyModel? ArchiveCompany(string[] ar)
        {
            Guid idCustomer = new Guid();
            if (Guid.TryParse(GetCustomerId(), out Guid result))
            {
                idCustomer = result;
            }

            Guid idCompany = new Guid();
            if (Guid.TryParse(ar[0], out Guid result1))
            {
                idCompany = result1;
            }

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                customer = customers.First(c => c.id.Equals(idCustomer));
                if (customer.companies != null)
                {
                    companies = customer.companies;
                    company = companies.First(c => c.id.Equals(idCompany));
                    CompanyStatusModel status = new CompanyStatusModel();
                    company.status = new CompanyStatusModel()
                    {
                        companyStatusType = CompanyStatus.Archive,
                        companyStatusName = CompanyStatus.Archive.GetDisplayName(),
                    };
                    DatabaseMoq.UpdateJson();
                    return company;
                }
            }
            return null;
        }


        public CompanyModel? DeleteCompany(string[] ar)
        {
            Guid idCustomer = new Guid();
            if (Guid.TryParse(GetCustomerId(), out Guid result))
            {
                idCustomer = result;
            }

            Guid idCompany = new Guid();
            if (Guid.TryParse(ar[0], out Guid result1))
            {
                idCompany = result1;
            }

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                customer = customers.First(c => c.id.Equals(idCustomer));
                if (customer.companies != null)
                {
                    companies = customer.companies;
                    company = companies.First(c => c.id.Equals(idCompany));
                    customer.companies.Remove(company);
                    DatabaseMoq.UpdateJson();
                }
            }
            return null;
        }

        private static Dictionary<Guid, CompanyModel> companiesData = new Dictionary<Guid, CompanyModel>();

        public CompanyModel? PostCheckCompany(string[] ar)
        {
            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }
            companiesData.Add(id, CheckCompanyWeb.ByCode(ar[1]));
            DatabaseMoq.UpdateJson();
            return null;
        }

        public CompanyModel? GetCheckCompany(string id)
        {

            Guid guid = new Guid();
            if (Guid.TryParse(id, out Guid result))
            {
                guid = result;
            }

            if (companiesData.TryGetValue(guid, out var companyDetails))
            {
                companiesData.Remove(guid);
                DatabaseMoq.UpdateJson();
                return companyDetails;
            }
            else
            {
                return null;
            }
        }

        public CompanyModel? CreateCompany(string[] ar)
        {
            CompanyModel company = new CompanyModel();
            company.name = ar[0];
            company.code = ar[1];
            company.address = ar[2];
            company.actions = ar[3];
            company.phone = ar[4];
            company.email = ar[5];
            company.registrationDate = DateTime.Today;
            company.rolls = DefaultEmployeeRolls.Get();
            company.positions = new List<EmployeePositionModel>();
            company.employees = new List<EmployeeModel>();
            company.status = new CompanyStatusModel()
            {
                companyStatusType = CompanyStatus.Project,
                companyStatusName = CompanyStatus.Project.GetDisplayName(),
            };

            Guid idCustomer = new Guid();
            if (Guid.TryParse(GetCustomerId(), out Guid result))
            {
                idCustomer = result;
            }

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                customer = customers.First(c => c.id.Equals(idCustomer));

                EmployeeModel chief = new EmployeeModel()
                {
                    firstName = customer.firstName,
                    secondName = customer.secondName,
                    thirdName = customer.thirdName,
                    emailWork = customer.email,

                };
                company.chief = chief;

                if (customer.companies != null)
                {
                    companies = customer.companies;
                    companies.Add(company);
                    DatabaseMoq.UpdateJson();
                }

            }
            
            return null;
        }
    }
}
