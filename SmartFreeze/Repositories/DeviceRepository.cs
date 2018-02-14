using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartFreeze.Context;
using SmartFreeze.Extensions;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Linq;

namespace SmartFreeze.Repositories
{
    public class DeviceRepository : IDeviceRepository
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

        public PaginatedItems<Device> GetAllPaginated(IMongoFilter<Site, Device> filter, int rowsPerPage, int pageNumber)
        {
            return collection.AsQueryable()
                .Filter(filter)
                .Paginate(rowsPerPage, pageNumber);
        }

        public Device Create(Device device, string siteId)
        {
            device.Id = "device"+DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            UpdateDefinition<Site> update = Builders<Site>.Update.Push(e => e.Devices, device);
            this.collection.UpdateOne(Builders<Site>.Filter.Eq(p => p.Id, siteId), update);
            return device;
        }

        public bool Update(Device device)
        {
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == device.Id);
            var result = this.collection.FindOneAndUpdate(filter,
               Builders<Site>.Update.Set("Devices.$.Name", device.Name)
               .Set("Devices.$.IsFavorite", device.IsFavorite)
               .Set("Devices.$.Zone", device.Zone)
               .Set("Devices.$.SiteId", device.SiteId)
               .Set("Devices.$.Position", device.Position));

            return result != null;
        }


        public bool Delete(string deviceId)
        {
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var update = Builders<Site>.Update.PullFilter(s => s.Devices, d => d.Id == deviceId);
            var result = collection.FindOneAndUpdate(filter, update);

            return result != null;
        }


        public void AddAlarm(string deviceId, Alarm alarm)
        {
            var siteIdFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var filter = Builders<Site>.Filter.And(siteIdFilter);
            UpdateDefinition<Site> update = Builders<Site>.Update.Push("Devices.$.Alarms", alarm);

            collection.FindOneAndUpdate(filter, update);

        }


        public bool Managefavorite(string deviceId, bool isFavorite)
        {
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var result = this.collection.FindOneAndUpdate(filter,
               Builders<Site>.Update.Set("Devices.$.IsFavorite", isFavorite));

            return result != null;

        }


        public bool UpdateLastCommunication(string deviceId, DateTime lastDate)
        {
            var filter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var result = this.collection.FindOneAndUpdate(filter,
               Builders<Site>.Update.Set("Devices.$.LastCommunication", lastDate));
            return result != null;

        }


    }
}
