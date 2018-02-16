using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WeatherLibrary.Abstraction;

namespace SmartFreezeScheduleFA.Models
{
    [BsonIgnoreExtraElements]
    public class Device : IStationPosition
    {
        public Device()
        {
            this._id = ObjectId.GenerateNewId();
        }

        [BsonId]
        private ObjectId _id { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string SiteId { get; set; }
        public string Zone { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime LastCommunication { get; set; }
        public Position Position { get; set; }

        public double Latitude { get => Position.Latitude; set => throw new NotImplementedException(); }
        public double Longitude { get => Position.Longitude; set => throw new NotImplementedException(); }
        public double Altitude { get => Position.Altitude; set => throw new NotImplementedException(); }
    }
}
