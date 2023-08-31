using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;

namespace ReportingSystem.Utils
{
    static public class DefaultEmployeeRolls
    {
        static public List<EmployeeRolModel> Get()
        {
            List<EmployeeRolModel> rolls = new List<EmployeeRolModel>();
            EmployeeRolModel userRolModel = new EmployeeRolModel();
            userRolModel.rolType = EmployeeRolStatus.Administrator;
            userRolModel.rolName = EmployeeRolStatus.Administrator.GetDisplayName();
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel();
            userRolModel.rolType = EmployeeRolStatus.ProjectManager;
            userRolModel.rolName = EmployeeRolStatus.ProjectManager.GetDisplayName();
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel();
            userRolModel.rolType = EmployeeRolStatus.Director;
            userRolModel.rolName = EmployeeRolStatus.Director.GetDisplayName();
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel();
            userRolModel.rolType = EmployeeRolStatus.User;
            userRolModel.rolName = EmployeeRolStatus.User.GetDisplayName();
            rolls.Add(userRolModel);

            return rolls;
        }
    }
}
