using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SmartFreeze.Repositories
{
    public class SiteRepository : ISiteRepository
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

        public PaginatedItems<Site> GetPaginatedByIds(IEnumerable<string> ids, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .Where(e => ids.Contains(e.Id))
                .Paginate(rowsPerPage, pageNumber);
        }

        public Site Create(Site site)
        {
                if (!String.IsNullOrEmpty(site.Name))
            {
                site.Name = site.Name.Normalize(NormalizationForm.FormD);
                var chars = site.Name.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                site.Name =  new string(chars).Normalize(NormalizationForm.FormC);
            }

            site.Id = site.Name.Replace(" ","")+ DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            collection.InsertOne(site);
            return site;
        }

        public bool Update(string siteId, Site site)
        {
            UpdateDefinition<Site> update = Builders<Site>.Update.Set(p => p.Name, site.Name)
                .Set(p => p.SurfaceArea, site.SurfaceArea)
                .Set(p => p.Image, site.Image)
                .Set(p => p.Description, site.Description)
                .Set(p => p.Zones, site.Zones); 
            var result = this.collection.UpdateOne(Builders<Site>.Filter.Eq(p => p.Id, siteId), update);

            return result.ModifiedCount > 0;
        }

        public bool Delete(string siteId)
        {
            var result = collection.DeleteOne(Builders<Site>.Filter.Eq("Id", siteId));

            return result.DeletedCount > 0;
        }
    }
}
