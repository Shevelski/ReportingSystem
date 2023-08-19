using ReportingSystem.Enum;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enum.Extensions;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {

        public List<CompanyModel> GetCompanies(string ar)
        {
            if (Guid.TryParse(ar, out Guid id))
            {

                var companies = DatabaseMoq.Customers.First(co => co.id.Equals(id)).companies;
                return companies;
            }
            return null;
        }


        
        public List<CompanyModel> GetActualCompanies(string ar)
        {
            List<CompanyModel> actual = new List<CompanyModel>();

            if (Guid.TryParse(ar, out Guid id))
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
            if (Guid.TryParse(ar[6], out Guid result))
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
            if (Guid.TryParse(ar[1], out Guid result))
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
    }
}
