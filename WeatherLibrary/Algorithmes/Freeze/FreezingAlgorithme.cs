using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Util;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezingAlgorithme : IAlgorithme<FreezeForecast>
    {
        private readonly IAltitudeClient client;

        public FreezingAlgorithme(IAltitudeClient altitudeClient)
        {
            this.client = altitudeClient;
        }

        public FreezingAlgorithme()
        {
            this.client = new GoogleMapElevationClient();
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
            UtilTemperature temperature = new UtilTemperature(client,forecastStation, forecast);
            double coefTemperature = device.Temperature / currentWeather.Temperature;
            double coefHumidity = device.Humidity / currentWeather.Humidity;

            if (IsFreezing(device))
            {
                freezeForecast.FreezingStart = currentWeather.Date;
            }
            IWeather deviceTheoric = device;

            foreach (IWeather current in forecast)
            {
                UtilTemperatureCurrent temperatureCurrent = await temperature.GetCurrentWeather(forecastStation.Latitude, forecastStation.Longitude);
                deviceTheoric.Temperature = coefTemperature * temperatureCurrent.Temperature;
                deviceTheoric.Humidity = coefHumidity * current.Humidity;

                if (IsFreezing(deviceTheoric) && (!freezeForecast.FreezingStart.HasValue))
                {
                    freezeForecast.FreezingStart = current.Date;
                }
                else if (!IsFreezing(deviceTheoric) && (!freezeForecast.FreezingEnd.HasValue))
                {
                    freezeForecast.FreezingEnd = current.Date;
                }

            }
            if (!freezeForecast.FreezingEnd.HasValue && freezeForecast.FreezingStart.HasValue)
            {
                freezeForecast.FreezingEnd = forecast.OrderBy(e => e.Date).Last().Date;
            }
            return freezeForecast;

        }


        /// <summary>
        /// temperature in Celsius
        /// return the dew point temperature in CELSIUS
        /// </summary>
        /// <param name="humidity"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        private double DewPoint(double humidity, double temperature)
        {
            double result = Math.Pow(humidity / 100.0, 1.0 / 8.0) * (112.0 + (0.9 * temperature)) + (0.1 * temperature) - 112.0;
            return Math.Round(result, 2);
        }

        /// <summary>
        /// temperature in Celsius
        /// return the freezing point temperature in CELSIUS
        /// </summary>
        /// <param name="dewPoint"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        private double FreezingPoint(double dewPoint, double temperature)
        {
            double temperatureK = CelsiusToKelvin(temperature);
            double result = CelsiusToKelvin(dewPoint) + (2671.02 / ((2954.61 / temperatureK) + (2.193665 * Math.Log(temperatureK)) - 13.3448)) - temperatureK;
            return Math.Round(KelvinToCelsius(result), 2);
        }
        
        // Use only when the temperature is <=0
        private bool IsFreezing(IWeather device)
        {
            if (device.Temperature <= -48.0)
            {
                return true;
            }

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
