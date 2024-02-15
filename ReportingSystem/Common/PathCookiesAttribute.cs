using Microsoft.AspNetCore.Mvc.Filters;

namespace ReportingSystem.Common
{
    public class PathCookiesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string? urlCurrent = context.HttpContext.Request.Path.Value;
            context.HttpContext.Response.Cookies.Append("previous", urlCurrent ?? "/Home/StartPage");
        }
    }
}