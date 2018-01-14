using WeatherLibrary.Abstraction;

namespace WeatherLibrary.OpenWeatherMap
{
    public class OwmStationPosition : IStationPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
