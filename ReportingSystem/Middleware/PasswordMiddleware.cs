using Microsoft.AspNetCore.Mvc;

namespace ReportingSystem.Middleware
{
    public class PasswordMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _password;

        public PasswordMiddleware(RequestDelegate next, string password)
        {
            _next = next;
            _password = password;
        }

        public async Task Invoke(HttpContext context)
        {
            string enteredPassword = context.Request.Query["password"];

            if (enteredPassword == _password)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized. Please enter the correct password.");
            }
        }
    }
}
