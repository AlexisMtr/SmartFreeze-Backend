using SmartFreezeFA.Repositories;
using System;

namespace SmartFreezeFA.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public void UpdateLastCommunication(string deviceId, DateTime date)
        {
            deviceRepository.UpdateLastCommunication(deviceId, date);
        }

        public string GetSiteId(string deviceId)
        {
            return deviceRepository.GetSiteId(deviceId);
        }
    }
}
