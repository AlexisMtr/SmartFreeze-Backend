using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartFreeze.Models
{
    [BsonIgnoreExtraElements]
    public class Telemetry
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime OccuredAt { get; set; }
        public double BatteryVoltage { get; set; }
        public double? Pressure { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
    }
}
