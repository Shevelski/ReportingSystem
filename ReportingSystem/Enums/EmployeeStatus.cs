using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum EmployeeStatus
    {
        [EnumDisplayName("Актуальний", typeof(Resources))]
        Actual = 1,

        [EnumDisplayName("Архівний", typeof(Resources))]
        Archive = 2,
    }
}