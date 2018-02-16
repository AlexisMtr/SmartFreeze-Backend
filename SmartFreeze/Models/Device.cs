using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SmartFreeze.Models
{
    [BsonIgnoreExtraElements]
    public class Device
    {
        [BsonId]
        private ObjectId _id { get; set; }

        public Device()
        {
            _id = ObjectId.GenerateNewId();
        }


        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Zone { get; set; }
        public string SiteId { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime LastCommunication { get; set; }
        public Position Position { get; set; }
    }
}
