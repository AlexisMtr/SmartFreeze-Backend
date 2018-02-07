using System;

namespace SmartFreeze.Dtos
{
    public class DeviceOverviewDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SiteName { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LastCommunication { get; set; }
        public int ActiveAlarmsCount { get; set; }
        public bool HasActiveAlarms { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
