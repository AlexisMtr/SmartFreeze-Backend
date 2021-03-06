﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using System;

namespace SmartFreezeFA.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IMongoCollection<Site> collection;

        public DeviceRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public void AddAlarm(string deviceId, Alarm alarm)
        {
            var siteIdFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Id", deviceId);
            var filter = Builders<Site>.Filter.And(siteIdFilter, deviceSiteFilter);
            UpdateDefinition<Site> update = Builders<Site>.Update.Push("Devices.$.Alarms", alarm);

            collection.FindOneAndUpdate(filter, update);
        }

        public void UpdateLastCommunication(string deviceId, DateTime date)
        {
            var siteIdFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Id", deviceId);
            var filter = Builders<Site>.Filter.And(siteIdFilter, deviceSiteFilter);
            UpdateDefinition<Site> update = Builders<Site>.Update.Set("Devices.$.LastCommunication", date);
            collection.FindOneAndUpdate(filter, update);
        }

        public string GetSiteId(string deviceId)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(e => e.Id == deviceId)
                .FirstOrDefault()?.SiteId;
        }
    }
}
