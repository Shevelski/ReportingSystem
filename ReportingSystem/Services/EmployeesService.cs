using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;

namespace ReportingSystem.Services
{
    public class EmployeesService
    {

        CompaniesService companiesService = new CompaniesService();
        List<CustomerModel>? customers = DatabaseMoq.Customers;
        CustomerModel? customer = new CustomerModel();
        CompanyModel? company = new CompanyModel();
        List<CompanyModel>? companies = new List<CompanyModel>();
        EmployeeModel? employee = new EmployeeModel();

        public List<EmployeeModel>? GetEmployees(string companyId)
        {
            string id = companiesService.GetCustomerId();

            if (customers != null && Guid.TryParse(id, out Guid idCustomer))
            {
                customer = customers.First(cu => cu.id.Equals(idCustomer));
                companies = customer.companies;
                if (companies != null)
                {
                    if (Guid.TryParse(companyId, out Guid companyIdGuid))
                    {
                        company = companies.FirstOrDefault(company => company.id == companyIdGuid);
                        if (company != null)
                        {
                            return company.employees;
                        }
                    }
                    else
                    {
                        if (companies != null)
                        {
                            company = companies[0];
                            return company.employees;
                        }
                    }
                }
            }
            return null;
        }

        public EmployeeModel? EditEmployee(Object employeeInput)
        {
            if (employeeInput != null)
            {
                EmployeeModel? employee = JsonConvert.DeserializeObject<EmployeeModel>(employeeInput.ToString());

                string id = companiesService.GetCustomerId();

                var customers = DatabaseMoq.Customers;
                var customer = customers.First(c => c.id.Equals(id));

                if ( customer != null)
                {
                    
                }


                return employee;
            }
            return null;
        }

      
    }


}
