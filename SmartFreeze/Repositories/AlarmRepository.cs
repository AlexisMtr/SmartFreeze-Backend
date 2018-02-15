using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SmartFreeze.Context;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class AlarmRepository : IAlarmRepository 
    {
        private readonly IMongoCollection<Site> collection;

        public AlarmRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }
        
        public IEnumerable<Alarm> Get(DeviceAlarmFilter filter, int rowsPerPage, int pageNumber)
        {
            filter.DeviceId = string.Empty;
            IEnumerable<BsonDocument> pipeline = filter.SkipedAlarmsPipeline(rowsPerPage, pageNumber);

            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            return Iterate<IList<Alarm>>(pipelineDefinition, (e, alarms) =>
            {
                if (alarms == null) alarms = new List<Alarm>();
                alarms.Add(JsonConvert.DeserializeObject<BsonAlarmRoot>(e).Alarms);
                return alarms;
            });
        }

        public int Count(DeviceAlarmFilter filter)
        {
            filter.DeviceId = string.Empty;
            IEnumerable<BsonDocument> pipeline = filter.CountAlarmsPipeline();
            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            return Iterate<int>(pipelineDefinition, (e, count) => JsonConvert.DeserializeObject<BsonAlarmRoot>(e).Count);
        }

        public IEnumerable<Alarm> GetByDevice(string deviceId, DeviceAlarmFilter filter, int rowsPerPage, int pageNumber)
        {
            IEnumerable<BsonDocument> pipeline = filter.SkipedAlarmsPipeline(rowsPerPage, pageNumber);

            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            return Iterate<IList<Alarm>>(pipelineDefinition, (e, alarms) =>
            {
                if (alarms == null) alarms = new List<Alarm>();
                alarms.Add(JsonConvert.DeserializeObject<BsonAlarmRoot>(e).Alarms);
                return alarms;
            });
        }

        public int CountByDevice(string deviceId, DeviceAlarmFilter filter)
        {
            IEnumerable<BsonDocument> pipeline = filter.CountAlarmsPipeline();
            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            return Iterate<int>(pipelineDefinition, (e, count) => JsonConvert.DeserializeObject<BsonAlarmRoot>(e).Count);
        }

        private T Iterate<T>(PipelineDefinition<Site, BsonDocument> pipeline, Func<string, T, T> callback)
        {
            var docCursor = collection.Aggregate(pipeline);

            T value = default(T);
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    value = callback.Invoke(item.ToJson(), value);
                }

            }

            return value;
        }

        public bool SetAlarmToRead(string alarmId)
        {
            var filterAlarm = Builders<Device>.Filter.ElemMatch(e => e.Alarms, a => a.Id == alarmId);
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, filterAlarm);

            Site site = collection.Find(filter).ToList().FirstOrDefault();
            Device devices = site.Devices.FirstOrDefault(e => e.Alarms.Any(a => a.Id == alarmId));
            Alarm alarm = devices.Alarms.First(e => e.Id == alarmId);
            int index = (devices.Alarms as List<Alarm>).IndexOf(alarm);

            UpdateResult result = collection.UpdateOne(filter, Builders<Site>.Update.Set($"Devices.$.Alarms.{index}.IsRead", true));

            return result.ModifiedCount == 1;
        }

        private class BsonAlarmRoot
        {
            public Alarm Alarms { get; set; }
            public int Count { get; set; }
        }
    }
}
