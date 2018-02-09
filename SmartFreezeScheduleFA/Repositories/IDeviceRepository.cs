using System.Collections.Generic;
using SmartFreezeScheduleFA.Models;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IDeviceRepository
    {
        void AddAlarm(string deviceId, Alarm alarm);
        Device Get(string deviceId);
        IEnumerable<Device> GetFailsCommunicationBetween(int minBundaryMin, int? maxBoundaryMin = null);
    }
}