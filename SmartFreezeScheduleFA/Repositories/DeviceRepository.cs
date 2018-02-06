using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreezeScheduleFA.Extensions;
using SmartFreezeScheduleFA.Configurations;
using SmartFreezeScheduleFA.Filters;
using SmartFreezeScheduleFA.Models;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class DeviceRepository
    {
        private readonly IMongoCollection<Device> collection;

        public DeviceRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Device>(nameof(Device));
        }


    }
}
