using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Models.User;

namespace ReportingSystem.Controllers
{

    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            await Task.Delay(10);
            //сюди треба пхати гуід з авторизації
            if (DatabaseMoq.Users != null)
            {
                return Json(DatabaseMoq.Users.FirstOrDefault(u => u.id.Equals(Guid.Parse("f4fe1337-02ce-4149-889d-064729db19a8"))));
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInfo([FromBody] EmployeeModel userInfoModel)
        {
            await Task.Delay(10);

            int userIndex = DatabaseMoq.Users.FindIndex(u => u.id.Equals(userInfoModel.id));

            if (userIndex >= 0)
            {
                DatabaseMoq.Users[userIndex] = userInfoModel;
                return Json(DatabaseMoq.Users);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
