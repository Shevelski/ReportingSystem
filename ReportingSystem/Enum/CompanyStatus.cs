using ReportingSystem.Enum.Extensions;

namespace ReportingSystem.Enum
{
    public enum CompanyStatus
    {
        [EnumDisplayName("В проекті")]
        Project = 1,

        [EnumDisplayName("Актуальна")]
        Actual = 2,

        [EnumDisplayName("Архівна")]
        Archive = 3,
    }
}
