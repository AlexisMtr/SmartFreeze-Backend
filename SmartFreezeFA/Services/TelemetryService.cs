using SmartFreezeFA.Models;
using SmartFreezeFA.Repositories;
using System.Collections.Generic;

namespace SmartFreezeFA.Services
{
    public class TelemetryService
    {
        private readonly ITelemetryRepository telemetryRepository;

        public TelemetryService(ITelemetryRepository telemetryRepository)
        {
            this.telemetryRepository = telemetryRepository;
        }

        public void InsertTelemetries(IEnumerable<Telemetry> telemetries)
        {
            telemetryRepository.InsertTelemetries(telemetries);
        }
    }
}
