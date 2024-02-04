using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum ProjectStepStatus
    {
        [EnumDisplayName("Waiting", typeof(Resources))]
        Waiting = 1,

        [EnumDisplayName("InProcess", typeof(Resources))]
        InProcess = 2,

        [EnumDisplayName("Complete", typeof(Resources))]
        Complete = 3,

        [EnumDisplayName("Warning", typeof(Resources))]
        Warning = 4,

        [EnumDisplayName("Alarm", typeof(Resources))]
        Alarm = 5,

    }
}