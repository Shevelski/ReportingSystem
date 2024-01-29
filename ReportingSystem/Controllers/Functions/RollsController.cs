using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class RollsController(RollsService rollsService) : Controller
    {
        private readonly RollsService _rollsService = rollsService;

        [HttpGet]
        //Отримання списку ролей компанії 
        public async Task<IActionResult> GetAllRolls(string idCu, string idCo, string idEm)
        {
            var result = await _rollsService.GetAllRolls(idCu, idCo, idEm);
            return Json(result);
        }
        
        [HttpGet]
        //Отримання списку співробітників за роллю
        public async Task<IActionResult> GetEmployeesByRoll(string idCu, string idCo, string rol)
        {
            var result = await _rollsService.GetEmployeesByRoll(idCu, idCo, rol);
            return Json(result);
        }
       

        [HttpPost]
        // редагування ролі у користувача
        public async Task EditEmployeeRol([FromBody] string[] ar)
        {
            await _rollsService.EditEmployeeRol(ar);
        }
    }
}
