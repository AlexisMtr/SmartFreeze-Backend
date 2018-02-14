using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using SmartFreezeScheduleFA.Helpers;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace SmartFreezeScheduleFA.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IMongoCollection<Site> collection;

        public DeviceRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public Device Get(string deviceId)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .FirstOrDefault(e => e.Id == deviceId);
        }

        public IEnumerable<Device> GetFailsCommunicationBetween(int minBundaryMin, int? maxBoundaryMin = null)
        {
            DateTime maxDate = DateTime.Now.AddMinutes(-minBundaryMin);

            Expression<Func<Device, bool>> expression;
            if(maxBoundaryMin.HasValue)
            {
                DateTime minDate = DateTime.Now.AddMinutes(-maxBoundaryMin.Value);
                expression = e => e.LastCommunication < maxDate && e.LastCommunication > minDate;
            }
            else
            {
                expression = e => e.LastCommunication < maxDate;
            }

            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(expression);
        }

        public void AddAlarm(string deviceId, Alarm alarm)
        {
            var siteIdFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Id", deviceId);
            var filter = Builders<Site>.Filter.And(siteIdFilter, deviceSiteFilter);
            UpdateDefinition<Site> update = Builders<Site>.Update.Push("Devices.$.Alarms", alarm);

            collection.FindOneAndUpdate(filter, update);
        }

        public IEnumerable<Device> Get(IEnumerable<string> ids)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(e => ids.Contains(e.Id));
        }

        public IEnumerable<AlarmNotification> GetNotificationDetails(IEnumerable<string> devicesIds)
        {
            BsonDocument unwindStage = new BsonDocument("$unwind", "$Devices");
            BsonDocument matchStage = new BsonDocument("$match", new BsonDocument
            {
                { "Devices.Id", new BsonDocument("$in", new BsonArray(devicesIds)) }
            });
            BsonDocument projectStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "SiteId", "$Id" },
                { "SiteName", "$Name" },
                { "DeviceId", "$Devices.Id" },
                { "DeviceName", "$Devices.Name" }
            });

            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(new List<BsonDocument> { unwindStage, matchStage, projectStage });

            return BsonIterator.Iterate<Site, IList<AlarmNotification>>(collection, pipelineDefinition, (e, items) =>
            {
                if (items == null) items = new List<AlarmNotification>();

                items.Add(JsonConvert.DeserializeObject<AlarmNotification>(e));
                return items;
            });
        }
    }
}
