using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class PositionsService
    {
        bool mode = Settings.Source().Equals("json");
        //Отримання списку посад компанії 
        public async Task<List<EmployeePositionModel>?> GetAllPositions(string idCu, string idCo)
        {
            return mode ? await new JsonRead().GetAllPositions(idCu, idCo) : await new SQLRead().GetAllPositions(idCu, idCo);
        }

        //Отримання списку посад компанії 
        public async Task<List<EmployeePositionEmpModel>?> GetAllPositionsWithEmployee(string idCu, string idCo)
        {
            return mode ? await new JsonRead().GetAllPositionsWithEmployee(idCu, idCo) : await new SQLRead().GetAllPositionsWithEmployee(idCu, idCo);
        }

        //Отримання списку унікальних посад компанії 
        public async Task<List<EmployeePositionModel>?> GetUniqPositions(string idCu, string idCo)
        {
            return mode ? await new JsonRead().GetUniqPositions(idCu, idCo) : await new SQLRead().GetUniqPositions(idCu, idCo);
        }

        //отримати користувачів компанії за ролями
        public async Task<List<EmployeeModel>?> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            return mode ? await new JsonRead().GetEmployeesByPosition(idCu, idCo, pos) : await new SQLRead().GetEmployeesByPosition(idCu, idCo, pos);
        }

        //створення нової посади
        public async Task CreatePosition(string[] ar)
        {
            await (mode ? new JsonWrite().CreatePosition(ar) : new SQLWrite().CreatePosition(ar));
        }

        //створення нової посади
        public async Task EditPosition(string[] ar)
        {
            await (mode ? new JsonWrite().EditPosition(ar) : new SQLWrite().EditPosition(ar));
        }

        //видалення посади
        public async Task DeletePosition(string[] ar)
        {
            await (mode ? new JsonWrite().DeletePosition(ar) : new SQLWrite().DeletePosition(ar));
        }

        //зміна посади
        public async Task EditEmployeePosition(string[] ar)
        {
            await (mode ? new JsonWrite().EditEmployeePosition(ar) : new SQLWrite().EditEmployeePosition(ar));
        }
    }
}
