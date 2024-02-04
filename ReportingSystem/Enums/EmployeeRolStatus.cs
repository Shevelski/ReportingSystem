using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace ReportingSystem.Enums
{
    public enum EmployeeRolStatus
    {
        [EnumDisplayName("Developer", typeof(Resources))]
        Developer = 1,

        [EnumDisplayName("DevAdministrator", typeof(Resources))]
        DevAdministrator = 2,

        [EnumDisplayName("Customer", typeof(Resources))] // бачить всі компанії
        Customer = 3,

        [EnumDisplayName("CEO", typeof(Resources))] // бачить тільки свою компанію
        CEO = 4,

        [EnumDisplayName("Administrator", typeof(Resources))]
        Administrator = 5,

        [EnumDisplayName("ProjectManager", typeof(Resources))]
        ProjectManager = 6,

        [EnumDisplayName("User", typeof(Resources))]
        User = 7,

        [EnumDisplayName("NoUser", typeof(Resources))]
        NoUser = 8
    }
}