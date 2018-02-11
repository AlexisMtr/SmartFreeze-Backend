using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Helpers;
using SmartFreezeScheduleFA.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreezeScheduleFA.Repositories
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Telemetry>(nameof(Telemetry));
        }

        public Dictionary<string, Telemetry> GetLastTelemetryByDevice()
        {
            BsonDocument sortStage = new BsonDocument("$sort", new BsonDocument
            {
                { "OccuredAt", 1 }
            });
            BsonDocument groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$DeviceId" },
                { "Accumulator", new BsonDocument
                    {
                        { "$last", "$$ROOT" }
                    }
                }
            });

            PipelineDefinition<Telemetry, BsonDocument> pipelineDefinition = PipelineDefinition<Telemetry, BsonDocument>.Create(new List<BsonDocument> { sortStage, groupStage });
            var elements = BsonIterator.Iterate(collection, pipelineDefinition, (BsonDocument e, IList<BsonGroupClass<Telemetry>> items) =>
            {
                if (items == null) items = new List<BsonGroupClass<Telemetry>>();
                items.Add(BsonSerializer.Deserialize<BsonGroupClass<Telemetry>>(e.ToBson()));
                return items;
            });

            return elements.ToDictionary(k => k.Id, v => v.Accumulator);
        }

        private class BsonGroupClass<T>
        {
            [JsonProperty("_id")]
            public string Id { get; set; }
            public T Accumulator { get; set; }
        }
    }
}
