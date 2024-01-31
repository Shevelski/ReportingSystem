using Microsoft.AspNetCore.Mvc;
using ReportingSystem;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Project;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class ProjectsCategoriesService
    {
        public async Task<List<ProjectCategoryModel>?> GetCategories(string idCu, string idCo)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetCategories(idCu, idCo) :
                      await new SQLRead().GetCategories(idCu, idCo);
            return result;
        }

        public async Task CreateCategory(string[] ar)
        {
            //await new JsonWrite().CreateCategory(ar); //звернути увагу під час тестування json
            await new SQLWrite().CreateCategory(ar);
        }
        public async Task EditNameCategory(string[] ar)
        {
            //await new JsonWrite().EditNameCategory(ar);
            await new SQLWrite().EditNameCategory(ar);
        }

        public async Task DeleteCategory(string[] ar)
        {
            //await new JsonWrite().DeleteCategory(ar);
            await new SQLWrite().DeleteCategory(ar);
        }
    }
}





