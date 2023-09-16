using ReportingSystem.Controllers;
using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Models.Authorize;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;
using System.Security.AccessControl;

namespace ReportingSystem.Services
{
    public class AuthorizeService
    {
        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();
        EmployeeModel employee = new EmployeeModel();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        AuthorizeModel authorize = new AuthorizeModel();
        List<EmployeeRolModel> employeeRolModels = new List<EmployeeRolModel>();

        //перевірка пошти для входу
        public AuthorizeModel? CheckEmail(string email)
        {
            AuthorizeModel? result = new AuthorizeModel();

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                foreach (var customer in customers)
                {
                    if (customer.email != null && customer.email.ToLower().Equals(email.ToLower()))
                    {
                        authorize.Email = customer.email;
                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                        authorize.Role = new EmployeeRolModel()
                        {
                            rolType = EmployeeRolStatus.Customer,
                            rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                        };
                        return authorize;
                    }

                    if (customer.companies != null)
                    {
                        foreach (var company in customer.companies)
                        {
                            if (company.employees != null)
                            {
                                foreach (var employee in company.employees)
                                {
                                    if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
                                    {
                                        authorize.Email = employee.emailWork;
                                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                                        authorize.AuthorizeStatusModel.authorizeStatusType = Enums.AuthorizeStatus.EmailOk;
                                        authorize.AuthorizeStatusModel.authorizeStatusName = Enums.AuthorizeStatus.EmailOk.GetDisplayName();
                                        authorize.Role = new Models.EmployeeRolModel();
                                        authorize.Role = employee.rol;
                                        return authorize;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailFailed;
            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailFailed.GetDisplayName();
            return authorize;
        }

        //перевірка пароля для входу
        public AuthorizeModel? CheckPassword(string email, string password)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                foreach (var customer in customers)
                {
                    if (customer.password != null && customer.password.ToLower().Equals(password.ToLower()))
                    {
                        authorize.Email = customer.email;
                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
                        authorize.Role = new EmployeeRolModel()
                        {
                            rolType = EmployeeRolStatus.Customer,
                            rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                        };
                        authorize.Employee = new EmployeeModel()
                        {
                            customerId = customer.id,
                            firstName = customer.firstName,
                            secondName = customer.secondName,
                            thirdName = customer.thirdName,
                            rol = new EmployeeRolModel()
                            {
                                rolType = EmployeeRolStatus.Customer,
                                rolName = EmployeeRolStatus.Customer.GetDisplayName(),
                            }
                    };
                        return authorize;
                    }

                    if (customer.companies != null)
                    {
                        foreach (var company in customer.companies)
                        {
                            if (company.employees != null)
                            {
                                foreach (var employee in company.employees)
                                {
                                    if (employee.emailWork != null && employee.emailWork.ToLower().Equals(email.ToLower()))
                                    {
                                        authorize.Email = employee.emailWork;
                                        authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
                                        authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.EmailOk;
                                        authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.EmailOk.GetDisplayName();
                                        authorize.Role = new EmployeeRolModel();
                                        authorize.Role = employee.rol;

                                        if (employee.password != null && employee.password.Equals(password))
                                        {
                                            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordOk;
                                            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordOk.GetDisplayName();
                                            authorize.Employee = employee;
                                            return authorize;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            authorize.AuthorizeStatusModel = new AuthorizeStatusModel();
            authorize.AuthorizeStatusModel.authorizeStatusType = AuthorizeStatus.PasswordFailed;
            authorize.AuthorizeStatusModel.authorizeStatusName = AuthorizeStatus.PasswordFailed.GetDisplayName();
            return authorize;
        }


            public string? GetRolController(AuthorizeModel authorizeModel)
            {
            employeeRolModels = DefaultEmployeeRolls.Get();
            foreach (var item in employeeRolModels)
            {
       
                if (authorize.Role != null && authorize.Role.rolType.Equals(item.rolType))
                {
                    switch (item.rolType)
                    {
                        case EmployeeRolStatus.Administrator:
                            return "EUAdministrator";
                        case EmployeeRolStatus.Developer:
                            return "EUDeveloper";
                        case EmployeeRolStatus.DevAdministrator:
                            return "EUDevAdministrator";
                        case EmployeeRolStatus.ProjectManager:
                            return "EUProjectManager";
                        case EmployeeRolStatus.User:
                            return "EUUser";
                        case EmployeeRolStatus.Customer:
                            return "EUCustomer";
                        case EmployeeRolStatus.CEO:
                            return "EUCEO";
                        default:
                            return "EUUser";
                    }
                }
            }
            return null;
        }
    }
}
