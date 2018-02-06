using MongoDB.Driver.Linq;
using SmartFreezeScheduleFA.Filters;
using System;
using System.Linq;

namespace SmartFreezeScheduleFA.Extensions
{
    public static class MongoQueryExtension
    {
        public static IMongoQueryable<TResult> Filter<TSource, TResult>(this IMongoQueryable<TSource> source, IMongoFilter<TSource, TResult> filter)
        {
            return filter.FilterSource(source);
        }
    }
}
