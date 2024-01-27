using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ReportingSystem.Controllers.Functions
{
    public class ConfigurationController : Controller
    {
        //[PasswordProtected(Password = "password")]
        //public IActionResult Configuration()
        //{
        //    return View();
        //}

        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        //public class PasswordProtectedAttribute : AuthorizeAttribute
        //{
        //    public string Password { get; set; }

        //    protected override bool AuthorizeCore(HttpContextBase httpContext)
        //    {
        //        // Перевірка паролю
        //        string enteredPassword = httpContext.Request["password"];
        //        return enteredPassword == Password;
        //    }
        //}
    }
}
