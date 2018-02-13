using SmartFreezeScheduleFA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFreezeScheduleFA.Services
{
    class FreezeService
    {
        private readonly IDeviceRepository deviceRepository;

        public FreezeService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public void CreateFreezeAndThawByDevice(string deviceId, object dicoPredictionBy12h)
        {
            foreach (var prediction in dicoPredictionBy12h)
            {

            }
        }

        public createFreeze(string deviceId, trustIndication)
        {

            deviceRepository.AddFreeze(deviceId, date, TrustIndication);
        }
    }
}
