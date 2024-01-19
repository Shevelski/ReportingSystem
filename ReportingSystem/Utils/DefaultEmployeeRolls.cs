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
                rolType = EmployeeRolStatus.Developer,
                rolName = EmployeeRolStatus.Developer.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.DevAdministrator,
                rolName = EmployeeRolStatus.DevAdministrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.Administrator,
                rolName = EmployeeRolStatus.Administrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.ProjectManager,
                rolName = EmployeeRolStatus.ProjectManager.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.Customer,
                rolName = EmployeeRolStatus.Customer.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.CEO,
                rolName = EmployeeRolStatus.CEO.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.User,
                rolName = EmployeeRolStatus.User.GetDisplayName()
            };
            rolls.Add(userRolModel);

            return rolls;
        }

        static public List<EmployeeRolModel> GetForEmployee()
        {
            List<EmployeeRolModel> rolls = [];

            EmployeeRolModel userRolModel = new()
            {
                rolType = EmployeeRolStatus.Administrator,
                rolName = EmployeeRolStatus.Administrator.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.ProjectManager,
                rolName = EmployeeRolStatus.ProjectManager.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.CEO,
                rolName = EmployeeRolStatus.CEO.GetDisplayName()
            };
            rolls.Add(userRolModel);
            userRolModel = new EmployeeRolModel
            {
                rolType = EmployeeRolStatus.User,
                rolName = EmployeeRolStatus.User.GetDisplayName()
            };
            rolls.Add(userRolModel);

            return rolls;
        }

    }
}
