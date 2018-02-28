using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherLibrary.Abstraction
{
    public interface IAlgorithme<TForecast>
    {
        Task<TForecast> Execute(IWeather device);

        Task<TForecast> Execute(IWeather device, IStationPosition devicePosition,
            IWeather currentWeather, IEnumerable<IWeather> forecast, IStationPosition forecastStation);
    }

}
