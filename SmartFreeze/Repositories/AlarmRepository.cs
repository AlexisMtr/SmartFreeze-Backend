using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class AlarmRepository
    {
        private readonly IMongoCollection<Site> collection;

        public AlarmRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }
        
        public IEnumerable<Alarm> GetByDevice(string deviceId, DeviceAlarmFilter filter, int rowsPerPage, int pageNumber)
        {
            IEnumerable<BsonDocument> pipeline = filter.SkipedAlarmsPipeline(rowsPerPage, pageNumber);

            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            var docCursor = collection.Aggregate(pipelineDefinition);

            IList<Alarm> alarms = new List<Alarm>();
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    alarms.Add(JsonConvert.DeserializeObject<BsonAlarmRoot>(item.ToJson()).Alarms);
                }

            }

            return alarms;
        }

        public int CountByDevice(string deviceId, DeviceAlarmFilter filter)
        {
            IEnumerable<BsonDocument> pipeline = filter.CountAlarmsPipeline();

            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            var docCursor = collection.Aggregate(pipelineDefinition);

            int count = 0;
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    count = JsonConvert.DeserializeObject<BsonAlarmRoot>(item.ToJson()).Count;
                }

            }
            return count;
        }

        private class BsonAlarmRoot
        {
            public Alarm Alarms { get; set; }
            public int Count { get; set; }
        }
    }
}
