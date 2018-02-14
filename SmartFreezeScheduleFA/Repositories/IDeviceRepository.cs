using System;
using System.Collections.Generic;
using SmartFreezeScheduleFA.Models;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IDeviceRepository
    {
        void AddAlarm(string deviceId, Alarm alarm);
        Device Get(string deviceId);
        IEnumerable<Device> Get(IEnumerable<string> ids);
        IEnumerable<Device> GetFailsCommunicationBetween(int minBundaryMin, int? maxBoundaryMin = null);
        IEnumerable<AlarmNotification> GetNotificationDetails(IEnumerable<string> devicesIds);
        IList<Alarm> GetCrossAlarmsByDevice(string deviceId, DateTime start, DateTime end);
        void UpdateAlarm(string deviceId, string alarmId, DateTime start, DateTime end);
        void deleteAlarmById(string deviceId, string alarmId);
    }
}