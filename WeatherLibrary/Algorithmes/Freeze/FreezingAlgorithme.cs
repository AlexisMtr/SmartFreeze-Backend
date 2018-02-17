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
            FreezeForecast freezeForecast = new FreezeForecast
            {
                FreezingStart = forecast.OrderBy(e => e.Date).First().Date,
                FreezingEnd = forecast.OrderBy(e => e.Date).Last().Date
            };
            double deviceTemperature = device.Temperature;
            double estimateDeviceTemperature = await EstimateWeatherByAltitudeDiff(deviceTemperature, forecastStation, devicePosition);
            if (estimateDeviceTemperature == 0.0)
            {
                estimateDeviceTemperature = 0.0000001;
            }
            if (currentWeather.Humidity == 0.0)
            {
                currentWeather.Humidity = 0.0000001;
            }
            double coefTemperature = Math.Abs(device.Temperature / estimateDeviceTemperature);
            double coefHumidity = Math.Abs(device.Humidity / currentWeather.Humidity);

            IWeather theoricWeather = device;
            theoricWeather.Temperature = coefTemperature * estimateDeviceTemperature;
            theoricWeather.Humidity = coefHumidity * currentWeather.Humidity;

            freezeForecast.FreezingProbabilityList[currentWeather.Date] = GetProbabilityFreezing(theoricWeather);

            IEnumerable<IWeather> forecastEstimation = await EstimateWeatherByAltitudeDiffForecast(forecast, forecastStation, devicePosition);
            int i = 0;
            foreach (IWeather forecastItem in forecast)
            {

                IWeather estimationWeather = forecastEstimation.ElementAt(i);
                i++;
                theoricWeather.Temperature = coefTemperature + estimationWeather.Temperature;
                theoricWeather.Humidity = coefHumidity + forecastItem.Humidity;

                freezeForecast.FreezingProbabilityList[forecastItem.Date] = GetProbabilityFreezing(theoricWeather);

            }
            freezeForecast.FreezingStart = forecast.OrderBy(e => e.Date).First().Date;
            freezeForecast.FreezingEnd = forecast.OrderBy(e => e.Date).Last().Date;
            return freezeForecast;
        }

        private async Task<double> EstimateWeatherByAltitudeDiff(double deviceTemperature, IStationPosition forecastStation, IStationPosition expectedStation)
        {
            forecastStation.Altitude = (await client.GetAltitude(forecastStation.Latitude, forecastStation.Longitude)).Altitude;

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
            forecastStation.Altitude = (await client.GetAltitude(forecastStation.Latitude, forecastStation.Longitude)).Altitude;

            IEnumerable<IWeather> theoricWeatherList = new List<IWeather>();

            double elevationBetweenWeatherStationAndSite = forecastStation.Altitude - expectedStation.Altitude;

            // Only if altitude diff is greater than 100
            if (Math.Abs(elevationBetweenWeatherStationAndSite) >= 100.0)
            {
                foreach (IWeather f in forecast)
                {
                    IWeather estimateWeather = f;
                    estimateWeather.Temperature = ConvertTemperature(f.Temperature, elevationBetweenWeatherStationAndSite);
                    (theoricWeatherList as List<IWeather>).Add(estimateWeather);
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
            double result = CelsiusToKelvin(dewPoint) + (2671.02 / ((2954.61 / temperatureK) + (2.193665 * Math.Log(temperatureK)) - 13.3448)) - temperatureK;
            return Math.Round(KelvinToCelsius(result), 2);
        }

        // Use only when the temperature is <=0
        private bool IsFreezing(IWeather device)
        {

            //The dew temperature is greater than the freezing temperature
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

        private FreezeForecast.FreezingProbability GetProbabilityFreezing(IWeather theoricWeather)
        {
            if (IsFreezing(theoricWeather))
            {
                return FreezeForecast.FreezingProbability.IMMINENT;
            }
            else if ((theoricWeather.Temperature <= 1.0) && (theoricWeather.Humidity >= 80.0))
            {
                return FreezeForecast.FreezingProbability.HIGH;
            }
            else if ((theoricWeather.Temperature <= 1.0) && (theoricWeather.Humidity < 80.0))
            {
                return FreezeForecast.FreezingProbability.MEDIUM;
            }
            else if (((theoricWeather.Temperature <= 5.0) && (theoricWeather.Temperature > 1
                )))
            {
                return FreezeForecast.FreezingProbability.MINIMUM;
            }
            else
            {
                return FreezeForecast.FreezingProbability.ZERO;
            }
        }
    }
}
