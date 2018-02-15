using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartFreeze.Models
{
    [BsonIgnoreExtraElements]
    public class Freeze
    {
        [BsonId]
        private ObjectId _id { get; set; }

        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public int TrustIndication { get; set; }
    }
}
