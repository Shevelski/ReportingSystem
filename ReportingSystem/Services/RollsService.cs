﻿using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;

namespace ReportingSystem.Services
{
    public class RollsService
    {
        CustomerModel customer = new CustomerModel();
        List<CustomerModel> customers = new List<CustomerModel>();
        CompanyModel company = new CompanyModel();
        List<CompanyModel> companies = new List<CompanyModel>();
        List<EmployeePositionModel> positions = new List<EmployeePositionModel>();
        List<EmployeeModel> employees = new List<EmployeeModel>();
        EmployeeModel employee = new EmployeeModel();

        //Отримання списку посад компанії 
        public List<EmployeePositionModel>? GetAllPositions(string idCu, string idCo)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(idCu, out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(idCo, out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            return company.positions;
                        }
                    }
                }
            }
            return null;
        }

        //Отримання списку унікальних посад компанії 
        public List<EmployeePositionModel>? GetUniqPositions(string idCu, string idCo)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(idCu, out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {

                        if (Guid.TryParse(idCo, out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
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



        //отримати користувачів компанії за ролями
        public List<EmployeeModel>? GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            List<EmployeeModel> employeesByPosition = new List<EmployeeModel>();

            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(idCu, out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(idCo, out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            if (company != null && company.employees != null)
                            {
                                employees = company.employees;
                                foreach (var employee in employees)
                                {
                                    if (employee.position != null && employee.position.namePosition != null && employee.position.namePosition.Equals(pos))
                                    {
                                        employeesByPosition.Add(employee);
                                    }
                                }
                                return employeesByPosition;
                            }

                        }
                    }
                }
            }
            return null;
        }

        //створення нової посади
        public List<EmployeeModel>? CreatePosition(string[] ar)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(ar[0], out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(ar[1], out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            if (company != null && company.positions != null)
                            {
                                EmployeePositionModel newEmployeePosition = new EmployeePositionModel()
                                {
                                    namePosition = ar[2]
                                };
                                company.positions.Add(newEmployeePosition);
                                DatabaseMoq.UpdateJson();
                            }
                        }
                    }
                }
            }
            return null;
        }

        //створення нової посади
        public List<EmployeeModel>? EditPosition(string[] ar)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(ar[0], out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(ar[1], out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            if (company != null && company.positions != null)
                            {
                                EmployeePositionModel position = company.positions.First(pos => pos.namePosition != null && pos.namePosition.Equals(ar[2]));
                                position.namePosition = ar[3];

                                if (company.employees != null)
                                {
                                    foreach (var emp in company.employees)
                                    {
                                        if (emp.position != null && emp.position.namePosition != null && emp.position.namePosition.Equals(ar[2]))
                                        {
                                            emp.position.namePosition = ar[3];
                                        }
                                    }
                                }
                                EmployeePositionModel oldPosition = company.positions.First(pos => pos.namePosition != null && pos.namePosition.Equals(ar[2]));
                                company.positions.Remove(oldPosition);
                                DatabaseMoq.UpdateJson();
                            }
                        }
                    }
                }
            }
            return null;
        }

        //видалення посади
        public List<EmployeeModel>? DeletePosition(string[] ar)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(ar[0], out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(ar[1], out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            if (company != null && company.positions != null)
                            {
                                EmployeePositionModel position = company.positions.First(pos => pos.namePosition != null && pos.namePosition.Equals(ar[2]));
                                company.positions.Remove(position);
                               
                                DatabaseMoq.UpdateJson();
                            }
                        }
                    }
                }
            }
            return null;
        }

        //зміна посади
        public EmployeeModel? EditEmployeePosition(string[] ar)
        {
            if (DatabaseMoq.Customers != null)
            {
                customers = DatabaseMoq.Customers;
                if (Guid.TryParse(ar[0], out Guid idCustomer))
                {
                    var companies = customers.First(co => co.id.Equals(idCustomer)).companies;
                    if (companies != null)
                    {
                        if (Guid.TryParse(ar[1], out Guid idCompany))
                        {
                            var company = companies.First(co => co.id.Equals(idCompany));
                            if (company != null && company.employees != null)
                            {
                                employees = company.employees;
                                if (Guid.TryParse(ar[2], out Guid idEmployee))
                                {
                                    employee = employees.First(em => em.id.Equals(idEmployee));
                                    if (employee != null && employee.position != null)
                                    {
                                        employee.position.namePosition = ar[3];
                                        DatabaseMoq.UpdateJson();
                                    }
                                } 
                            }
                        }
                    }
                }
            }
            return null;
        }

        //Guid idCompany = new Guid();
        //if (Guid.TryParse(idCo, out Guid result0))
        //{
        //    idCompany = result0;
        //}

        //Guid idCustomer = new Guid();
        //if (Guid.TryParse(idCu, out Guid result))
        //{
        //    idCustomer = result;
        //}

        //if (DatabaseMoq.Customers != null)
        //{
        //    customers = DatabaseMoq.Customers;
        //    if (customers != null)
        //    {
        //        customer = customers.First(c => c.id.Equals(idCustomer));
        //        if (customer != null && customer.configure != null)
        //        {
        //            if (idCompany == Guid.Empty)
        //            {
        //                customer.configure.IdSavedCompany = Guid.Empty;
        //                customer.configure.IsSaveCompany = false;
        //            } else
        //            {
        //                customer.configure.IdSavedCompany = idCompany;
        //                customer.configure.IsSaveCompany = true;
        //            }
        //            DatabaseMoq.UpdateJson();
        //        }
        //    }
        //}
        //return customer;

        ////Отримання списку компаній замовника 
        //public List<CompanyModel>? GetCompanies(string idCu)
        //{
        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        if (Guid.TryParse(idCu, out Guid id))
        //        {
        //            var companies = customers.First(co => co.id.Equals(id)).companies;
        //            return companies;
        //        }
        //    }
        //    return null;
        //}

        ////Перевірка чи є збережена компанія для входу за замовчуванням 
        //public string? CheckSave(string idCu)
        //{
        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        if (Guid.TryParse(idCu, out Guid id))
        //        {
        //            CustomerConfigModel? conf = customers.First(co => co.id.Equals(id)).configure;
        //            if (conf != null)
        //            {
        //                return conf.IdSavedCompany.ToString();
        //            }
        //        }
        //    }
        //    return null;
        //}


        ////Отримання всіх посад в компанії 
        //public List<EmployeePositionModel>? GetPositions(string idCu, string idCo)
        //{
        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        if (Guid.TryParse(idCu, out Guid idCustomer))
        //        {
        //            customer = customers.First(c => c.id.Equals(idCustomer));
        //            if (customer != null)
        //            {
        //                if (customer.companies != null && Guid.TryParse(idCo, out Guid idCompany)){

        //                    company = customer.companies.First(co => co.id.Equals(idCompany));
        //                    if (company.positions != null)
        //                    {
        //                        List<EmployeePositionModel> uniqPosition = new List<EmployeePositionModel>();
        //                        List<string> listNamePositions = new List<string>();
        //                        foreach (var position in company.positions)
        //                        {
        //                            if (position.namePosition != null)
        //                            {
        //                                if (!listNamePositions.Contains(position.namePosition))
        //                                {
        //                                    listNamePositions.Add(position.namePosition);
        //                                    uniqPosition.Add(position);
        //                                }
        //                            }

        //                        }
        //                        return uniqPosition;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}

        ////Отримання всіх ролей системи в компанії 
        //public List<EmployeeRolModel>? GetRolls(string idCu, string idCo)
        //{
        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        if (Guid.TryParse(idCu, out Guid idCustomer))
        //        {
        //            customer = customers.First(c => c.id.Equals(idCustomer));
        //            if (customer != null)
        //            {
        //                if (customer.companies != null && Guid.TryParse(idCo, out Guid idCompany))
        //                {

        //                    company = customer.companies.First(co => co.id.Equals(idCompany));
        //                    if (company.rolls != null)
        //                    {
        //                        return company.rolls;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}

        ////отримання списку компаній з статусом актуальні
        //public List<CompanyModel>? GetActualCompanies(string idCu)
        //{
        //    List<CompanyModel> actual = new List<CompanyModel>();

        //    if (DatabaseMoq.Customers != null && Guid.TryParse(idCu, out Guid id))
        //    {
        //        customers = DatabaseMoq.Customers;
        //        var customer = customers.First(co => co.id.Equals(id));

        //        if (customer.companies != null)
        //        {
        //            companies = customer.companies;

        //            foreach (var item in companies)
        //            {
        //                if (item.status != null && item.status.companyStatusType.Equals(CompanyStatus.Actual))
        //                {
        //                    actual.Add(item);
        //                }
        //            }
        //            return actual;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    return null;
        //}

        ////редагування компанії
        //public CompanyModel? EditCompany(string[] ar)
        //{
        //    Guid idCustomer = new Guid();
        //    if (Guid.TryParse(ar[6], out Guid result))
        //    {
        //        idCustomer = result;
        //    }

        //    Guid idCompany = new Guid();
        //    if (Guid.TryParse(ar[0], out Guid result1))
        //    {
        //        idCompany = result1;
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        customer = customers.First(c => c.id.Equals(idCustomer));
        //        if (customer.companies != null)
        //        {
        //            companies = customer.companies;
        //            company = customer.companies.First(c => c.id.Equals(idCompany));
        //            company.name = ar[1];
        //            company.address = ar[2];
        //            company.actions = ar[3];
        //            company.phone = ar[4];
        //            company.email = ar[5];
        //            DatabaseMoq.UpdateJson();
        //            return company;
        //        }

        //    }
        //    return null;
        //}

        ////архівування компанії
        //public CompanyModel? ArchiveCompany(string[] ar)
        //{
        //    Guid idCustomer = new Guid();
        //    if (Guid.TryParse(ar[1], out Guid result))
        //    {
        //        idCustomer = result;
        //    }

        //    Guid idCompany = new Guid();
        //    if (Guid.TryParse(ar[0], out Guid result1))
        //    {
        //        idCompany = result1;
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        customer = customers.First(c => c.id.Equals(idCustomer));
        //        if (customer.companies != null)
        //        {
        //            companies = customer.companies;
        //            company = companies.First(c => c.id.Equals(idCompany));
        //            CompanyStatusModel status = new CompanyStatusModel();
        //            company.status = new CompanyStatusModel()
        //            {
        //                companyStatusType = CompanyStatus.Archive,
        //                companyStatusName = CompanyStatus.Archive.GetDisplayName(),
        //            };
        //            DatabaseMoq.UpdateJson();
        //            return company;
        //        }
        //    }
        //    return null;
        //}


        ////видалення компанії
        //public CompanyModel? DeleteCompany(string[] ar)
        //{
        //    Guid idCustomer = new Guid();
        //    if (Guid.TryParse(ar[1], out Guid result))
        //    {
        //        idCustomer = result;
        //    }

        //    Guid idCompany = new Guid();
        //    if (Guid.TryParse(ar[0], out Guid result1))
        //    {
        //        idCompany = result1;
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        customer = customers.First(c => c.id.Equals(idCustomer));
        //        if (customer.companies != null)
        //        {
        //            companies = customer.companies;
        //            company = companies.First(c => c.id.Equals(idCompany));
        //            customer.companies.Remove(company);
        //            DatabaseMoq.UpdateJson();
        //        }
        //    }
        //    return null;
        //}

        //private static Dictionary<Guid, CompanyModel> companiesData = new Dictionary<Guid, CompanyModel>();

        ////перевірка єдрпу компанії при створенні - повернення даних про компанію
        //public void PostCheckCompany(string[] ar)
        //{
        //    Guid id = new Guid();
        //    if (Guid.TryParse(ar[0], out Guid result))
        //    {
        //        id = result;
        //    }
        //    companiesData.Add(id, CheckCompanyWeb.ByCode(ar[1]));
        //}

        ////перевірка єдрпу компанії при створенні
        //public CompanyModel? GetCheckCompany(string id)
        //{

        //    Guid guid = new Guid();
        //    if (Guid.TryParse(id, out Guid result))
        //    {
        //        guid = result;
        //    }

        //    if (companiesData.TryGetValue(guid, out var companyDetails))
        //    {
        //        companiesData.Remove(guid);
        //        DatabaseMoq.UpdateJson();
        //        return companyDetails;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ////створення компанії
        //public CompanyModel? CreateCompany(string[] ar)
        //{
        //    CompanyModel company = new CompanyModel();
        //    company.name = ar[0];
        //    company.code = ar[1];
        //    company.address = ar[2];
        //    company.actions = ar[3];
        //    company.phone = ar[4];
        //    company.email = ar[5];
        //    company.registrationDate = DateTime.Today;
        //    company.rolls = DefaultEmployeeRolls.Get();
        //    company.positions = new List<EmployeePositionModel>();
        //    company.employees = new List<EmployeeModel>();
        //    company.status = new CompanyStatusModel()
        //    {
        //        companyStatusType = CompanyStatus.Project,
        //        companyStatusName = CompanyStatus.Project.GetDisplayName(),
        //    };

        //    Guid idCustomer = new Guid();
        //    if (Guid.TryParse(ar[6], out Guid result))
        //    {
        //        idCustomer = result;
        //    }

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        customers = DatabaseMoq.Customers;
        //        customer = customers.First(c => c.id.Equals(idCustomer));

        //        EmployeeModel chief = new EmployeeModel()
        //        {
        //            firstName = customer.firstName,
        //            secondName = customer.secondName,
        //            thirdName = customer.thirdName,
        //            emailWork = customer.email,

        //        };
        //        company.chief = chief;
        //        if (customer.companies != null)
        //        {
        //            companies = customer.companies;
        //            companies.Add(company);
        //            DatabaseMoq.UpdateJson();
        //        }
        //    }
        //    return null;
        //}
    }
}
