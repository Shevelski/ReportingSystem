using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ReportingSystem.Enums
{
    public enum EmployeeRolStatus
    {
        [EnumDisplayName("Розробник системи")]
        Developer = 1,

        [EnumDisplayName("Адміністратор системи")]
        DevAdministrator = 2,

        [EnumDisplayName("Замовник")] // бачить всі компанії
        Customer = 3,

        [EnumDisplayName("Директор компанії")] // бачить тільки свою компанію
        CEO = 4,

        [EnumDisplayName("Адміністратор")]
        Administrator = 5,

        [EnumDisplayName("Менеджер")]
        ProjectManager = 6,

        [EnumDisplayName("Користувач")]
        User = 7,

        [EnumDisplayName("Не визначено")]
        NoUser = 8
    }
}