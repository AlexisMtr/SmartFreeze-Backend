using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SmartFreezeFA.Models
{
    [BsonIgnoreExtraElements]
    public class Device
    {
        public Device()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime LastCommunication { get; set; }
    }
}
