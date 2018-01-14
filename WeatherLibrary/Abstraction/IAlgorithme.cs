using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherLibrary.Abstraction
{
    public interface IAlgorithme<TForecast>
    {
        Task<TForecast> Execute(IWeather current, IStationPosition currentStation,
            IEnumerable<IWeather> forecast, IStationPosition forecastStation);
    }
}
