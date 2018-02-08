﻿using MongoDB.Driver;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public class AlarmRepository : IAlarmRepository
    {
        private readonly IMongoCollection<Site> collection;

        public AlarmRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public Alarm AddAlarmToSite(string siteId, Alarm alarm)
        {
            UpdateDefinition<Site> update = Builders<Site>.Update.Push(e => e.Alarms, alarm);
            var result = collection.UpdateOne(Builders<Site>.Filter.Eq(e => e.Id, siteId), update);
            return alarm;
        }

        public Alarm AddAlarmToDevice(string deviceId, Alarm alarm)
        {
            // TODO : Add alarm to the right collection
            return alarm;
        }

    }
}