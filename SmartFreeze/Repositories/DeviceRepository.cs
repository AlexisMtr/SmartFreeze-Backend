using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class DeviceRepository
    {
        private readonly IMongoCollection<Site> collection;

        public DeviceRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public Device Get(string deviceId)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .FirstOrDefault(e => e.Id.Equals(deviceId));
        }

        public PaginatedItems<Device> GetAllPaginated(IMongoFilter<Device> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }

        public object Register(object device)
        {
            return null;
        }
    }
}
