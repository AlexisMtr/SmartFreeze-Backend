using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class AlarmFilter : IMongoFilter<Device, Alarm>, IMongoFilter<Site, Alarm>
    {
        public Alarm.Gravity Gravity { get; set; }
        
        public IMongoQueryable<Alarm> FilterSource(IMongoQueryable<Device> source)
        {
            if (Gravity == Alarm.Gravity.All) return source.SelectMany(e => e.Alarms);

            return source.SelectMany(e => e.Alarms.Where(a => a.AlarmGravity == Gravity));
        }

        public IMongoQueryable<Alarm> FilterSource(IMongoQueryable<Site> source)
        {
            if (Gravity == Alarm.Gravity.All) return source.SelectMany(e => e.Alarms);

            return source.SelectMany(e => e.Alarms.Where(a => a.AlarmGravity == Gravity));
        }
    }
}
