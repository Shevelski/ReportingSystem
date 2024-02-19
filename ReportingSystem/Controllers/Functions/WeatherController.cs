using Bogus.Bson;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportingSystem.Models.Weather;
using System.Diagnostics;
using System.Net.Http;
using static ReportingSystem.Models.Weather.WeatherApiModel;

namespace ReportingSystem.Controllers.Functions
{
    public class WeatherController : Controller
    {
        private static Dictionary<string,DateTime> SaveDateTimeLastGetWeather = new();
        private static Dictionary<string,WeatherModel> SaveDateTimeLastGetWeatherData = new();

        [HttpGet]
        public async Task<List<CityModel>> Cities()
        {

            List<CityModel> cities = new List<CityModel>();
            CityModel city = new CityModel();
            city.Name = "Київ";
            city.Longitude = 30.523333;
            city.Latitude = 50.450001;
            city.Weather = await GetWeather(city);
            cities.Add(city);

            city = new CityModel();
            city.Name = "Харків";
            city.Longitude = 36.232845;
            city.Latitude = 49.988358;
            city.Weather = await GetWeather(city);
            cities.Add(city);

            city = new CityModel();
            city.Name = "Одеса";
            city.Longitude = 30.712481;
            city.Latitude = 46.482952;
            city.Weather = await GetWeather(city);
            cities.Add(city);

            city = new CityModel();
            city.Name = "Львів";
            city.Longitude = 24.031111;
            city.Latitude = 49.842957;
            city.Weather = await GetWeather(city);
            cities.Add(city);

            return cities;
        }
        private async Task<WeatherModel> GetWeather(CityModel city)
        {
            DateTime now = DateTime.Now;
            DateTime currentRounded = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            DateTime previousRounded = currentRounded.AddHours(-1);
            WeatherModel weatherModel = new WeatherModel();

            if (SaveDateTimeLastGetWeather != null && SaveDateTimeLastGetWeather.ContainsKey(city.Name))
            {
                // Отримання значення по ключу
                DateTime savedDateTime = SaveDateTimeLastGetWeather[city.Name];

                if (savedDateTime != currentRounded)
                {
                    SaveDateTimeLastGetWeather.Clear();
                    SaveDateTimeLastGetWeather.Add(city.Name, currentRounded);

                    WeatherModel weather = await GetWeatherFromApi(city);
                    SaveDateTimeLastGetWeatherData.Remove(city.Name);
                    SaveDateTimeLastGetWeatherData.Add(city.Name, weather);

                    weatherModel = weather; 
                    return weatherModel;
                } else
                {
                    SaveDateTimeLastGetWeatherData.TryGetValue(city.Name, out WeatherModel? weather);
                    weatherModel = weather;
                    return weatherModel;
                }
            } else
            {
                SaveDateTimeLastGetWeather.Add(city.Name, now);
                WeatherModel weather = await GetWeatherFromApi(city);
                weatherModel = weather;
                SaveDateTimeLastGetWeatherData.Remove(city.Name);
                SaveDateTimeLastGetWeatherData.Add(city.Name, weather);
                return weatherModel;
            }
        }
        private async Task<WeatherModel> GetWeatherFromApi(CityModel city)
        {

            string lat = city.Latitude.ToString().Replace(",",".");
            string lon = city.Longitude.ToString().Replace(",",".");
            var apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&hourly=rain,wind_speed_180m,temperature_80m&past_days=2&forecast_days=3";

            WeatherModel weatherResult = new();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
                        
                        WeatherModel weather = new WeatherModel();
                        weather.Temperature = weatherData.Hourly.Temperature80m;
                        weather.Windspeed = weatherData.Hourly.WindSpeed180m;
                        weather.DateTime = weatherData.Hourly.Time.Select(DateTime.Parse).ToList();
                        weather.Rain = weatherData.Hourly.Rain;
                        weatherResult = weather;
                        return weatherResult;
                    }
                    else
                    {
                        Console.WriteLine($"Помилка отримання даних. Код статусу: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            return weatherResult;

        }
    }
}
