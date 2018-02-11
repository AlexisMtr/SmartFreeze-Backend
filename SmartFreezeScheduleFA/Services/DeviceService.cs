using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly ITelemetryRepository telemetryRepository;

        public DeviceService(IDeviceRepository deviceRepository, ITelemetryRepository telemetryRepository)
        {
            this.deviceRepository = deviceRepository;
            this.telemetryRepository = telemetryRepository;
        }

        public Dictionary<Device, Telemetry> GetLatestTelemetryByDevice()
        {
            return null;
        }
    }
}
