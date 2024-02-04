using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum EmployeeStatus
    {
        [EnumDisplayName("ActualSecond", typeof(Resources))]
        Actual = 1,

        [EnumDisplayName("ArchiveSecond", typeof(Resources))]
        Archive = 2,
    }
}