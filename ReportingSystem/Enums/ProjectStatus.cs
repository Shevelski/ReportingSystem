using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum ProjectStatus
    {
        [EnumDisplayName("В проекті", typeof(Resources))]
        Project = 1,

        [EnumDisplayName("В роботі", typeof(Resources))]
        InProcess = 2,

        [EnumDisplayName("У впровадженні", typeof(Resources))]
        InImprove = 3,

        [EnumDisplayName("Підтримка", typeof(Resources))]
        Support = 4,

        [EnumDisplayName("Архівний", typeof(Resources))]
        Archive = 5,

    }
}