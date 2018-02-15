using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Collections.Generic;


namespace SmartFreeze.Repositories
{
    public interface IAlarmRepository
    {
        IEnumerable<Alarm> Get(DeviceAlarmFilter filter, int rowsPerPage, int pageNumber);
        IEnumerable<Alarm> GetByDevice(string deviceId, DeviceAlarmFilter filter, int rowsPerPage, int pageNumber);
        int Count(DeviceAlarmFilter filter);
        int CountByDevice(string deviceId, DeviceAlarmFilter filter);
        bool SetAlarmToRead(string alarmId);
    }
}