using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public interface IDeviceRepository
    {
        void AddAlarm(string deviceId, Alarm alarm);
    }
}