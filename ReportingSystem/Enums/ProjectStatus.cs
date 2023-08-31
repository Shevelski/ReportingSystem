using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;

namespace ReportingSystem.Enums
{
    public enum ProjectStatus
    {
        [EnumDisplayName("В проекті")]
        Project = 1,

        [EnumDisplayName("В роботі")]
        InProcess = 2,

        [EnumDisplayName("У впровадженні")]
        InImprove = 3,

        [EnumDisplayName("Підтримка")]
        Support = 4,

        [EnumDisplayName("Архівний")]
        Archive = 5,

    }
}