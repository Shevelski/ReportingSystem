using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum CompanyStatus
    {
        [EnumDisplayName("Project", typeof(Resources))]
        Project = 1,

        [EnumDisplayName("Actual", typeof(Resources))]
        Actual = 2,

        [EnumDisplayName("Archive", typeof(Resources))]
        Archive = 3,
    }
}
