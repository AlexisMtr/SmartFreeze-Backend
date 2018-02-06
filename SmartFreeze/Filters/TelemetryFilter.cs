using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class TelemetryFilter : IMongoFilter<Telemetry>
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string DeviceId { get; set; }
        
        public IMongoQueryable<Telemetry> FilterSource(IMongoQueryable<Telemetry> source)
        {
            if(!string.IsNullOrEmpty(DeviceId))
            {
                source = source.Where(e => e.DeviceId == DeviceId);
            }
            if (Start.HasValue)
            {
                source = source.Where(e => e.OccuredAt >= Start.Value);
            }
            if (End.HasValue)
            {
                source = source.Where(e => e.OccuredAt <= End.Value);
            }

            return source;
        }
    }
}
