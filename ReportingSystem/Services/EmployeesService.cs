using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Enum;
using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using System.Linq;

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
            EmployeeModel editedEmployee = JsonConvert.DeserializeObject<EmployeeModel>(employeeInput.ToString());

            if (editedEmployee != null)
            {
                customers = DatabaseMoq.Customers;
                if (customers != null)
                {
                    var customer = customers.First(c => c.id.Equals(editedEmployee.customerId));
                    if (customer != null)
                    {
                        companies = customer.companies;
                        if (companies != null)
                        {
                            company = companies.First(c => c.id.Equals(editedEmployee.companyId));
                            if (company != null && company.employees != null)
                            {
                                employee = company.employees.First(e => e.id.Equals(editedEmployee.id));
                                //employee = editedEmployee;
                                foreach (var propertyInfo in typeof(EmployeeModel).GetProperties())
                                {
                                    var editedValue = propertyInfo.GetValue(editedEmployee);
                                    if (editedValue != null)
                                    {
                                        var employeeProperty = typeof(EmployeeModel).GetProperty(propertyInfo.Name);
                                        employeeProperty.SetValue(employee, editedValue);
                                    }
                                }
                                DatabaseMoq.UpdateJson();
                                return (employee);
                            }
                            
                        }
                    }

                    return employee;
                }
            }
                
                
            return null;
        }

      
    }


}
