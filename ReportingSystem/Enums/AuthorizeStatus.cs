using ReportingSystem.Enums.Extensions;

namespace ReportingSystem.Enums
{
    public enum AuthorizeStatus
    {

        [EnumDisplayName("Email вірний. Введіть пароль")]
        //[EnumDisplayName("Email вірний. Введіть пароль")]
        EmailOk = 1,

        [EnumDisplayName("Користувача з таким Email не існує. Бажаєте зареєструватися в системі як замовник?")]
        EmailFailed = 2,

        [EnumDisplayName("Пароль вірний. Перехід на сторінку")]
        PasswordOk = 3,

        [EnumDisplayName("Пароль невірний. Повторіть спробу")]
        PasswordFailed = 4,
    }
}
