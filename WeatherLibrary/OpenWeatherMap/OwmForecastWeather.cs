using System.Collections.Generic;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.OpenWeatherMap
{
    public class OwmForecastWeather
    {
        public IStationPosition StationPosition { get; set; }
        public IEnumerable<IWeather> Forecast { get; set; }
    }
}
