using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Helpers;
using SmartFreezeFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Alarm GetLatestAlarmByType(string deviceId, Alarm.Type type, Alarm.Gravity? gravity)
        {
            IEnumerable<BsonDocument> pipeline = GetPipeline(deviceId, type, gravity);
            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);

            return BsonIterator.Iterate(collection, pipelineDefinition, (BsonDocument e, Alarm item) => BsonSerializer.Deserialize<SingleBsonItem<Alarm>>(e).Item);
        }

        private IEnumerable<BsonDocument> GetPipeline(string deviceId, Alarm.Type type, Alarm.Gravity? gravity)
        {
            IList<BsonDocument> pipeline = new List<BsonDocument>();

            BsonDocument unwindDeviceStage = new BsonDocument("$unwind", "$Devices");
            pipeline.Add(unwindDeviceStage);

            BsonDocument projectDevicesStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "Device", "$Devices" }
            });
            pipeline.Add(projectDevicesStage);

            BsonDocument matchDeviceStage = new BsonDocument("$match", new BsonDocument("Device.Id", deviceId));
            pipeline.Add(matchDeviceStage);

            BsonDocument unwindAlarmsStage = new BsonDocument("$unwind", "$Device.Alarms");
            pipeline.Add(unwindAlarmsStage);

            BsonDocument projectAlarmsStage = new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "Item", "$Device.Alarms" }
            });
            pipeline.Add(projectAlarmsStage);

            BsonDocument matchAlarmStage = new BsonDocument("$match", new BsonDocument
            {
                { "Item.IsActive", true },
                { "Item.AlarmType", type }
            });
            pipeline.Add(matchAlarmStage);

            if(gravity.HasValue)
            {
                BsonDocument matchGravityStage = new BsonDocument("$match", new BsonDocument
                {
                    { "Item.AlarmGravity", gravity.Value}
                });
                pipeline.Add(matchGravityStage);
            }

            return pipeline;
        }

        public void SetAsInactive(string alarmId)
        {
            var filterAlarm = Builders<Device>.Filter.ElemMatch(e => e.Alarms, a => a.Id == alarmId);
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, filterAlarm);

            Site site = collection.Find(filter).ToList().FirstOrDefault();
            Device devices = site.Devices.FirstOrDefault(e => e.Alarms.Any(a => a.Id == alarmId));
            Alarm alarm = devices.Alarms.First(e => e.Id == alarmId);
            int index = (devices.Alarms as List<Alarm>).IndexOf(alarm);
            
            collection.UpdateOne(filter, Builders<Site>.Update
                .Set($"Devices.$.Alarms.{index}.IsActive", false));
        }

        public void SetAsInactive(string deviceId, Alarm.AlarmSubtype subtype)
        {
            Device device = collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(e => e.Id == deviceId)
                .FirstOrDefault();
            IEnumerable<Alarm> alarms = device.Alarms.Where(e => e.IsActive && e.Subtype == subtype);

            System.Diagnostics.Debug.WriteLine(collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(e => e.Id == deviceId));

            foreach(Alarm alarm in alarms)
            {
                var filterAlarm = Builders<Device>.Filter.ElemMatch(e => e.Alarms, a => a.Id == alarm.Id);
                var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, filterAlarm);
                
                int index = (device.Alarms as List<Alarm>).IndexOf(alarm);

                collection.UpdateOne(filter, Builders<Site>.Update
                    .Set($"Devices.$.Alarms.{index}.IsActive", false));
            }
        }

        private class SingleBsonItem<T>
        {
            public T Item { get; set; }
        }
    }
}
