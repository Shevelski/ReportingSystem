using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Services;
using System.Text;

namespace ReportingSystem.Controllers.Functions
{

    public class ServiceController : Controller
    {
        //private readonly CompaniesService _companiesService;

        //public CompaniesController(CompaniesService companiesService)
        //{
        //    _companiesService = companiesService;
        //}

        [HttpGet]
        public async Task<string[]?> GetIds()
        {
            await Task.Delay(10);
            if (HttpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
            {
                var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));
                if (ids != null)
                {
                    return ids;
                }
                
            }
            return null;
        }

    }
}