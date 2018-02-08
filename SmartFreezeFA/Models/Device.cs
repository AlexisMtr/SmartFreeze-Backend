using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SmartFreezeFA.Models
{
    [BsonIgnoreExtraElements]
    public class Device
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string Id { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        public DateTime LastCommunication { get; set; }
    }
}
