using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;

namespace ReportingSystem.Enums
{
    public enum LicenceType
    {
        [EnumDisplayName("None", typeof(Resources))]
        None = 0,

        [EnumDisplayName("Test", typeof(Resources))]
        Test = 1,

        [EnumDisplayName("Main", typeof(Resources))]
        Main = 2,

        [EnumDisplayName("Expired", typeof(Resources))]
        Expired = 3,

        [EnumDisplayName("Archive", typeof(Resources))]
        Archive = 4,

        [EnumDisplayName("Nulled", typeof(Resources))]
        Nulled = 5,
    }
}
