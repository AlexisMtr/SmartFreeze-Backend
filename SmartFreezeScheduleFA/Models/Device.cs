using System;
using System.Collections.Generic;
using WeatherLibrary.Abstraction;

namespace SmartFreezeScheduleFA.Models
{
    public class Device : IStationPosition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string Zone { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        public DateTime LastCommunication { get; set; }
        public Position Position { get; set; }

        public double Latitude { get => Position.Latitude; set => throw new NotImplementedException(); }
        public double Longitude { get => Position.Longitude; set => throw new NotImplementedException(); }
        public double Altitude { get => Position.Altitude; set => throw new NotImplementedException(); }
    }
}
