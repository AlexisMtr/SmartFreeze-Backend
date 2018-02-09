using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherLibrary.Abstraction
{
    public interface IAlgorithme<TForecast>
    {
        Task<TForecast> Execute(IWeather device, IStationPosition devicePosition);

        Task<TForecast> Execute(IWeather device, IStationPosition devicePosition, IWeather currentWeather, IEnumerable<IWeather> forecast, IStationPosition forecastStation);

        double DewPoint(double humidity, double temperature);

        double FreezingPoint(double dewPoint, double temperature);

        bool isFreezing(IWeather device);

        bool IsDewing(IWeather device);
    }

}
