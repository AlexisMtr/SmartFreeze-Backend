using System;

namespace WeatherLibrary.Abstraction
{
    public interface IWeather
    {
        double Pressure { get; set; }
        double Humidity { get; set; }
        double Temperature { get; set; }
        double WindSpeed { get; set; }

        DateTime Date { get; set; }
    }
}
