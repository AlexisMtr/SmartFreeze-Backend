﻿using MongoDB.Bson;
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

        public enum Gravity
        {
            Critical = 1,
            Serious = 2,
            Information = 3
        }

        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string SiteId { get; set; }
        public bool IsActive { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public Type AlarmType { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public Gravity AlarmGravity { get; set; }
        public DateTime OccuredAt { get; set; }
        public DateTime LastUpdate { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
