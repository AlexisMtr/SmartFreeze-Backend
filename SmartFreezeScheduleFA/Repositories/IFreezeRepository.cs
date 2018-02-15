using SmartFreezeScheduleFA.Models;
using System;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IFreezeRepository
    {
        void AddOrUpdateFreeze(string deviceId, DateTime date, int trustIndication);
        void AddFreeze(IEnumerable<Freeze> freezeList);
        Freeze GetLastFreezeByDevice(string deviceId);
    }
}