﻿using ReportingSystem.Enums;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using Newtonsoft.Json;
using ReportingSystem.Data;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {
        //Отримання списку компаній замовника
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCompanies(idCu) :
                      await new SQLRead().GetCompanies(idCu);
            return result;
        }

        //Отримання ролей системи в компанії 
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            bool mode = Utils.Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetRolls(idCu, idCo) :
                      await new SQLRead().GetRolls(idCu, idCo);
            return result;
        }
        public async Task<List<EmployeeRolModel>?> GetDevRolls()
        {
            bool mode = Utils.Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetDevRolls() :
                      await new SQLRead().GetDevRolls();
            return result;
        }

        //отримання списку компаній з статусом актуальні
        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetActualCompanies(idCu) :
                      await new SQLRead().GetActualCompanies(idCu);
            return result;
        }
        
        //редагування компанії
        public async Task<CompanyModel?> EditCompany(string[] ar)
        {
            bool mode = Settings.Source().Equals("json");
            var result1 = await new JsonWrite().EditCompany(ar);
            var result2 = await new SQLWrite().EditCompany(ar);
            return mode ? result1 : result2;
        }


        //архівування компанії
        public async Task<CompanyModel?> ArchiveCompany(string[] ar)
        {
            bool mode = Settings.Source().Equals("json");
            var result1 = await new JsonWrite().ArchiveCompany(ar);
            var result2 = await new SQLWrite().ArchiveCompany(ar);
            return mode ? result1 : result2;
        }

        //видалення компанії
        public async Task<CompanyModel?> DeleteCompany(string[] ar)
        {
            bool mode = Settings.Source().Equals("json");
            var result1 = await new JsonWrite().DeleteCompany(ar);
            var result2 = await new SQLWrite().DeleteCompany(ar);
            return mode ? result1 : result2;
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

        public async Task<CompanyModel?> CreateCompany(string[] ar)
        {
            bool mode = Settings.Source().Equals("json");
            var result1 = await new JsonWrite().CreateCompany(ar);
            var result2 = await new SQLWrite().CreateCompany(ar);
            return mode ? result1 : result2;
        }

        //public CompanyModel? CreateCompany(string[] ar)
        //{
        //    if (ar.Length < 7 || !Guid.TryParse(ar[6], out Guid idCustomer))
        //    {
        //        return null;
        //    }

        //    var company = new CompanyModel
        //    {
        //        name = ar[0],
        //        code = ar[1],
        //        address = ar[2],
        //        actions = ar[3],
        //        phone = ar[4],
        //        email = ar[5],
        //        registrationDate = DateTime.Today,
        //        rolls = DefaultEmployeeRolls.Get(),
        //        positions = new List<EmployeePositionModel>(),
        //        employees = new List<EmployeeModel>(),
        //        status = new CompanyStatusModel
        //        {
        //            companyStatusType = CompanyStatus.Project,
        //            companyStatusName = CompanyStatus.Project.GetDisplayName()
        //        }
        //    };

        //    if (DatabaseMoq.Customers != null)
        //    {
        //        var customers = DatabaseMoq.Customers;
        //        var customer = customers.FirstOrDefault(c => c.id.Equals(idCustomer));

        //        if (customer != null && customer.companies != null)
        //        {
        //            var chief = new EmployeeModel
        //            {
        //                firstName = customer.firstName,
        //                secondName = customer.secondName,
        //                thirdName = customer.thirdName,
        //                emailWork = customer.email
        //            };

        //            company.chief = chief;
        //            customer.companies.Add(company);
        //            DatabaseMoq.UpdateJson();
        //            return company;
        //        }
        //    }

        //    return null;
        //}

    }
}
