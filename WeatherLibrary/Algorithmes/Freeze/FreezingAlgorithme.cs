using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherLibrary.Abstraction;
using WeatherLibrary.Algorithmes.Util;
using WeatherLibrary.GoogleMapElevation;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezingAlgorithme : IAlgorithme<FreezeForecast>
    {
        private readonly GoogleMapElevationClient client = new GoogleMapElevationClient();

        public async Task<FreezeForecast> Execute(IWeather device, IStationPosition stationPosition)
        {
            FreezeForecast freezeForecast = new FreezeForecast();
            if (isFreezing(device))
            {
                freezeForecast.FreezingStart = DateTime.Now;
                return freezeForecast;
            }

            return freezeForecast;

        }
        public async Task<FreezeForecast> Execute(IWeather device, IStationPosition devicePosition, IWeather currentWeather, IEnumerable<IWeather> forecast, IStationPosition forecastStation)
        {

            FreezeForecast freezeForecast = new FreezeForecast();
            //UtilTemperature temperature = new UtilTemperature(client,forecastStation,forecast);
            double coefTemperature = device.Temperature / currentWeather.Temperature;
            double coefHumidity = device.Humidity / currentWeather.Humidity;

            if (isFreezing(device))
            {
                freezeForecast.FreezingStart = currentWeather.Date;
            }
            IWeather deviceTheoric = device;

            foreach (IWeather current in forecast)
            {
                deviceTheoric.Temperature = coefTemperature * current.Temperature; // temperatureCurrent à la place de current.Temperature
                deviceTheoric.Humidity = coefHumidity * current.Humidity;
                //UtilTemperatureCurrent temperatureCurrent =  await temperature.GetCurrentWeather(sitePosition.Latitude, sitePosition.Longitude);
                if (isFreezing(deviceTheoric) && (!freezeForecast.FreezingStart.HasValue))
                {
                    freezeForecast.FreezingStart = current.Date;
                }
                else if (!isFreezing(deviceTheoric) && (!freezeForecast.FreezingEnd.HasValue))
                {
                    freezeForecast.FreezingEnd = current.Date;
                }

            }
            if (!freezeForecast.FreezingEnd.HasValue && freezeForecast.FreezingStart.HasValue)
            {
                freezeForecast.FreezingEnd = (forecast as List<IWeather>)[(forecast as List<IWeather>).Count - 1].Date;
            }
            return freezeForecast;

        }


        // temperature in Celsius
        // return the dew point temperature in CELSIUS
        public double DewPoint(double humidity, double temperature)
        {
            double result = Math.Pow(humidity / 100.0, 1.0 / 8.0) * (112.0 + (0.9 * temperature)) + (0.1 * temperature) - 112.0;
            return Math.Round(result, 2);
        }
        //temperature in Celsius
        // return the freezing point temperature in CELSIUS
        public double FreezingPoint(double dewPoint, double temperature)
        {
            double temperatureK = CelsiusToKelvin(temperature);
            double result = CelsiusToKelvin(dewPoint) + (2671.02 / ((2954.61 / temperatureK) + (2.193665 * Math.Log(temperatureK)) - 13.3448)) - temperatureK;
            return Math.Round(KelvinToCelsius(result), 2);
        }

        // Use only when the temperature > 0
        public bool IsDewing(IWeather device)
        {
            double dewTemperature = DewPoint(device.Humidity, device.Temperature);
            if (device.Temperature <= dewTemperature)
            {
                return true;
            }
            return false;
        }

        // Use only when the temperature is <=0
        public bool isFreezing(IWeather device)
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
