using Newtonsoft.Json;

namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class Geocoordinate
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
