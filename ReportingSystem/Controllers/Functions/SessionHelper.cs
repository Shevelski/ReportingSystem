using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Text;

namespace ReportingSystem.Controllers.Functions
{
    //функція для виклику сторінки з вкладанням сесійних даних
    public static class SessionHelper
    {
        public static IActionResult ViewDataSession(HttpContext httpContext)
        {
            if (httpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
            {
                var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));

                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ids
                };

                return new PartialViewResult
                {
                    ViewData = viewData
                };
            }
            else
            {
                httpContext.SignOutAsync();
                return new RedirectToActionResult("Authorize", "Home", null);
            }
        }
        public static IActionResult ViewDataFullSession(HttpContext httpContext)
        {
            if (httpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
            {
                var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));

                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = ids
                };

                return new ViewResult
                {
                    ViewData = viewData
                };
            }
            else
            {
                httpContext.SignOutAsync();
                return new RedirectToActionResult("Authorize", "Home", null);
            }
        }
    }

}
