using MongoDB.Driver;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Models;

namespace SmartFreezeScheduleFA.Repositories
{
    public class TelemetryRepository
    {
        private readonly IMongoCollection<Telemetry> collection;

        public TelemetryRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Telemetry>("Temp_Telemetry");
        }
    }
}
