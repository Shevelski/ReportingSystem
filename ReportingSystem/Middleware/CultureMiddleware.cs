using System.Globalization;

namespace ReportingSystem.Middleware
{
    public class CultureMiddleware
    {
        RequestDelegate _next;
        string _culture;
        public CultureMiddleware(RequestDelegate next, string culture = "uk-UA")
        {
            _next = next;
            _culture = culture;
        }

        public async Task InvokeAsync(HttpContext http)
        {

            if (http.Request.Cookies.TryGetValue("culture", out string? culture))
            {
                culture ??= _culture;

                CultureInfo[] supported = CultureInfo.GetCultures(CultureTypes.AllCultures);

                if (supported.Any(c => c.Name == culture))
                {
                    CultureInfo culture1 = new CultureInfo(culture);
                    CultureInfo.CurrentCulture = culture1;
                    CultureInfo.CurrentUICulture = culture1;
                }
            }
            await _next(http);
        }
    }
}