using Newtonsoft.Json;

namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class OwmCurrentRoot
    {
        public string Base { get; set; }
        public double Visibility { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public Wind Wind { get; set; }
        public Geocoordinate Coord { get; set; }
        [JsonProperty("dt")]
        public long Timestamp { get; set; }
        [JsonProperty("main")]
        public Weather Weather { get; set; }
        [JsonProperty("cod")]
        public int Code { get; set; }
    }
}
