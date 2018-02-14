using MongoDB.Driver;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Models;
using System;
using System.Collections.Generic;

namespace SmartFreezeScheduleFA.Repositories
{
    class FreezeRepository : IFreezeRepository
    {
        private readonly IMongoCollection<Freeze> collection;

        public FreezeRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Freeze>(nameof(Freeze));
        }

        public void AddFreeze(string deviceId, DateTime date, int TrustIndication)
        {
            collection.InsertOne(new Freeze()
            {
                DeviceId = deviceId,
                Date = date,
                TrustIndication = TrustIndication
            });
        }

        public void AddFreeze(IEnumerable<Freeze> freezeList)
        {
            collection.InsertMany(freezeList);
        }
    }
}
