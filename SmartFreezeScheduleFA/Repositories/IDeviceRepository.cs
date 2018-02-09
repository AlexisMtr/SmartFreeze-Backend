using SmartFreezeScheduleFA.Models;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IDeviceRepository
    {
        void AddAlarm(string deviceId, Alarm alarm);
    }
}