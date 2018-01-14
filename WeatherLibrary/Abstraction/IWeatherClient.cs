using System.Threading.Tasks;

namespace WeatherLibrary.Abstraction
{
    public interface IWeatherClient<TCurrent, TForecast>
    {
        Task<TCurrent> GetCurrentWeather(double latitude, double longitude);
        Task<TForecast> GetForecastWeather(double latitude, double longitude);
    }
}
