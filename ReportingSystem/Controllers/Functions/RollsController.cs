using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class RollsController : Controller
    {
        private readonly RollsService _rollsService;

        public RollsController(RollsService rollsService)
        {
            _rollsService = rollsService;
        }

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetAllPositions(string idCu, string idCo)
        {
            await Task.Delay(10);
            var result = _rollsService.GetAllPositions(idCu, idCo);
            return Json(result);
        }

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetUniqPositions(string idCu, string idCo)
        {
            await Task.Delay(10);
            var result = _rollsService.GetUniqPositions(idCu, idCo);
            return Json(result);
        }

        
        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            await Task.Delay(10);
            var result = _rollsService.GetEmployeesByPosition(idCu, idCo, pos);
            return Json(result);
        }
       

        [HttpPost]
        // створення нової посади
        public async Task<IActionResult> CreatePosition([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _rollsService.CreatePosition(ar);
            return Json(result);
        }

        [HttpPost]
        // видалення посади
        public async Task<IActionResult> DeletePosition([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _rollsService.DeletePosition(ar);
            return Json(result);
        }

        [HttpPost]
        // редагування посади у користовуча
        public async Task<IActionResult> EditEmployeePosition([FromBody] string[] ar)
        {
            await Task.Delay(10);
            var result = _rollsService.EditEmployeePosition(ar);
            return Json(result);
        }
        
    }
}
