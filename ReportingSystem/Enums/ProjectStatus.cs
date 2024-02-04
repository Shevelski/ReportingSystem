using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum ProjectStatus
    {
        [EnumDisplayName("Project", typeof(Resources))]
        Project = 1,

        [EnumDisplayName("InProcess", typeof(Resources))]
        InProcess = 2,

        [EnumDisplayName("InImprove", typeof(Resources))]
        InImprove = 3,

        [EnumDisplayName("Support", typeof(Resources))]
        Support = 4,

        [EnumDisplayName("ArchiveSecond", typeof(Resources))]
        Archive = 5,

    }
}