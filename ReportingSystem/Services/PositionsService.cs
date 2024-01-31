using ReportingSystem.Data.JSON;
using ReportingSystem.Data.SQL;
using ReportingSystem.Models.User;
using ReportingSystem.Utils;

namespace ReportingSystem.Services
{
    public class PositionsService
    {

        //Отримання списку посад компанії 
        public async Task<List<EmployeePositionModel>?> GetAllPositions(string idCu, string idCo)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetAllPositions(idCu, idCo) :
                      await new SQLRead().GetAllPositions(idCu, idCo);
            return result;
        }

        //Отримання списку посад компанії 
        public async Task<List<EmployeePositionEmpModel>?> GetAllPositionsWithEmployee(string idCu, string idCo)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetAllPositionsWithEmployee(idCu, idCo) :
                      await new SQLRead().GetAllPositionsWithEmployee(idCu, idCo);
            return result;
        }


        //Отримання списку унікальних посад компанії 
        public async Task<List<EmployeePositionModel>?> GetUniqPositions(string idCu, string idCo)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetUniqPositions(idCu, idCo) :
                      await new SQLRead().GetUniqPositions(idCu, idCo);
            return result;
        }

        //отримати користувачів компанії за ролями
        public async Task<List<EmployeeModel>?> GetEmployeesByPosition(string idCu, string idCo, string pos)
        {
            bool mode = Settings.Source().Equals("json");
            var result = mode ? await new JsonRead().GetEmployeesByPosition(idCu, idCo, pos) :
                      await new SQLRead().GetEmployeesByPosition(idCu, idCo, pos);
            return result;
        }

        //створення нової посади
        public async Task CreatePosition(string[] ar)
        {
            //await new JsonWrite().CreatePosition(ar);
            await new SQLWrite().CreatePosition(ar);
        }


        //створення нової посади
        public async Task EditPosition(string[] ar)
        {
            //await new JsonWrite().EditPosition(ar);
            await new SQLWrite().EditPosition(ar);
        }

        //видалення посади
        public async Task DeletePosition(string[] ar)
        {
            //await new JsonWrite().DeletePosition(ar);
            await new SQLWrite().DeletePosition(ar);
        }

        //зміна посади
        public async Task EditEmployeePosition(string[] ar)
        {
            //await new JsonWrite().EditEmployeePosition(ar);
            await new SQLWrite().EditEmployeePosition(ar);
        }
    }
}
