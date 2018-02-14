using MongoDB.Driver;
using SmartFreezeFA.Configurations;
using SmartFreezeFA.Models;

namespace SmartFreezeFA.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IMongoCollection<Site> collection;

        public DeviceRepository(DbContext context)
        {
            this.collection = context.Database
                .GetCollection<Site>(nameof(Site));
        }

        public void AddAlarm(string deviceId, Alarm alarm)
        {
            var siteIdFilter = Builders<Site>.Filter.ElemMatch(e => e.Devices, d => d.Id == deviceId);
            var deviceSiteFilter = Builders<Site>.Filter.Eq("Devices.Id", deviceId);

            var filter = Builders<Site>.Filter.And(siteIdFilter, deviceSiteFilter);
            UpdateDefinition<Site> update = Builders<Site>.Update.Push("Devices.$.Alarms", alarm);

            collection.FindOneAndUpdate(filter, update);
        }
    }
}
