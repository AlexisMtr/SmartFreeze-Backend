using System;
using System.Collections.Generic;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezeForecast
    {
        public List<FreezingProbability> FreezingProbabilityList { get; set; }
        public DateTime? FreezingStart { get; set; }
        public DateTime? FreezingEnd { get; set; }

        public FreezeForecast()
        {
            FreezingProbabilityList = new List<FreezingProbability>();
            this.FreezingStart = null;
            this.FreezingEnd = null;
        }

        public enum FreezingProbability
        {
            ZERO, // Not freezing
            MINIMUM,
            MEDIUM,
            HIGH,
            IMMINENT, // freezing !
        }
    }
}
