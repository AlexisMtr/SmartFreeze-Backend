using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Collections;
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

        public PaginatedItems<Alarm> GetBySite(string siteId, IMongoFilter<Site, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .Where(e => e.Id == siteId)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }

        public IEnumerable<Alarm> GetByDevice(string deviceId, IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            BsonDocument unwindDevices = new BsonDocument("$unwind", "$Devices");
            BsonDocument projectDevices = new BsonDocument("$project", new BsonDocument
            {
                { "Devices", "$Devices" },
                { "_id", 0 }
            });
            BsonDocument matchDevice = new BsonDocument("$match", new BsonDocument
            {
                { "Devices.Id", deviceId }
            });
            BsonDocument unwindAlarms = new BsonDocument("$unwind", "$Devices.Alarms");
            BsonDocument projectAlarms = new BsonDocument("$project", new BsonDocument
            {
                { "Alarms", "$Devices.Alarms" },
                { "_id", 0 }
            });

            IEnumerable<BsonDocument> pipeline = new List<BsonDocument> { unwindDevices, projectDevices, matchDevice, unwindAlarms, projectAlarms };
            PipelineDefinition<Site, BsonDocument> pipelineDefinition = PipelineDefinition<Site, BsonDocument>.Create(pipeline);
            var docCursor = collection.Aggregate(pipelineDefinition);

            IList<Alarm> alarms = new List<Alarm>();
            while (docCursor.MoveNext())
            {
                var doc = docCursor.Current;
                foreach (var item in doc)
                {
                    alarms.Add(JsonConvert.DeserializeObject<BsonAlarmRoot>(item.ToJson()).Alarm);
                }

            }

            return alarms;
        }

        private class BsonAlarmRoot
        {
            public Alarm Alarm { get; set; }
        }
    }
}
