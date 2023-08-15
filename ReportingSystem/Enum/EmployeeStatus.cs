using ReportingSystem.Enum.Extensions;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Enum
{
    public enum EmployeeStatus
    {
        [EnumDisplayName("Актуальний")]
        Actual = 1,

        [EnumDisplayName("Архівник")]
        Archive = 2,
    }
}