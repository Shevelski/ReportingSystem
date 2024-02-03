using ReportingSystem.Enums;
using ReportingSystem;
using ReportingSystem.Models.Company;
using ReportingSystem.Models.Customer;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Utils;
using ReportingSystem.Models.User;
using ReportingSystem.Models;
using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;

namespace ReportingSystem.Services
{
    public class RollsService
    {
        bool mode = Settings.Source().Equals("json");

        //Отримання списку посад компанії 
        public async Task<List<EmployeeRolModel>?> GetAllRolls(string idCu, string idCo)
        {
            var result = mode ? await new JsonRead().GetRolls(idCu, idCo) :
                      await new SQLRead().GetRolls(idCu, idCo);
            return result;
        }

        //отримати користувачів компанії за ролями
        public async Task<List<EmployeeModel>?> GetEmployeesByRoll(string idCu, string idCo, string rol)
        {
            var result = mode ? await new JsonRead().GetEmployeesByRoll(idCu, idCo, rol) :
                      await new SQLRead().GetEmployeesByRoll(idCu, idCo, rol);
            return result;
        }

        //зміна ролі
        public async Task EditEmployeeRol(string[] ar)
        {
            await (mode ? new JsonWrite().EditEmployeeRol(ar) : new SQLWrite().EditEmployeeRol(ar));
        }
    }
}
