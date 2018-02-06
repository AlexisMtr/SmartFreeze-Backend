using SmartFreezeScheduleFA.Models;
using SmartFreezeScheduleFA.Repositories;
using System;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Services
{
    public class CommunicationStateService
    {
        private readonly DeviceRepository deviceRepository;

        // vérification des dernières communications
        public void CheckDeviceCommunication (int minBoundaryMin, int? maxBoundaryMin = null)
        {
            IEnumerable<Device> failDevices = deviceRepository.GetFailsCommunicationBetween(minBoundaryMin, maxBoundaryMin);
        }

        public void GenerateAlarm()
        {

        }
    }
}
