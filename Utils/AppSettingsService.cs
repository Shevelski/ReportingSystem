using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class AppSettingsService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:7042/api/appsettings/";

        public AppSettingsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAppSetting(string key)
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}{key}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

            throw new Exception($"Failed to retrieve app setting. Status code: {response.StatusCode}");
        }
    }
}
