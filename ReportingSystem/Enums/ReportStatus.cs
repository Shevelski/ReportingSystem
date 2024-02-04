using ReportingSystem.Enums.Extensions;
using ReportingSystem.Models.Project;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum ReportStatus
    {
        [EnumDisplayName("Підтверджено працівником", typeof(Resources))]
        Employee = 1,

        [EnumDisplayName("Підтверджено керівником", typeof(Resources))]
        Manager = 2,

        [EnumDisplayName("До змін керівником", typeof(Resources))]
        EditManager = 3,

        [EnumDisplayName("Відхилено керівником", typeof(Resources))]
        NoManager = 4,

        [EnumDisplayName("Затверджений", typeof(Resources))]
        Good = 5,

    }
}