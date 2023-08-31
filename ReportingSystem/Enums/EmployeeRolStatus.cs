using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ReportingSystem.Enums
{
    public enum EmployeeRolStatus
    {
        [EnumDisplayName("Розробник")]
        Developer = 1,

        [EnumDisplayName("ІАдміністратор")]
        DevAdministrator = 2,

        [EnumDisplayName("Директор")]
        Director = 3,

        [EnumDisplayName("Адміністратор")]
        Administrator = 4,

        [EnumDisplayName("Менеджер")]
        ProjectManager = 5,

        [EnumDisplayName("Користувач")]
        User = 6,

        [EnumDisplayName("Не визначено")]
        NoUser = 7
    }
}