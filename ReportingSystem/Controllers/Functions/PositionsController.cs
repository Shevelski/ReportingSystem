using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Services;

namespace ReportingSystem.Controllers.Functions
{

    public class PositionsController(PositionsService positionsService) : Controller
    {
        private readonly PositionsService _positionsService = positionsService;

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetAllPositions(string idCu, string idCo)
        {
            var result = await _positionsService.GetAllPositions(idCu, idCo);
            return Json(result);
        }

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetAllPositionsWithEmployee(string idCu, string idCo)
        {
            var result = await _positionsService.GetAllPositionsWithEmployee(idCu, idCo);
            return Json(result);
        }

        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetUniqPositions(string idCu, string idCo)
        {
            var result = await _positionsService.GetUniqPositions(idCu, idCo);
            return Json(result);
        }

        
        [HttpGet]
        //Отримання списку посад компанії 
        public async Task<IActionResult> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            var result = await _positionsService.GetEmployeesByPosition(idCu, idCo, pos);
            return Json(result);
        }


        [HttpPost]
        // створення нової посади
        public async Task CreatePosition([FromBody] string[] ar)
        {
             await _positionsService.CreatePosition(ar);
        }

        [HttpPost]
        // видалення посади
        public async Task DeletePosition([FromBody] string[] ar)
        {
            await _positionsService.DeletePosition(ar);
        }

        [HttpPost]
        // редагування посади у користувача
        public async Task EditEmployeePosition([FromBody] string[] ar)
        {
            await _positionsService.EditEmployeePosition(ar);
        }
    }
}
