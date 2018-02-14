using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SmartFreeze.Context;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;

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

        private class BsonAlarmRoot
        {
            public Alarm Alarms { get; set; }
            public int Count { get; set; }
        }
    }
}
