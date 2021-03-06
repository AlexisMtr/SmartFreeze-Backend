﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Exceptions;
using static WeatherLibrary.Algorithmes.Freeze.FreezeForecast;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezingAlgorithme : IAlgorithme<FreezeForecast>
    {
        private readonly IAltitudeClient client;
        private readonly ILogger logger;

        public FreezingAlgorithme(IAltitudeClient altitudeClient, ILogger logger)
        {
            this.client = altitudeClient;
            this.logger = logger;
        }

        /// <summary>
        /// Execute the freezing algorithme on a specific device
        /// </summary>
        /// <param name="device">Device telemetry with temperature in Celsius</param>
        /// <returns></returns>
        public async Task<FreezeForecast> Execute(IWeather device)
        {
            FreezeForecast freezeForecast = new FreezeForecast
            {
                FreezingStart = device.Date
            };

            freezeForecast.FreezingProbabilityList.Add(device.Date, GetProbabilityFreezing(device));
            return freezeForecast;
        }

        /// <summary>
        /// Execute the freezing algorithme depending on the forecast weather.
        /// It return a FreezeForcast object which contains 
        /// a dictionnary of DateTime,FreezingProbability
        /// There is four levels of probability :
        /// ZERO = 0% of freezing
        /// LOW = The probability of freeze is under 50%
        /// MEDIUM = The probability of freezing is between 50% and 80%
        /// HIGH = The probability of freezing is greater than 80%
        /// IMMINENT = The probability of freezing is 100% 
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
            if (!forecast.Any()) return null;
            try
            {
                FreezeForecast freezeForecast = new FreezeForecast();
                double deviceTemperature = device.Temperature;
                double estimateDeviceTemperature = await EstimateWeatherByAltitudeDiff(deviceTemperature, forecastStation, devicePosition);

                if (estimateDeviceTemperature == 0.0)
                {
                    estimateDeviceTemperature = 1;
                }
                if (currentWeather.Humidity == 0.0)
                {
                    // to force the coef to be 1
                    currentWeather.Humidity = device.Humidity;
                }
                double coefTemperature = Math.Abs(device.Temperature / estimateDeviceTemperature);
                double coefHumidity = Math.Abs(device.Humidity / currentWeather.Humidity);
                logger.Info($"coef temp. : {coefTemperature}");
                logger.Info($"coef hum. : {coefHumidity}");

                IWeather theoricWeather = Activator.CreateInstance(device.GetType()) as IWeather;
                theoricWeather.Temperature = coefTemperature * estimateDeviceTemperature;
                theoricWeather.Humidity = coefHumidity * currentWeather.Humidity;

                freezeForecast.FreezingProbabilityList[currentWeather.Date] = GetProbabilityFreezing(theoricWeather);

                IEnumerable<IWeather> forecastEstimation = await EstimateWeatherByAltitudeDiffForecast(forecast, forecastStation, devicePosition);

                int i = 0;
                IWeather forecastItem;
                IWeather estimationWeather;
                // while not out of range
                while (i < forecast.Count() || i < forecastEstimation.Count())
                {
                    forecastItem = forecast.ElementAt(i);
                    estimationWeather = forecastEstimation.ElementAt(i);

                    theoricWeather.Temperature = coefTemperature * estimationWeather.Temperature;
                    theoricWeather.Humidity = coefHumidity * forecastItem.Humidity;

                    freezeForecast.FreezingProbabilityList[forecastItem.Date] = GetProbabilityFreezing(theoricWeather);

                    i++;
                }

                StringBuilder logBuilder = new StringBuilder();
                logBuilder.Append($"Freeze probabibility {FreezingProbability.ZERO.ToString()} : {freezeForecast.FreezingProbabilityList.Count(e => e.Value == FreezingProbability.ZERO)}");
                logBuilder.Append(" / ");
                logBuilder.Append($"Freeze probabibility {FreezingProbability.MINIMUM.ToString()} : {freezeForecast.FreezingProbabilityList.Count(e => e.Value == FreezingProbability.MINIMUM)}");
                logBuilder.Append(" / ");
                logBuilder.Append($"Freeze probabibility {FreezingProbability.MEDIUM.ToString()} : {freezeForecast.FreezingProbabilityList.Count(e => e.Value == FreezingProbability.MEDIUM)}");
                logBuilder.Append(" / ");
                logBuilder.Append($"Freeze probabibility {FreezingProbability.HIGH.ToString()} : {freezeForecast.FreezingProbabilityList.Count(e => e.Value == FreezingProbability.HIGH)}");
                logBuilder.Append(" / ");
                logBuilder.Append($"Freeze probabibility {FreezingProbability.IMMINENT.ToString()} : {freezeForecast.FreezingProbabilityList.Count(e => e.Value == FreezingProbability.IMMINENT)}");

                logger.Info(logBuilder.ToString());

                freezeForecast.FreezingStart = forecast.OrderBy(e => e.Date).FirstOrDefault().Date;
                freezeForecast.FreezingEnd = forecast.OrderBy(e => e.Date).LastOrDefault().Date;

                return freezeForecast;
            }
            catch(Exception e)
            {
                logger.Error($"Error occured on executing algorithme", e);
                throw new AlgorithmeException("Error occured on executing algorithme", e);
            }
        }

        private async Task<double> EstimateWeatherByAltitudeDiff(double deviceTemperature, IStationPosition forecastStation, IStationPosition expectedStation)
        {
            logger.Info($"Altitude on station ({forecastStation.Latitude} ; {forecastStation.Longitude}) :");
            forecastStation.Altitude = (await client.GetAltitude(forecastStation.Latitude, forecastStation.Longitude)).Altitude;
            logger.Info($"{forecastStation.Altitude}");

            double elevationBetweenWeatherStationAndSite = forecastStation.Altitude - expectedStation.Altitude;
            double estimateTemperature = deviceTemperature;
            // Only if altitude diff is greater than 100
            if (Math.Abs(elevationBetweenWeatherStationAndSite) >= 100.0)
            {
                estimateTemperature = ConvertTemperature(estimateTemperature, elevationBetweenWeatherStationAndSite);
            }

            return estimateTemperature;
        }

        //Reduce the number of calls to Google Map Elevation API
        private async Task<IEnumerable<IWeather>> EstimateWeatherByAltitudeDiffForecast(IEnumerable<IWeather> forecast, IStationPosition forecastStation, IStationPosition expectedStation)
        {
            logger.Info($"Altitude on station ({forecastStation.Latitude} ; {forecastStation.Longitude}) :");
            forecastStation.Altitude = (await client.GetAltitude(forecastStation.Latitude, forecastStation.Longitude)).Altitude;
            logger.Info($"{forecastStation.Altitude}");

            IList<IWeather> theoricWeatherList = new List<IWeather>();

            double elevationBetweenWeatherStationAndSite = forecastStation.Altitude - expectedStation.Altitude;

            // Only if altitude diff is greater than 100
            if (Math.Abs(elevationBetweenWeatherStationAndSite) >= 100.0)
            {
                foreach (IWeather f in forecast)
                {
                    IWeather estimateWeather = f;
                    estimateWeather.Temperature = ConvertTemperature(f.Temperature, elevationBetweenWeatherStationAndSite);
                    theoricWeatherList.Add(estimateWeather);
                }
                return theoricWeatherList;
            }
            return forecast;
        }

        private double ConvertTemperature(double temperature, double altitudeDiff)
        {
            double temperatureAdiabatic = (altitudeDiff * (6.5)) / 1_000;
            double predictedTemperature = temperature + temperatureAdiabatic;
            return predictedTemperature;
        }

        private double DewPoint(double humidity, double temperature)
        {
            double result = Math.Pow(humidity / 100.0, 1.0 / 8.0) * (112.0 + (0.9 * temperature)) + (0.1 * temperature) - 112.0;
            return Math.Round(result, 2);
        }

        private double FreezingPoint(double dewPoint, double temperature)
        {
            double temperatureK = CelsiusToKelvin(temperature);
            double result = CelsiusToKelvin(dewPoint) + (2_671.02 / ((2_954.61 / temperatureK) + (2.193665 * Math.Log(temperatureK)) - 13.3448)) - temperatureK;
            return Math.Round(KelvinToCelsius(result), 2);
        }

        // Use only when the temperature is <=0
        private bool IsFreezing(IWeather device)
        {
            if (device.Temperature <= 0)
            {
                //The dew temperature is greater than the freezing temperature
                double dewTemperature = DewPoint(device.Humidity, device.Temperature);
                if ((device.Temperature <= dewTemperature) && (dewTemperature <= 0))
                {
                    return true;
                }

                double freezingTemperature = FreezingPoint(dewTemperature, device.Temperature);
                if (device.Temperature <= freezingTemperature)
                {
                    return true;
                }
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

        private FreezingProbability GetProbabilityFreezing(IWeather theoricWeather)
        {
            if (IsFreezing(theoricWeather))
            {
                return FreezingProbability.IMMINENT;
            }
            else if ((theoricWeather.Temperature <= 1.0) && (theoricWeather.Humidity >= 80.0))
            {
                return FreezingProbability.HIGH;
            }
            else if ((theoricWeather.Temperature <= 1.0) && (theoricWeather.Humidity < 80.0))
            {
                return FreezingProbability.MEDIUM;
            }
            else if (theoricWeather.Temperature <= 5.0 && theoricWeather.Temperature > 1)
            {
                return FreezingProbability.MINIMUM;
            }
            else
            {
                return FreezingProbability.ZERO;
            }
        }
    }
}
