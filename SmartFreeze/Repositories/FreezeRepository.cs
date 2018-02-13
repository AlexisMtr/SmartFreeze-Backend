using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Helpers;
using SmartFreeze.Models;

namespace SmartFreeze.Repositories
{
    public class FreezeRepository : IFreezeRepository
    {
        private readonly IMongoCollection<Freeze> collection;

        public FreezeRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Freeze>(nameof(Freeze));
        }

        public IEnumerable<Freeze> GetByDevice(string deviceId, DateTime? from = null)
        {
            return collection.AsQueryable()
                .Where(e => e.DeviceId == deviceId);
        }

        public Dictionary<string, IEnumerable<Freeze>> GetByDevice(IEnumerable<string> devicesIds = null, DateTime? from = null)
        {
            PipelineDefinition<Freeze, BsonDocument> pipelineDefinition = PipelineDefinition<Freeze, BsonDocument>.Create(GetPipeline(devicesIds));

            return BsonIterator.Iterate(collection, pipelineDefinition, (BsonDocument e, Dictionary<string, IEnumerable<Freeze>> items) =>
            {
                if (items == null) items = new Dictionary<string, IEnumerable<Freeze>>();

                var item = BsonSerializer.Deserialize<BsonGroup<string, IEnumerable<Freeze>>>(e);
                items.Add(item.Id, item.Accumulator);

                return items;
            });
        }

        private IEnumerable<BsonDocument> GetPipeline(IEnumerable<string> ids = null)
        {
            List<BsonDocument> pipeline = new List<BsonDocument>();

            if(ids != null && ids.Any())
            {
                BsonDocument matchStage = new BsonDocument("$match", new BsonDocument
                {
                    { "DeviceId", new BsonDocument("$in", new BsonArray(ids)) }
                });
                pipeline.Add(matchStage);
            }

            BsonDocument groupStage = new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$DeviceId" },
                { "Accumulator", new BsonDocument("$push", "$$ROOT") }
            });
            pipeline.Add(groupStage);

            return pipeline;
        }

        private class BsonGroup<TKey, TValue>
        {
            [BsonElement("_id")]
            public TKey Id { get; set; } 
            public TValue Accumulator { get; set; }
        }
    }
}
