using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class AlarmFilter : IMongoFilter<Device, Alarm>
    {
        public Alarm.Gravity Gravity { get; set; }
        public Alarm.Type AlarmType { get; set; }
        
        public IMongoQueryable<Alarm> FilterSource(IMongoQueryable<Device> source)
        {
            if(Gravity != Alarm.Gravity.All)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmGravity == Gravity));
            }

            if(AlarmType != Alarm.Type.All)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmType == AlarmType));
            }

            return source.SelectMany(e => e.Alarms);
        }
    }
}
