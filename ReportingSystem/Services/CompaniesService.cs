using ReportingSystem.Enum;
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

        public string GetCustomerId()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string ar = MyConfig.GetValue<string>("TempCustomer:id");

            return ar;
        }

        public List<CompanyModel>? GetCompanies()
        {

            if (Guid.TryParse(GetCustomerId(), out Guid id))
            {

                var companies = DatabaseMoq.Customers.First(co => co.id.Equals(id)).companies;
                return companies;
            }
            return null;
        }



        public List<CompanyModel>? GetActualCompanies()
        {
            List<CompanyModel> actual = new List<CompanyModel>();

            if (Guid.TryParse(GetCustomerId(), out Guid id))
            {

                var customer = DatabaseMoq.Customers.First(co => co.id.Equals(id));

                if (customer != null)
                {
                    //where просто відмовився працювати
                    foreach (var item in customer.companies)
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

        public CompanyModel EditCompany(string[] ar)
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

            CompanyModel company = DatabaseMoq.Customers.First(c => c.id.Equals(idCustomer)).companies.First(c => c.id.Equals(idCompany));
            company.name = ar[1];
            company.address = ar[2];
            company.actions = ar[3];
            company.phone = ar[4];
            company.email = ar[5];
            DatabaseMoq.UpdateJson();
            return company;
        }

        public CompanyModel ArchiveCompany(string[] ar)
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


            CompanyModel company = DatabaseMoq.Customers.First(c => c.id.Equals(idCustomer)).companies.First(c => c.id.Equals(idCompany));
            CompanyStatusModel status = new CompanyStatusModel();
            company.status = new CompanyStatusModel()
            {
                companyStatusType = CompanyStatus.Archive,
                companyStatusName = CompanyStatus.Archive.GetDisplayName(),
            };
            DatabaseMoq.UpdateJson();
            return company;
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

            CustomerModel customer = DatabaseMoq.Customers.First(c => c.id.Equals(idCustomer));
            CompanyModel company = DatabaseMoq.Customers.First(c => c.id.Equals(idCustomer)).companies.First(c => c.id.Equals(idCompany));

            customer.companies.Remove(company);
            DatabaseMoq.UpdateJson();
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

            var customer = DatabaseMoq.Customers.First(c => c.id.Equals(idCustomer));

            EmployeeModel chief = new EmployeeModel()
            {
                firstName = customer.firstName,
                secondName = customer.secondName,
                thirdName = customer.thirdName,
                emailWork = customer.email,

            };
            company.chief = chief;

            customer.companies.Add(company);
            DatabaseMoq.UpdateJson();
            return null;
        }
    }
}
