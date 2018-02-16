using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WeatherLibrary.Abstraction;

namespace SmartFreezeScheduleFA.Models
{
    [BsonIgnoreExtraElements]
    public class Telemetry : IWeather
    {
        public Telemetry()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime OccuredAt { get; set; }
        public double BatteryVoltage { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }

        public double WindSpeed { get => 0; set => throw new NotImplementedException(); }
        public DateTime Date { get => OccuredAt; set => throw new NotImplementedException(); }
    }
}
