using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.Report;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ReportController(ReportService reportService) : Controller
    {
        private readonly ReportService _reportService = reportService;

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<List<ReportModel>> GetReports(string idCu, string idCo, string idEm, string datestart, string dateend)
        {
            return await _reportService.GetReports(idCu, idCo, idEm, datestart, dateend);
        }

        //[HttpGet]
        ////Отримання списку посад компанії 
        //public async Task<IActionResult> GetEmployeesByRoll(string idCu, string idCo, string rol)
        //{
        //    await Task.Delay(10);
        //    var result = _reportService.GetEmployeesByRoll(idCu, idCo, rol);
        //    return Json(result);
        //}


        [HttpPost]
        // редагування ролі у користувача
        public async Task SendReport([FromBody] string[] ar)
        {
            await _reportService.SendReport(ar);
        }

        [HttpPost]
        // редагування ролі у користувача
        public async Task ClearReport([FromBody] string[] ar)
        {
            await _reportService.ClearReport(ar);
        }

        [HttpPost]
        // редагування ролі у користувача
        public async Task ClearDayReport([FromBody] string[] ar)
        {
            await _reportService.ClearDayReport(ar);
        }
    }
}
