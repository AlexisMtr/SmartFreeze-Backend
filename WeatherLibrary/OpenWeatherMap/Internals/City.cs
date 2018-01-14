namespace WeatherLibrary.OpenWeatherMap.Internals
{
    internal class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public Geocoordinate Coord { get; set; }
    }
}
