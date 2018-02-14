using SmartFreezeScheduleFA.Models;
using System;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IFreezeRepository
    {
        void AddFreeze(string deviceId, DateTime date, int TrustIndication);
        void AddFreeze(IEnumerable<Freeze> freezeList);
        Freeze getLastFreezeByDevice(string deviceId);
    }
}