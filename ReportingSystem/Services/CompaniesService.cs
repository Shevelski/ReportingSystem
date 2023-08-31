using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {

        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();

        // заглушка - отримати ід замовника, повинно братися з форми авторизації, зараз з конфігураційного файла
        public string GetCustomerId()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return MyConfig.GetValue<string>("TempCustomer:id");
        }

        //Зберегти компанію за використання за замовчуванням 
        public CustomerModel? SavePermanentCompany(string id)
        {
            Guid idCompany = new Guid();
            if (Guid.TryParse(id, out Guid result0))
            {
                idCompany = result0;
            }

            Guid idCustomer = new Guid();
            if (Guid.TryParse(GetCustomerId(), out Guid result))
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
                        if (idCompany == Guid.Empty)
                        {
                            customer.configure.IdSavedCompany = Guid.Empty;
                            customer.configure.IsSaveCompany = false;
                        } else
                        {
                            customer.configure.IdSavedCompany = idCompany;
                            customer.configure.IsSaveCompany = true;
                        }
                        DatabaseMoq.UpdateJson();
                    }
                    
                }
            }
            return customer;
            
        }

        //Отримання списку компаній замовника 
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

        //Перевірка чи є збережена компанія для входу за замовчуванням 
        public string? CheckSave()
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(GetCustomerId(), out Guid id))
                {
                    CustomerConfigModel conf = customers.First(co => co.id.Equals(id)).configure;
                    if (conf != null)
                    {
                        return conf.IdSavedCompany.ToString();
                    }
                }
            }
            return null;
        }


        //Отримання всіх посад в компанії 
        public List<EmployeePositionModel>? GetPositions(string id)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(GetCustomerId(), out Guid idCustomer))
                {
                    customer = customers.First(c => c.id.Equals(idCustomer));
                    if (customer != null)
                    {
                        if (customer.companies != null && Guid.TryParse(id, out Guid idCompany)){

                            company = customer.companies.First(co => co.id.Equals(idCompany));
                            if (company.positions != null)
                            {
                                List<EmployeePositionModel> uniqPosition = new List<EmployeePositionModel>();
                                List<string> listNamePositions = new List<string>();
                                foreach (var position in company.positions)
                                {
                                    if (position.namePosition != null)
                                    {
                                        if (!listNamePositions.Contains(position.namePosition))
                                        {
                                            listNamePositions.Add(position.namePosition);
                                            uniqPosition.Add(position);
                                        }
                                    }
                                   
                                }
                                return uniqPosition;
                            }
                        }
                    }
                }
            }
            return null;
        }

        //Отримання всіх ролей системи в компанії 
        public List<EmployeeRolModel>? GetRolls(string id)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(GetCustomerId(), out Guid idCustomer))
                {
                    customer = customers.First(c => c.id.Equals(idCustomer));
                    if (customer != null)
                    {
                        if (customer.companies != null && Guid.TryParse(id, out Guid idCompany))
                        {

                            company = customer.companies.First(co => co.id.Equals(idCompany));
                            if (company.rolls != null)
                            {
                                return company.rolls;
                            }
                        }
                    }
                }
            }
            return null;
        }

        //отримання списку компаній з статусом актуальні
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
                    return actual;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        //редагування компанії
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

        //архівування компанії
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


        //видалення компанії
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
        
        //перевірка єдрпу компанії при створенні - повернення даних про компанію
        public void PostCheckCompany(string[] ar)
        {
            Guid id = new Guid();
            if (Guid.TryParse(ar[0], out Guid result))
            {
                id = result;
            }
            companiesData.Add(id, CheckCompanyWeb.ByCode(ar[1]));
        }

        //перевірка єдрпу компанії при створенні
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

        //створення компанії
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
