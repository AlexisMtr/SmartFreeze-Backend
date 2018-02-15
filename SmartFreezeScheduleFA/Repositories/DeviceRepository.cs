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
using MongoDB.Bson.Serialization;

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
            if (maxBoundaryMin.HasValue)
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

        //TODO test
        public IList<Alarm> GetCrossAlarmsByDevice(string deviceId, DateTime start, DateTime end)
        {
            IEnumerable<BsonDocument> pipelineDef = GetPipelineDefinition(deviceId, start, end);
            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipelineDef);

            return BsonIterator.Iterate(collection, pipelineDefinition, (BsonDocument e, IList<Alarm> items) =>
            {
                if (items == null) items = new List<Alarm>();
                
                items.Add(BsonSerializer.Deserialize<SingleBsonItem<Alarm>>(e).Item);

                return items;
            });
        }

        private IEnumerable<BsonDocument> GetPipelineDefinition(string deviceId, DateTime start, DateTime end)
        {
            BsonDocument unwindDeviceStage = new BsonDocument("$unwind", "$Devices");
            BsonDocument projectDeviceStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id" , 0 },
                { "Devices", "$Devices" }
            });
            BsonDocument matchDeviceStage = new BsonDocument("$match", new BsonDocument
            {
                { "Devices.Id", deviceId }
            });
            BsonDocument unwindAlarmStage = new BsonDocument("$unwind", "$Devices.Alarms");
            BsonDocument projectAlarmsStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id" , 0 },
                { "Item", "$Devices.Alarms" }
            });

            BsonDocument alarmDateBetweenStage = new BsonDocument
            {
                { "Item.End", new BsonDocument("$gt", start) },
                { "Item.Start", new BsonDocument("$lte", start) }
            };
            BsonDocument startBetweenStage = new BsonDocument
            {
                { "Item.Start", new BsonDocument
                    {
                        { "$gte", start },
                        { "$lt", end }
                    }
                }
            };

            BsonDocument orStage = new BsonDocument("$match",
                new BsonDocument("$or", new BsonArray(new List<BsonDocument> { alarmDateBetweenStage, startBetweenStage })));

            BsonDocument matchTypeStage = new BsonDocument("$match", new BsonDocument("Item.AlarmType", Alarm.Type.FreezeWarning));

            return new List<BsonDocument> { unwindDeviceStage, projectDeviceStage, matchDeviceStage, unwindAlarmStage, projectAlarmsStage, orStage, matchTypeStage };
        }

        //TODO test
        public void UpdateAlarm(string deviceId, string alarmId, DateTime start, DateTime end)
        {
            //var deviceFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            //var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Alarms.Id", alarmId);
            //var filter = Builders<Site>.Filter.And(deviceFilter, deviceSiteFilter);
            //UpdateDefinition<Site> update = Builders<Site>.Update.Set("Devices.$.Alarms.Start", start)
            //    .Set("Devices.$.Alarms.End", end);

            //collection.FindOneAndUpdate(filter, update);

            var filterAlarm = Builders<Device>.Filter.ElemMatch(e => e.Alarms, a => a.Id == alarmId);
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, filterAlarm);

            Site site = collection.Find(filter).ToList().FirstOrDefault();
            Device devices = site.Devices.FirstOrDefault(e => e.Alarms.Any(a => a.Id == alarmId));
            Alarm alarm = devices.Alarms.First(e => e.Id == alarmId);
            int index = (devices.Alarms as List<Alarm>).IndexOf(alarm);

            UpdateResult result = collection.UpdateOne(filter, Builders<Site>.Update
                .Set($"Devices.$.Alarms.{index}.Start", start)
                .Set($"Devices.$.Alarms.{index}.End", end), new UpdateOptions { IsUpsert = true });
        }

        //TODO todo test
        public void deleteAlarmById(string deviceId, string alarmId)
        {
            var deviceFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Alarms.Id", alarmId);
            var filter = Builders<Site>.Filter.And(deviceFilter, deviceSiteFilter);

            //var update = Builders<Site>.Update.PullFilter(e => e.Devices, a => a.alarmId == alarmId);
            //collection.FindOneAndUpdate(filter, update);
        }

        public bool UpdateStatusAlarm(string deviceId, Alarm alarm)
        {

            var filterAlarm = Builders<Device>.Filter.ElemMatch(e => e.Alarms, a => a.Id == alarm.Id);
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, filterAlarm);

            Site site = collection.Find(filter).ToList().FirstOrDefault();
            Device devices = site.Devices.FirstOrDefault(e => e.Alarms.Any(a => a.Id == alarm.Id));
            Alarm alarmBase = devices.Alarms.First(e => e.Id == alarm.Id);
            int index = (devices.Alarms as List<Alarm>).IndexOf(alarmBase);

            UpdateResult result = collection.UpdateOne(filter, Builders<Site>.Update.Set($"Devices.$.Alarms.{index}.IsActive", alarm.IsActive));

            return result.ModifiedCount == 1;
        }


        private class SingleBsonItem<T>
        {
            public T Item { get; set; }
        }
    }
}
