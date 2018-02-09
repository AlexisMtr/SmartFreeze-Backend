using System.Collections.Generic;
using Newtonsoft.Json;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.GoogleMapElevation.DTOs
{
    public class GMERoot
    {
        [JsonProperty("results")]
        public IEnumerable<GMEAltitude> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
