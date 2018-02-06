using MongoDB.Driver.Linq;
using SmartFreezeScheduleFA;
using SmartFreezeScheduleFA.Models;
using System.Linq;

namespace SmartFreezeScheduleFA.Filters
{
    public class DeviceFilter : IMongoFilter<Device>, IMongoFilter<Site, Device>
    {
        public string Site { get; set; }
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
            var devicesSource = source
                    .Where(e => e.Name.Equals(Site))
                    .SelectMany(e => e.Devices);

            return FilterSource(devicesSource);
        }
    }
}
