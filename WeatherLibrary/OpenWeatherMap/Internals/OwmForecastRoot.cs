using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class OwmForecastRoot
    {
        [JsonProperty("cod")]
        public int Code { get; set; }
        [JsonProperty("cnt")]
        public int Count { get; set; }
        public City City { get; set; }
        [JsonProperty("list")]
        public IEnumerable<OwmForecastItem> Forecast { get; set; }
    }
}
