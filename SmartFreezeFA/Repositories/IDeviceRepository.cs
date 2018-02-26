using SmartFreezeFA.Models;
using System;

namespace SmartFreezeFA.Repositories
{
    public interface IDeviceRepository
    {
        void AddAlarm(string deviceId, Alarm alarm);
        void UpdateLastCommunication(string deviceId, DateTime date);
        string GetSiteId(string deviceId);
    }
}