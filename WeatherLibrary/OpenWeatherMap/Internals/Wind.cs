using Newtonsoft.Json;

namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
        [JsonProperty("deg")]
        public double Degree { get; set; }
    }
}
