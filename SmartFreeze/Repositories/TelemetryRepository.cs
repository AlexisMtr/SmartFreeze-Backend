using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;

namespace SmartFreeze.Repositories
{
    public class TelemetryRepository
    {
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(SmartFreezeContext context)
        {
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
