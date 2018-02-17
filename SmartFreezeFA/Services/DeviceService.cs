using SmartFreezeFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
