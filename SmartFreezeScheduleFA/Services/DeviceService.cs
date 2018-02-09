using SmartFreezeScheduleFA.Repositories;

namespace SmartFreezeScheduleFA.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }
    }
}
