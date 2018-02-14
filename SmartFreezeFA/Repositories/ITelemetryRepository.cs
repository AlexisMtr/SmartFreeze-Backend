using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public interface ITelemetryRepository
    {
        Telemetry GetLatest(string deviceId);
    }
}