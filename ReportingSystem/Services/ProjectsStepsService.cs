using Microsoft.AspNetCore.Mvc;
using ReportingSystem;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.Project;
using ReportingSystem.Models.Project.Step;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class ProjectsStepsService
    {
        bool mode = Settings.Source().Equals("json");

        public async Task<List<ProjectStepModel>?> GetStepsProjects(string idCu, string idCo, string idPr)
        {
            //return mode ? await new JsonRead().GetStepsProjects(idCu, idCo) : 
                return await new SQLRead().GetStepsProjects(idCu, idCo, idPr);
        }

        //public async Task CreateCategory(string[] ar)
        //{
        //    await (mode ? new JsonWrite().CreateCategory(ar) : new SQLWrite().CreateCategory(ar));//звернути увагу під час тестування json
        //}
        //public async Task EditNameCategory(string[] ar)
        //{
        //    await (mode ? new JsonWrite().EditNameCategory(ar) : new SQLWrite().EditNameCategory(ar));
        //}

        //public async Task DeleteCategory(string[] ar)
        //{
        //    await (mode ? new JsonWrite().DeleteCategory(ar) : new SQLWrite().DeleteCategory(ar));
        //}
    }
}





