using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class ReportController(ReportService reportService) : Controller
    {
        private readonly ReportService _reportService = reportService;

        //[HttpGet]
        ////Отримання списку посад компанії 
        //public async Task<IActionResult> GetAllRolls(string idCu, string idCo, string idEm)
        //{
        //    await Task.Delay(10);
        //    var result = _reportService.GetAllRolls(idCu, idCo, idEm);
        //    return Json(result);
        //}

        //[HttpGet]
        ////Отримання списку посад компанії 
        //public async Task<IActionResult> GetEmployeesByRoll(string idCu, string idCo, string rol)
        //{
        //    await Task.Delay(10);
        //    var result = _reportService.GetEmployeesByRoll(idCu, idCo, rol);
        //    return Json(result);
        //}


        //[HttpPost]
        //// редагування ролі у користовуча
        //public async Task<IActionResult> EditEmployeeRol([FromBody] string[] ar)
        //{
        //    await Task.Delay(10);
        //    var result = _reportService.EditEmployeeRol(ar);
        //    return Json(result);
        //}
    }
}
