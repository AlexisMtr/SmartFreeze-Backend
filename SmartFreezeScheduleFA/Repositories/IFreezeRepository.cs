using System;

namespace SmartFreezeScheduleFA.Repositories
{
    public interface IFreezeRepository
    {
        void AddFreeze(string deviceId, DateTime date, int TrustIndication);
    }
}