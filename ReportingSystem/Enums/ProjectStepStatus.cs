using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Enums
{
    public enum ProjectStepStatus
    {
        [EnumDisplayName("В очікуванні")]
        Waiting = 1,

        [EnumDisplayName("В роботі")]
        InProcess = 2,

        [EnumDisplayName("Завершений")]
        Complete = 3,

        [EnumDisplayName("Завершений з зауваженнями")]
        Warning = 4,

        [EnumDisplayName("Звернути увагу")]
        Alarm = 5,

    }
}