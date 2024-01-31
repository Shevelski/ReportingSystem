using ReportingSystem.Enums.Extensions;
using ReportingSystem.Properties;
using System.ComponentModel.DataAnnotations;

namespace ReportingSystem.Enums
{
    public enum AuthorizeStatus
    {

        [EnumDisplayName("Email вірний. Введіть пароль")]
        [Display(Name = "EmailOk", ResourceType = typeof(Resources))]
        EmailOk = 1,

        [EnumDisplayName("Користувача з таким Email не існує. Бажаєте зареєструватися в системі як замовник?")]
        [Display(Name = "EmailOk", ResourceType = typeof(Resources))]
        EmailFailed = 2,

        [EnumDisplayName("Пароль вірний. Перехід на сторінку")]
        [Display(Name = "EmailOk", ResourceType = typeof(Resources))]
        PasswordOk = 3,

        [EnumDisplayName("Пароль невірний. Повторіть спробу")]
        [Display(Name = "EmailOk", ResourceType = typeof(Resources))]
        PasswordFailed = 4,
    }
}
