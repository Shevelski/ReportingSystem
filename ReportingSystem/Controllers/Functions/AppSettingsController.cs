using Microsoft.AspNetCore.Mvc;
using ReportingSystem.Data;

namespace ReportingSystem.Controllers.Functions
{
    [Route("api/appsettings")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {

        [HttpGet("{key}")]
        public async Task<string> GetSetting(string key)
        {
            await Task.Delay(10);
            string value = "";
            
            if (key.Equals("Server"))
            {
                value = Context.serverName;
            }
            if (key.Equals("Db"))
            {
                value = Context.dbName;
            }
            if (key.Equals("Login"))
            {
                value = Context.login;
            }
            if (key.Equals("Password"))
            {
                value = Context.password;
            }

            return value;
        }
    }
}
