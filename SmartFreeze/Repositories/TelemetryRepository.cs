using MongoDB.Driver;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class TelemetryRepository
    {
        private readonly SmartFreezeContext context;
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(SmartFreezeContext context)
        {
            this.context = context;
            this.collection = context.Database
                .GetCollection<Telemetry>(nameof(Telemetry));
        }


        public PaginatedItems<Telemetry> GetByDevice(string deviceId, DateTime? from = null, DateTime? to = null, int rowsPerPage = 20, int pageNumber = 1)
        {
            var filter = new TelemetryFilter
            {
                Start = from,
                End = to
            };

            return collection.AsQueryable().Where(e => true)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }
    }
}
