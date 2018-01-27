using SmartFreeze.Models;
using System;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class TelemetryFilter : IFilter<Telemetry>
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public IQueryable<Telemetry> FilterSource(IQueryable<Telemetry> source)
        {
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
