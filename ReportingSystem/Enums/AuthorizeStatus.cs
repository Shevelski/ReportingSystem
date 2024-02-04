using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;
using System.ComponentModel.DataAnnotations;
using ReportingSystem.Common;
namespace ReportingSystem.Enums
{
    public enum AuthorizeStatus
    {
        

        [EnumDisplayName("EmailOk", typeof(Resources))]
        EmailOk = 1,

        [EnumDisplayName("EmailFailed", typeof(Resources))]
        EmailFailed = 2,

        [EnumDisplayName("PasswordOk", typeof(Resources))]
        PasswordOk = 3,

        [EnumDisplayName("PasswordFailed", typeof(Resources))]
        PasswordFailed = 4,
    }
}
