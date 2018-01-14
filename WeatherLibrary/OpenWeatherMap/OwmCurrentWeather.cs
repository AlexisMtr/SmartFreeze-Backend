using WeatherLibrary.Abstraction;

namespace WeatherLibrary.OpenWeatherMap
{
    public class OwmCurrentWeather
    {
        public IWeather Weather { get; set; }
        public IStationPosition StationPosition { get; set; }
    }
}
