﻿using System.Collections.Generic;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.Algorithmes.Util
{
    internal class UtilTemperatureForecast
    {
        public List<double> TemperatureList { get; set; }

        public UtilTemperatureForecast(IEnumerable<IWeather> weatherList)
        {
            TemperatureList = new List<double>();
            (weatherList as List<IWeather>).ForEach(w => this.TemperatureList.Add(w.Temperature));
        }

        public UtilTemperatureForecast(IEnumerable<double> temperatureList)
        {
            TemperatureList = new List<double>();
            this.TemperatureList = temperatureList as List<double>;
        }
    }
}
