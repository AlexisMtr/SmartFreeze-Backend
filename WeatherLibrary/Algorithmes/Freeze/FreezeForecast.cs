using System;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezeForecast
    {
        public DateTime? FreezingStart { get; set; }
        public DateTime? FreezingEnd { get; set; }

        public FreezeForecast()
        {
            this.FreezingStart = null;
            this.FreezingEnd = null;
        }
    }
}
