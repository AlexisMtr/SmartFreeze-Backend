using MongoDB.Driver;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFreezeFA.Repositories
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Telemetry>(nameof(Telemetry));
        }

        public Telemetry GetLatest(string deviceId)
        {
            return collection.AsQueryable()
                .Where(e => e.DeviceId == deviceId)
                .OrderByDescending(e => e.OccuredAt)
                .FirstOrDefault();
        }

        public void InsertTelemetries(IEnumerable<Telemetry> telemetries)
        {
            collection.InsertMany(telemetries);
        }
    }
}
