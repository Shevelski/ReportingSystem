using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Enums
{
    public enum EmployeeStatus
    {
        [EnumDisplayName("Актуальний")]
        Actual = 1,

        [EnumDisplayName("Архівний")]
        Archive = 2,
    }
}