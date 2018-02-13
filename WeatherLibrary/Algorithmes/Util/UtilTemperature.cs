using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.Algorithmes.Util
{
    internal class UtilTemperature : IWeatherClient<UtilTemperatureCurrent, UtilTemperatureForecast>
    {
        private readonly IAltitudeClient client;
        private readonly IStationPosition sitePosition;
        private readonly IWeather weatherCurrent;
        private readonly IEnumerable<IWeather> forecast;

        public UtilTemperature(IAltitudeClient client, IStationPosition sitePosition, IWeather weatherCurrent)
        {
            this.client = client;
            this.sitePosition = sitePosition;
            this.weatherCurrent = weatherCurrent;
        }

        public UtilTemperature(IAltitudeClient client, IStationPosition sitePosition, IEnumerable<IWeather> forecast)
        {
            this.client = client;
            this.sitePosition = sitePosition;
            this.forecast = forecast;
        }

        public async Task<UtilTemperatureCurrent> GetCurrentWeather(double latitude, double longitude)
        {

            double siteElevation = (await client.GetAltitude(latitude, longitude)).Altitude;
            double temperatureSite = this.weatherCurrent.Temperature;
            double elevationBetweenWeatherStationAndSite = siteElevation - sitePosition.Altitude;
            if (elevationBetweenWeatherStationAndSite >= 100.0 || elevationBetweenWeatherStationAndSite <= -100.0)
            {
                temperatureSite = ConvertTemperature(this.weatherCurrent.Temperature, elevationBetweenWeatherStationAndSite);
            }
            return new UtilTemperatureCurrent(temperatureSite);

        }

        public async Task<UtilTemperatureForecast> GetForecastWeather(double latitude, double longitude)
        {
            List<double> temperatureSiteList = new List<double>();
            double weatherStationElevation = (await client.GetAltitude(latitude, longitude)).Altitude;
            double elevationBetweenWeatherStationAndSite = weatherStationElevation - sitePosition.Altitude;

            if (elevationBetweenWeatherStationAndSite >= 100.0 || elevationBetweenWeatherStationAndSite <= -100.0)
            {
                foreach (IWeather f in forecast)
                {
                    double temperatureSite = ConvertTemperature(f.Temperature, elevationBetweenWeatherStationAndSite);
                    temperatureSiteList.Add(temperatureSite);
                }
                return new UtilTemperatureForecast(temperatureSiteList);
            }
            else
            {
                return new UtilTemperatureForecast(forecast);
            }

        }

        private double ConvertTemperature(double weatherStationTemperature, double elevationBetweenWeatherStationAndSite)
        {
            double temperatureAdiabatic = (elevationBetweenWeatherStationAndSite * (-6.5)) / 1000;
            double predictedTemperature = weatherStationTemperature + temperatureAdiabatic;
            return predictedTemperature;

        }
    }
}
