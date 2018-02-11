using SmartFreezeScheduleFA.Models;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface ITelemetryRepository
    {
        Telemetry GetLastTelemetry();
    }
}