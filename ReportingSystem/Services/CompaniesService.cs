using ReportingSystem.Models.Company;
using ReportingSystem.Utils;
using ReportingSystem.Models;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;

namespace ReportingSystem.Services
{
    public class CompaniesService
    {
        bool mode = Settings.Source().Equals("json");

        //Отримання списку компаній замовника
        public async Task<List<CompanyModel>?> GetCompanies(string idCu)
        {
            return mode ? await new JsonRead().GetCompanies(idCu) : await new SQLRead().GetCompanies(idCu);
        }

        //Отримання ролей системи в компанії 
        public async Task<List<EmployeeRolModel>?> GetRolls(string idCu, string idCo)
        {
            return mode ? await new JsonRead().GetRolls(idCu, idCo) : await new SQLRead().GetRolls(idCu, idCo);
        }
        public async Task<List<EmployeeRolModel>?> GetDevRolls()
        {
            return mode ? await new JsonRead().GetDevRolls() : await new SQLRead().GetDevRolls();
        }

        //отримання списку компаній з статусом актуальні
        public async Task<List<CompanyModel>?> GetActualCompanies(string idCu)
        {
            return mode ? await new JsonRead().GetActualCompanies(idCu) : await new SQLRead().GetActualCompanies(idCu);
        }
        
        //редагування компанії
        public async Task EditCompany(string[] ar)
        {
            await (mode ? new JsonWrite().EditCompany(ar) : new SQLWrite().EditCompany(ar));
        }

        //архівування компанії
        public async Task ArchiveCompany(string[] ar)
        {
            await (mode ? new JsonWrite().ArchiveCompany(ar) : new SQLWrite().ArchiveCompany(ar));
        }

        //архівування компанії
        public async Task RenewalCompany(string[] ar)
        {
            //await (mode ? new JsonWrite().ArchiveCompany(ar) : 
                await new SQLWrite().RenewalCompany(ar);
        }

        //видалення компанії
        public async Task DeleteCompany(string[] ar)
        {
            await (mode ? new JsonWrite().DeleteCompany(ar) : new SQLWrite().DeleteCompany(ar));
        }

        private static Dictionary<Guid, CompanyModel> companiesData = [];

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
                return companyDetails;
            }
            return null;
        }

        //створення компанії

        public async Task CreateCompany(string[] ar)
        {
            await (mode ? new JsonWrite().CreateCompany(ar) : new SQLWrite().CreateCompany(ar));
        }
    }
}
