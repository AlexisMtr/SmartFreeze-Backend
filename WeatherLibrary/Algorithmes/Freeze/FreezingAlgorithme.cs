using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezingAlgorithme : IAlgorithme<FreezeForecast>
    {
        public Task<FreezeForecast> Execute(IWeather current, IStationPosition currentStation,
            IEnumerable<IWeather> forecast, IStationPosition forecastStation)
        {
            throw new NotImplementedException();
        }
    }
}
