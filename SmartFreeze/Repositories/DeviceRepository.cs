using MongoDB.Driver;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class DeviceRepository
    {
        private readonly SmartFreezeContext context;
        private readonly IMongoCollection<Device> collection;

        public DeviceRepository(SmartFreezeContext context)
        {
            this.context = context;
            this.collection = context.Database
                .GetCollection<Device>(nameof(Device));
        }

        public Device Get(string deviceId)
        {
            return collection.AsQueryable().FirstOrDefault(e => true);
        }

        public PaginatedItems<Device> GetAllPaginated(DeviceFilter filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable().Where(e => true)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }

        public object Register(object device)
        {
            return null;
        }
    }
}
