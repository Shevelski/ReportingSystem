using ReportingSystem.Models.Company;
using ReportingSystem.Utils;
using ReportingSystem.Models;
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
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetRolls(idCu, idCo) :
                      await new SQLRead().GetRolls(idCu, idCo);
            return result;
        }
        public async Task<List<EmployeeRolModel>?> GetDevRolls()
        {
            bool mode = Settings.Source().Equals("json");
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
        public async Task EditCompany(string[] ar)
        {
            await new JsonWrite().EditCompany(ar);
            await new SQLWrite().EditCompany(ar);
        }

        //архівування компанії
        public async Task ArchiveCompany(string[] ar)
        {
            await new JsonWrite().ArchiveCompany(ar);
            await new SQLWrite().ArchiveCompany(ar);
        }

        //видалення компанії
        public async Task DeleteCompany(string[] ar)
        {
            await new JsonWrite().DeleteCompany(ar);
            await new SQLWrite().DeleteCompany(ar);
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
                //DatabaseMoq.UpdateJson();
                return companyDetails;
            }
            return null;
        }

        //створення компанії

        public async Task CreateCompany(string[] ar)
        {
            await new JsonWrite().CreateCompany(ar);
            await new SQLWrite().CreateCompany(ar);
        }
    }
}
