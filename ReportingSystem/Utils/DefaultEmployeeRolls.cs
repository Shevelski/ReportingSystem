using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;

namespace ReportingSystem.Utils
{
    static public class DefaultEmployeeRolls
    {
        static public List<EmployeeRolModel> Get()
        {
            List<EmployeeRolModel> rolls = [];

            EmployeeRolModel userRolModel = new();
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.Developer,
                RolName = EmployeeRolStatus.Developer.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.DevAdministrator,
                RolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.Administrator,
                RolName = EmployeeRolStatus.Administrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.ProjectManager,
                RolName = EmployeeRolStatus.ProjectManager.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.Customer,
                RolName = EmployeeRolStatus.Customer.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.CEO,
                RolName = EmployeeRolStatus.CEO.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.User,
                RolName = EmployeeRolStatus.User.GetDisplayName()
            };
            rolls.Add(userRolModel);

            return rolls;
        }

        static public List<EmployeeRolModel> GetForEmployee()
        {
            List<EmployeeRolModel> rolls = [];

            EmployeeRolModel userRolModel = new()
            {
                RolType = EmployeeRolStatus.Administrator,
                RolName = EmployeeRolStatus.Administrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.ProjectManager,
                RolName = EmployeeRolStatus.ProjectManager.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.CEO,
                RolName = EmployeeRolStatus.CEO.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                RolType = EmployeeRolStatus.User,
                RolName = EmployeeRolStatus.User.GetDisplayName()
            };
            rolls.Add(userRolModel);

            return rolls;
        }

    }
}
