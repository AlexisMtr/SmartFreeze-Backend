using SmartFreezeFA.Models;
using System.Collections.Generic;

namespace SmartFreezeFA.Repositories
{
    public interface ITelemetryRepository
    {
        Telemetry GetLatest(string deviceId);
        void InsertTelemetries(IEnumerable<Telemetry> telemetries);
    }
}