using ReportingSystem.Enums.Extensions;

namespace ReportingSystem.Enums
{
    public enum LicenceType
    {
        [EnumDisplayName("Неіснуюча")]
        None = 0,

        [EnumDisplayName("Тестова")]
        Test = 1,

        [EnumDisplayName("Основна")]
        Main = 2,

        [EnumDisplayName("Завершена")]
        Expired = 3,

        [EnumDisplayName("Архівна")]
        Archive = 4,

        [EnumDisplayName("Анульована")]
        Nulled = 5,
    }
}
