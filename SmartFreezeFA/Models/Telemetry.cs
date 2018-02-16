using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WeatherLibrary.Abstraction;

namespace SmartFreezeFA.Models
{
    public class Telemetry : IWeather
    {
        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime OccuredAt { get; set; }
        public double? BatteryVoltage { get; set; }
        public double Pressure { get => PressureValue ?? 0; set => PressureValue = value; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }

        [BsonIgnore]
        public double WindSpeed { get => 0; set => throw new NotImplementedException(); }
        [BsonIgnore]
        public DateTime Date { get => OccuredAt; set => throw new NotImplementedException(); }

        [BsonIgnore]
        public double? PressureValue { get; set; }
    }
}
