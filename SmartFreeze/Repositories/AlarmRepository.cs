using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;

namespace SmartFreeze.Repositories
{
    public class AlarmRepository
    {
        private readonly IMongoCollection<Site> collection;

        public AlarmRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public PaginatedItems<Alarm> GetBySite(string siteId, IMongoFilter<Site, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .Where(e => e.Id == siteId)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }

        public PaginatedItems<Alarm> GetByDevice(string deviceId, IMongoFilter<Device, Alarm> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .SelectMany(e => e.Devices)
                .Where(e => e.Id == deviceId)
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }
    }
}
