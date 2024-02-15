using Azure.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ReportingSystem.Controllers.Users
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string? cookieValue = Request.Cookies["culture"];

            if (cookieValue == null)
            {
                CultureInfo? currentCulture = HttpContext?.Features?.Get<IRequestCultureFeature>()?.RequestCulture.Culture;
                string? languageCode = currentCulture?.TwoLetterISOLanguageName;
                ViewBag.CookieValue = languageCode;
            }
            else
            {
                ViewBag.CookieValue = cookieValue;
            }
            base.OnActionExecuting(filterContext);
        }


    }

}
