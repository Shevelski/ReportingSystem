using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum LicenceType
    {
        [EnumDisplayName("Неіснуюча", typeof(Resources))]
        None = 0,

        [EnumDisplayName("Тестова", typeof(Resources))]
        Test = 1,

        [EnumDisplayName("Основна", typeof(Resources))]
        Main = 2,

        [EnumDisplayName("Завершена", typeof(Resources))]
        Expired = 3,

        [EnumDisplayName("Архівна", typeof(Resources))]
        Archive = 4,

        [EnumDisplayName("Анульована", typeof(Resources))]
        Nulled = 5,
    }
}
