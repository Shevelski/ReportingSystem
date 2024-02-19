namespace ReportingSystem.Models.Weather
{
    public class CityModel
    {
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public WeatherModel? Weather { get; set; }
    }
}
