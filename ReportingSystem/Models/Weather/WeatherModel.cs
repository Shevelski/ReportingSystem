namespace ReportingSystem.Models.Weather
{
    public class WeatherModel
    {
        public List<DateTime>? DateTime { get; set; }
        public List<double>? Temperature { get; set; }
        public List<double>? Windspeed { get; set; }
        public List<double>? Rain { get; set; }
    }
}
