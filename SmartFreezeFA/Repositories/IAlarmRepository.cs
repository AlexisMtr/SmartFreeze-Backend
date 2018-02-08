using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public interface IAlarmRepository
    {
        Alarm AddAlarmToDevice(string deviceId, Alarm alarm);
        Alarm AddAlarmToSite(string siteId, Alarm alarm);
    }
}