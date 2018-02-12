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

        public enum FreezingProbability : int
        {
            ZERO = 0, // between 0 and 29
            MINIMUM = 30, // between 30 and 49
            MEDIUM = 50, // between 50 and 75
            HIGH = 75, // between 75 and 99
            IMMINENT = 100, // freezing !
        }
    }
}
