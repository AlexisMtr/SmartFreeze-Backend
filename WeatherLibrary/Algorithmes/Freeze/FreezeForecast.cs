using System;
using System.Collections.Generic;

namespace WeatherLibrary.Algorithmes.Freeze
{
    public class FreezeForecast
    {
        public Dictionary<DateTime, FreezingProbability> FreezingProbabilityList { get; set; }
        public DateTime? FreezingStart { get; set; }
        public DateTime? FreezingEnd { get; set; }

        public FreezeForecast()
        {
            FreezingProbabilityList = new Dictionary<DateTime, FreezingProbability>();
            this.FreezingStart = null;
            this.FreezingEnd = null;
        }

        public enum FreezingProbability
        {
            ZERO = 0, // Not freezing
            MINIMUM = 1,
            MEDIUM = 2,
            HIGH = 3,
            IMMINENT = 4 // freezing !
        }
    }
}
