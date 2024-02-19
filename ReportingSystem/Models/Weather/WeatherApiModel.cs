using Newtonsoft.Json;

namespace ReportingSystem.Models.Weather
{
    public class WeatherApiModel
    {
        public class HourlyData
        {
            [JsonProperty("time")]
            public List<string> Time { get; set; }

            [JsonProperty("rain")]
            public List<double> Rain { get; set; }

            [JsonProperty("wind_speed_180m")]
            public List<double> WindSpeed180m { get; set; }

            [JsonProperty("temperature_80m")]
            public List<double> Temperature80m { get; set; }
        }

        public class WeatherData
        {
            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            [JsonProperty("generationtime_ms")]
            public double GenerationTimeMs { get; set; }

            [JsonProperty("utc_offset_seconds")]
            public int UtcOffsetSeconds { get; set; }

            [JsonProperty("timezone")]
            public string Timezone { get; set; }

            [JsonProperty("timezone_abbreviation")]
            public string TimezoneAbbreviation { get; set; }

            [JsonProperty("elevation")]
            public double Elevation { get; set; }

            [JsonProperty("hourly_units")]
            public Dictionary<string, string> HourlyUnits { get; set; }

            [JsonProperty("hourly")]
            public HourlyData Hourly { get; set; }
        }
    }
}
