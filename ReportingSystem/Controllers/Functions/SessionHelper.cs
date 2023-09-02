using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReportingSystem.Controllers.Functions
{
    public static class SessionHelper
    {
        public static IActionResult ViewWithIdsFromSession(HttpContext httpContext, string view = "Index")
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
                    ViewName = view,
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


//private IActionResult GetIdsFromSession()
//{
//    if (HttpContext.Session.TryGetValue("ids", out byte[]? idsBytes))
//    {
//        var ids = JsonConvert.DeserializeObject<string[]>(Encoding.UTF8.GetString(idsBytes));
//        return View(ids);
//    }
//    else
//    {
//        HttpContext.SignOutAsync();
//        return RedirectToAction("Authorize", "Home");
//    }
//}