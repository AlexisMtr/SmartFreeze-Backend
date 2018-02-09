using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Services
{
    public class CommunicationStateService
    {
        private readonly IDeviceRepository deviceRepository;

        public CommunicationStateService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public IEnumerable<Device> CheckDeviceCommunication (int minBoundaryMin, int? maxBoundaryMin = null)
        {
            return deviceRepository.GetFailsCommunicationBetween(minBoundaryMin, maxBoundaryMin);
            
        }
    }
}
