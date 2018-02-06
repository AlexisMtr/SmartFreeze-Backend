using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Collections.Generic;
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


        public void addAlarm(String idDevice, Alarm alarm)
        {
            var site = this.Get(idDevice);
            IEnumerable<Alarm> Alarms = site.Alarms;
            Alarms.ToList().Add(alarm);
            UpdateDefinition<Device> update = Builders<Device>.Update
                  .Set(p => p.Alarms, Alarms);
            this.collection.UpdateOne(Builders<Site>.Filter.Eq(p => p.Id, idDevice), update);

        }
    }
}
