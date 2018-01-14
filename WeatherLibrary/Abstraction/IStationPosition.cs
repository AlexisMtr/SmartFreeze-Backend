namespace WeatherLibrary.Abstraction
{
    public interface IStationPosition
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Altitude { get; set; }
    }
}
