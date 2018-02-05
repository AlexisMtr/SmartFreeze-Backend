using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SmartFreeze.Filters
{
    public class AlarmFilter : IMongoFilter<Device, Alarm>, IMongoFilter<Site, Alarm>
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
            //Expression expression = null;

            //if (Gravity != Alarm.Gravity.All)
            //{
            //    Expression<Func<Alarm, bool>> gravityCheck = e => e.AlarmGravity == Gravity;
            //    Expression.AndAlso(expression, gravityCheck);
            //}
            //if (AlarmType != Alarm.Type.All)
            //{
            //    Expression<Func<Alarm, bool>> typeCheck = e => e.AlarmType == AlarmType;
            //    Expression.AndAlso(expression, typeCheck);
            //}

            //return source.SelectMany(Expression.Lambda(expression, source));
            return source.SelectMany(e => e.Alarms);
        }

        public IMongoQueryable<Alarm> FilterSource(IMongoQueryable<Site> source)
        {
            if (Gravity != Alarm.Gravity.All)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmGravity == Gravity));
            }

            if (AlarmType != Alarm.Type.All)
            {
                source = source.Where(e => e.Alarms.Any(a => a.AlarmType == AlarmType));
            }

            return source.SelectMany(e => e.Alarms);
        }
    }
}
