using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezingAlgorithme : IAlgorithme<FreezeForecast>
    {
        private readonly IAltitudeClient client;

        public FreezingAlgorithme(IAltitudeClient altitudeClient)
        {
            this.client = altitudeClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device">Device telemetry with temperature in Celsius</param>
        /// <returns></returns>
        public async Task<FreezeForecast> Execute(IWeather device)
        {
            FreezeForecast freezeForecast = new FreezeForecast();
            if (IsFreezing(device))
            {
                freezeForecast.FreezingStart = DateTime.Now;
                return freezeForecast;
            }

            return freezeForecast;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device">Device telemetry with temperature in Celsius</param>
        /// <param name="devicePosition"></param>
        /// <param name="currentWeather">Weather with temperature in Celsius</param>
        /// <param name="forecast">Forecast weather with temperature in Celsius</param>
        /// <param name="forecastStation"></param>
        /// <returns></returns>
        public async Task<FreezeForecast> Execute(IWeather device, IStationPosition devicePosition,
            IWeather currentWeather, IEnumerable<IWeather> forecast, IStationPosition forecastStation)
        {
            FreezeForecast freezeForecast = new FreezeForecast();

            double coefTemperature = device.Temperature / currentWeather.Temperature;
            double coefHumidity = device.Humidity / currentWeather.Humidity;

            if (IsFreezing(device))
            {
                freezeForecast.FreezingStart = currentWeather.Date;
            }
            IWeather theoricWeather = device;

            foreach (IWeather forecastItem in forecast)
            {
                IWeather estimationWeather = await EstimateWeatherByAltitudeDiff(forecastItem, forecastStation, devicePosition);

                theoricWeather.Temperature = coefTemperature * estimationWeather.Temperature;
                theoricWeather.Humidity = coefHumidity * forecastItem.Humidity;

                if (IsFreezing(theoricWeather) && (!freezeForecast.FreezingStart.HasValue))
                {
                    freezeForecast.FreezingStart = forecastItem.Date;
                }
                else if (!IsFreezing(theoricWeather) && (!freezeForecast.FreezingEnd.HasValue))
                {
                    freezeForecast.FreezingEnd = forecastItem.Date;
                }

            }
            if (!freezeForecast.FreezingEnd.HasValue && freezeForecast.FreezingStart.HasValue)
            {
                freezeForecast.FreezingEnd = forecast.OrderBy(e => e.Date).Last().Date;
            }

            return freezeForecast;
        }

        private async Task<IWeather> EstimateWeatherByAltitudeDiff(IWeather weather, IStationPosition forecastStation, IStationPosition expectedStation)
        {
            forecastStation.Altitude = (await client.GetAltitude(forecastStation.Latitude, forecastStation.Longitude)).Altitude;

            double elevationBetweenWeatherStationAndSite = forecastStation.Altitude - expectedStation.Altitude;
            // Only if altitude diff is greater than 100
            if (Math.Abs(elevationBetweenWeatherStationAndSite) >= 100.0)
            {
                weather.Temperature = ConvertTemperature(weather.Temperature, elevationBetweenWeatherStationAndSite);
            }

            return weather;
        }

        private double ConvertTemperature(double temperature, double altitudeDiff)
        {
            double temperatureAdiabatic = (altitudeDiff * (-6.5)) / 1_000;
            double predictedTemperature = temperature + temperatureAdiabatic;
            return predictedTemperature;
        }
        
        // TODO : Clarck, need to be check, tests failed
        private double DewPoint(double humidity, double temperature)
        {
            double result = Math.Pow(humidity / 100.0, 1.0 / 8.0) * (112.0 + (0.9 * temperature)) + (0.1 * temperature) - 112.0;
            return Math.Round(result, 2);
        }
        
        private double FreezingPoint(double dewPoint, double temperature)
        {
            double temperatureK = CelsiusToKelvin(temperature);
            double result = CelsiusToKelvin(dewPoint) + (2671.02 / ((2954.61 / temperatureK) + (2.193665 * Math.Log(temperatureK)) - 13.3448)) - temperatureK;
            return Math.Round(KelvinToCelsius(result), 2);
        }

        // Use only when the temperature is <=0
        private bool IsFreezing(IWeather device)
        {

            //In this case the dew temperature is greater than the freezing temperature
            double dewTemperature = DewPoint(device.Humidity, device.Temperature);
            if (device.Temperature <= dewTemperature)
            {
                return true;
            }

            double freezingTemperature = FreezingPoint(dewTemperature, device.Temperature);
            if (device.Temperature <= freezingTemperature)
            {
                return true;
            }
            return false;
        }

        private double CelsiusToKelvin(double celsiusTemp)
        {
            return celsiusTemp + 273.15;
        }

        private double KelvinToCelsius(double kelvinTemp)
        {
            return kelvinTemp - 273.15;
        }


    }
}
