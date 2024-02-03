using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum CompanyStatus
    {
        [EnumDisplayName("В проекті", typeof(Resources))]
        Project = 1,

        [EnumDisplayName("Актуальна", typeof(Resources))]
        Actual = 2,

        [EnumDisplayName("Архівна", typeof(Resources))]
        Archive = 3,
    }
}
