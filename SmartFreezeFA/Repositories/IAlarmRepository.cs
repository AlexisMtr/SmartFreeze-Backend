using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public interface IAlarmRepository
    {
        Alarm GetLatestAlarmByType(string deviceId, Alarm.Type type, Alarm.Gravity? gravity);
        void SetAsInactive(string deviceId, Alarm.AlarmSubtype subtype);
        void SetAsInactive(string alarmId);
    }
}