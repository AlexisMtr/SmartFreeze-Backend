using System;

namespace SmartFreeze.Dtos
{
    public class TelemetryDto
    {
        public string DeviceId { get; set; }
        public DateTime OccuredAt { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
    }
}
