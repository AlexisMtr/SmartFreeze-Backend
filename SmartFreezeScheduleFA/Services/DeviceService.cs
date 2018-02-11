using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Device> CheckDeviceCommunication(int minBoundaryMin, int? maxBoundaryMin = null)
        {
            return deviceRepository.GetFailsCommunicationBetween(minBoundaryMin, maxBoundaryMin);
        }

        public Dictionary<Device, Telemetry> GetLatestTelemetryByDevice()
        {
            var telemetries = telemetryRepository.GetLastTelemetryByDevice();
            var devices = deviceRepository.Get(telemetries.Keys);
            System.Diagnostics.Debug.WriteLine(devices);
            Dictionary<Device, Telemetry> telemetryByDevice = new Dictionary<Device, Telemetry>();
            foreach(var item in devices)
            {
                telemetryByDevice.Add(item, telemetries.FirstOrDefault(e => e.Key == item.Id).Value);
            }

            return telemetryByDevice;
        }
    }
}
