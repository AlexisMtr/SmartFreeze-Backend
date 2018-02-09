using Newtonsoft.Json;

namespace WeatherLibrary.GoogleMapElevation.Internals
{
    public class GMEAltitude
    {
        [JsonProperty("elevation")]
        public double Elevation { get; set; }
        [JsonProperty("location")]
        public GMELocation Location { get; set; }
        [JsonProperty("resolution")]
        public double Resolution { get; set; }

    }
}
