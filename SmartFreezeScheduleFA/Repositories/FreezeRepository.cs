using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public void AddOrUpdateFreeze(string deviceId, DateTime date, int trustIndication)
        {
            FilterDefinition<Freeze> queryFilterDate = Builders<Freeze>.Filter.Eq(e => e.Date, date);
            FilterDefinition<Freeze> queryFilterDevice = Builders<Freeze>.Filter.Eq(e => e.DeviceId, deviceId);
            UpdateDefinition<Freeze> update = Builders<Freeze>.Update
                .Set(e => e.Date, date)
                .Set(e => e.DeviceId, deviceId)
                .Set(e => e.TrustIndication, trustIndication);

            collection.UpdateOne(Builders<Freeze>.Filter.And(queryFilterDate, queryFilterDevice), update, new UpdateOptions { IsUpsert = true });
        }

        public void AddFreeze(IEnumerable<Freeze> freezeList)
        {
            collection.InsertMany(freezeList);
        }

        public Freeze GetLastFreezeByDevice(string deviceId)
        {
            return collection.AsQueryable()
                .Where(e => e.DeviceId == deviceId && e.Date < DateTime.UtcNow)
                .OrderByDescending(e => e.Date)
                .Take(1)
                .FirstOrDefault();
        }
    }
}
