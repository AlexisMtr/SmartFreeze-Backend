using MongoDB.Driver.Linq;

namespace SmartFreezeScheduleFA.Filters
{
    public interface IMongoFilter<TSource, TResult>
    {
        IMongoQueryable<TResult> FilterSource(IMongoQueryable<TSource> source);
    }

    public interface IMongoFilter<T> : IMongoFilter<T, T> { }
}
