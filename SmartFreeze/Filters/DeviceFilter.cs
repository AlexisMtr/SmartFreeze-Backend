using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class DeviceFilter : IMongoFilter<Device>, IMongoFilter<Site, Device>
    {
        public ApplicationContext Context { get; set; }
        public string SiteId { get; set; }
        public bool? Warning { get; set; }
        public bool? Failure { get; set; }
        public bool? Favorite { get; set; }
        public Alarm.Gravity Gravity { get; set; }
        
        public IMongoQueryable<Device> FilterSource(IMongoQueryable<Device> source)
        {
            if (Favorite.HasValue)
            {
                source = source.Where(e => e.IsFavorite == Favorite.Value);
            }
            if (Warning.HasValue)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmType == Alarm.Type.FreezeWarning && a.IsActive == Warning.Value));
            }
            if (Failure.HasValue)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmType == Alarm.Type.DeviceFailure && a.IsActive == Failure.Value));
            }

            if (Gravity != Alarm.Gravity.All)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmGravity == Gravity));
            }

            return source;
        }

        public IMongoQueryable<Device> FilterSource(IMongoQueryable<Site> source)
        {
            IMongoQueryable<Device> devicesSource;

            source = source.Where(e => e.SiteType == Context);

            if(string.IsNullOrEmpty(SiteId))
            {
                devicesSource = source.SelectMany(e => e.Devices);
            }
            else
            {
                devicesSource = source
                       .Where(e => e.Id.Equals(SiteId))
                       .SelectMany(e => e.Devices);
            }

            return FilterSource(devicesSource);
        }
    }
}
