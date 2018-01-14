using System;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.OpenWeatherMap
{
    public class OwmWeather : IWeather
    {
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }

        public DateTime Date { get; set; }
    }
}
