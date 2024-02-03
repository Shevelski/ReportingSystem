using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum ProjectStepStatus
    {
        [EnumDisplayName("В очікуванні", typeof(Resources))]
        Waiting = 1,

        [EnumDisplayName("В роботі", typeof(Resources))]
        InProcess = 2,

        [EnumDisplayName("Завершений", typeof(Resources))]
        Complete = 3,

        [EnumDisplayName("Завершений з зауваженнями", typeof(Resources))]
        Warning = 4,

        [EnumDisplayName("Звернути увагу", typeof(Resources))]
        Alarm = 5,

    }
}