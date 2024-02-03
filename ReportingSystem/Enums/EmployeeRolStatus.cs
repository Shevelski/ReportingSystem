using ReportingSystem.Enums;
using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models;
using ReportingSystem.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace ReportingSystem.Enums
{
    public enum EmployeeRolStatus
    {
        [EnumDisplayName("Розробник системи", typeof(Resources))]
        Developer = 1,

        [EnumDisplayName("Адміністратор системи", typeof(Resources))]
        DevAdministrator = 2,

        [EnumDisplayName("Замовник", typeof(Resources))] // бачить всі компанії
        Customer = 3,

        [EnumDisplayName("Директор компанії", typeof(Resources))] // бачить тільки свою компанію
        CEO = 4,

        [EnumDisplayName("Адміністратор", typeof(Resources))]
        Administrator = 5,

        [EnumDisplayName("Менеджер", typeof(Resources))]
        ProjectManager = 6,

        [EnumDisplayName("Користувач", typeof(Resources))]
        User = 7,

        [EnumDisplayName("Не визначено", typeof(Resources))]
        NoUser = 8
    }
}