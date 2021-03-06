﻿using System;
using WeatherLibrary.Abstraction;

namespace WeatherLibrary.Algorithmes.Util
{
    internal class UtilTemperatureCurrent : IWeather
    {
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Date { get; set; }

        public UtilTemperatureCurrent(IWeather weather)
        {
            this.Temperature = weather.Temperature;
        }

        public UtilTemperatureCurrent(double temperature)
        {
            this.Temperature = temperature;
        }
    }
}
