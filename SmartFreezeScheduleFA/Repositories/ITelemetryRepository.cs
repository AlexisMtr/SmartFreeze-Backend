using SmartFreezeScheduleFA.Models;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface ITelemetryRepository
    {
        Dictionary<string, Telemetry> GetLastTelemetryByDevice();
    }
}