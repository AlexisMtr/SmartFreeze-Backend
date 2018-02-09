using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherLibrary.GoogleMapElevation.Internals;

namespace WeatherLibrary.Abstraction
{
    public interface IAltitudeClient
    {
        Task<List<GMEAltitude>> GetAltitude(double latitude, double longitude);
    }
}
