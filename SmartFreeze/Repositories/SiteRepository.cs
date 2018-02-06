using MongoDB.Driver;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public bool Create(Site site)
        {
            var result = collection.InsertOneAsync(site);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool Update(Site site)
        {      UpdateDefinition<Site> update = Builders<Site>.Update.Set(p => p.Name, site.Name)
                   .Set(p => p.SurfaceArea, site.SurfaceArea)
                   .Set(p => p.ImageUri, site.ImageUri)
                   .Set(p => p.Description, site.Description)
                   .Set(p => p.Zones, site.Zones);
            var result = this.collection.UpdateOne(Builders<Site>.Filter.Eq(p => p.Id, site.Id), update);
            if (result != null)
            {
                return true;
            }
            return false;
        }


        public bool Delete(String idSite)
        {
            var result = collection.DeleteOneAsync(Builders<Site>.Filter.Eq("Id", idSite));
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public void addAlarm(String idSite, Alarm alarm)
        {
            var site = this.Get(idSite);
            IEnumerable<Alarm> Alarms = site.Alarms;
            Alarms.ToList().Add(alarm);
            UpdateDefinition<Site> update = Builders<Site>.Update
                  .Set(p => p.Alarms, Alarms);
           this.collection.UpdateOne(Builders<Site>.Filter.Eq(p => p.Id, idSite), update);

        }
    } 
}
