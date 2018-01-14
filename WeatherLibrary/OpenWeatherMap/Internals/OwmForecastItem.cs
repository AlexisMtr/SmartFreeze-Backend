using Newtonsoft.Json;
using System;

namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class OwmForecastItem
    {
        [JsonProperty("dt")]
        public long Timestamp { get; set; }
        [JsonProperty("main")]
        public Weather Weather { get; set; }
        public Wind Wind { get; set; }
        [JsonProperty("dt_txt")]
        public DateTime ForecastDate { get; set; }
    }
}
