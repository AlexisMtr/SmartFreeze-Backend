﻿using MongoDB.Driver;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Sorters;
using System;

namespace SmartFreeze.Repositories
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Telemetry>(nameof(Telemetry));
        }


        public PaginatedItems<Telemetry> GetByDevice(string deviceId, int rowsPerPage, int pageNumber, DateTime? from = null, DateTime? to = null)
        {
            var filter = new TelemetryFilter
            {
                Start = from,
                End = to,
                DeviceId = deviceId
            };

            return collection.AsQueryable()
                .Filter(filter)
                .Sort(new TelemetrySorter())
                .Paginate(rowsPerPage, pageNumber);
        }
    }
}
