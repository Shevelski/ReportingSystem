using ReportingSystem.Enum.Extensions;

namespace ReportingSystem.Enum
{
    public enum LicenceType
    {
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
