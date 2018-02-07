using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFreeze.Sorters
{
    public class TelemetrySorter : IMongoSorter<Telemetry>
    {
        public IOrderedMongoQueryable<Telemetry> SortSource(IMongoQueryable<Telemetry> source)
        {
            return source.OrderBy(e => e.OccuredAt);
        }
    }
}
