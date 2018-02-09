using Newtonsoft.Json;

namespace WeatherLibrary.GoogleMapElevation.Internals
{
    public class GMELocation
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}