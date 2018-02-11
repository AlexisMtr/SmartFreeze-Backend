using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherLibrary.GoogleMapElevation.Internals
{
    public class GMERoot
    {
        [JsonProperty("results")]
        public IEnumerable<GMEAltitude> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
