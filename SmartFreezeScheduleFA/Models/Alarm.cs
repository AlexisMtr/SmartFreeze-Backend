using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartFreezeScheduleFA.Models
{
    [BsonIgnoreExtraElements]
    public class Alarm
    {
        public enum Type
        {
            All = 0,
            FreezeWarning = 1,
            ThawWarning = 2,
            DeviceFailure = 3,
            CommuniationFailure = 4,
            BatteryWarning = 5
        }

        public enum Gravity
        {
            All = 0,
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
        public string SiteId { get; set; } //null si c'est une device alarme
        public bool IsActive { get; set; }
        public Type AlarmType { get; set; }
        public Gravity AlarmGravity { get; set; }
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
