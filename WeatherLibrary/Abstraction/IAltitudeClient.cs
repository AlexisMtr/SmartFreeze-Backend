using System.Threading.Tasks;

namespace WeatherLibrary.Abstraction
{
    public interface IAltitudeClient
    {
        Task<IStationPosition> GetAltitude(double latitude, double longitude);
    }
}
