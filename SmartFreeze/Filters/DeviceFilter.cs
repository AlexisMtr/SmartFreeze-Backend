using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class DeviceFilter : IFilter<Device>
    {
        public string Site { get; set; }
        public bool? Warning { get; set; }
        public bool? Failure { get; set; }
        public bool? Favorite { get; set; }
        public Alarm.Gravity Gravity { get; set; }

        public IQueryable<Device> FilterSource(IQueryable<Device> source)
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
                source.Where(e => e.Alarms.Any(a => a.AlarmGravity == Gravity));
            }

            return source;
        }
    }
}
