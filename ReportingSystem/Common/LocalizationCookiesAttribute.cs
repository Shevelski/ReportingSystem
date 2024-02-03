using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ReportingSystem.Common
{
    public class LocalizationCookiesAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Request.Query.TryGetValue("culture", out StringValues culture);
            if (culture != StringValues.Empty)
            {
                context.HttpContext.Response.Cookies.Append("culture", culture);
            }
        }
    }
}