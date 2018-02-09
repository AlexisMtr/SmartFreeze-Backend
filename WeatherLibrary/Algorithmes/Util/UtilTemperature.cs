using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Algorithmes.Util
{
    public class UtilTemperature : IWeatherClient<UtilTemperatureCurrent, UtilTemperatureForecast>
    {
        private readonly GoogleMapElevationClient client;
        private IStationPosition siteStationPosition;
        private IWeather weatherCurrent;
        private IEnumerable<IWeather> forecast;

        public UtilTemperature(GoogleMapElevationClient client, IStationPosition siteStationPosition, IWeather weatherCurrent)
        {
            this.client = client;
            this.siteStationPosition = siteStationPosition;
            this.weatherCurrent = weatherCurrent;
        }

        public UtilTemperature(GoogleMapElevationClient client, IStationPosition siteStationPosition, IEnumerable<IWeather> forecast)
        {
            this.client = client;
            this.siteStationPosition = siteStationPosition;
            this.forecast = forecast;
        }

        public async Task<UtilTemperatureCurrent> GetCurrentWeather(double latitude, double longitude)
        {

            double siteElevation = client.GetAltitude(latitude, longitude).Result[0].Elevation;
            double temperatureSite = this.weatherCurrent.Temperature;
            double elevationBetweenWeatherStationAndSite = siteElevation - siteStationPosition.Altitude;
            if (elevationBetweenWeatherStationAndSite >= 100.0 || elevationBetweenWeatherStationAndSite <= -100.0)
            {
                temperatureSite = ConvertTemperature(this.weatherCurrent.Temperature, elevationBetweenWeatherStationAndSite);
            }
            return new UtilTemperatureCurrent(temperatureSite);

        }

        public async Task<UtilTemperatureForecast> GetForecastWeather(double latitude, double longitude)
        {
            List<double> temperatureSiteList = new List<double>();
            double weatherStationElevation = client.GetAltitude(latitude, longitude).Result[0].Elevation;
            double elevationBetweenWeatherStationAndSite = weatherStationElevation - siteStationPosition.Altitude;

            if (elevationBetweenWeatherStationAndSite >= 100.0)
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
