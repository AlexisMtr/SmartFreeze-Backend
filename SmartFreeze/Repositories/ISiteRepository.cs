using SmartFreeze.Filters;
using SmartFreeze.Models;
using System.Collections.Generic;


namespace SmartFreeze.Repositories
{
    public interface ISiteRepository
    {
        Site Get(string siteId);

        PaginatedItems<Site> GetAllPaginated(IMongoFilter<Site> filter, int rowsPerPage, int pageNumber);

        PaginatedItems<Site> GetPaginatedByIds(IEnumerable<string> ids, int rowsPerPage, int pageNumber);

        Site Create(Site site);

        bool Update(string siteId, Site site);

        bool Delete(string siteId);
    }
}
