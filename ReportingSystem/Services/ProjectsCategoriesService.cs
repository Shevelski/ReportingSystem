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
        bool mode = Settings.Source().Equals("json");
        public async Task<List<ProjectCategoryModel>?> GetCategories(string idCu, string idCo)
        {
            return mode ? await new JsonRead().GetCategories(idCu, idCo) : await new SQLRead().GetCategories(idCu, idCo);
        }

        public async Task CreateCategory(string[] ar)
        {
            await (mode ? new JsonWrite().CreateCategory(ar) : new SQLWrite().CreateCategory(ar));//звернути увагу під час тестування json
        }
        public async Task EditNameCategory(string[] ar)
        {
            await (mode ? new JsonWrite().EditNameCategory(ar) : new SQLWrite().EditNameCategory(ar));
        }

        public async Task DeleteCategory(string[] ar)
        {
            await (mode ? new JsonWrite().DeleteCategory(ar) : new SQLWrite().DeleteCategory(ar));
        }
    }
}





