using MongoDB.Driver;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class SiteRepository
    {
        private readonly IMongoCollection<Site> collection;

        public SiteRepository(SmartFreezeContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public Site Get(string siteId)
        {
            return collection.AsQueryable().FirstOrDefault(e => e.Id.Equals(siteId));
        }

        public PaginatedItems<Site> GetAllPaginated(IMongoFilter<Site> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }
    }
}
