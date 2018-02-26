using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartFreezeFA.Models
{
    [BsonIgnoreExtraElements]
    public class Alarm
    {
        public enum Type
        {
            FreezeWarning = 1,
            ThawWarning = 2,
            DeviceFailure = 3,
            CommunicationError = 4
        }

        public enum AlarmSubtype
        {
            Temperature = 1,
            Humidity = 2,
            Battery = 3,
            Freeze = 4
        }

        public enum Gravity
        {
            Critical = 1,
            Serious = 2,
            Information = 3
        }

        public Alarm()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string SiteId { get; set; }
        public bool IsActive { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public Type AlarmType { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public Gravity AlarmGravity { get; set; }
        // only for this FA. Allow to make a difference between deviceFailure alarms
        [BsonRepresentation(BsonType.Int32)]
        public AlarmSubtype Subtype { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime OccuredAt { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime LastUpdate { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime? Start { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime? End { get; set; }
    }
}
