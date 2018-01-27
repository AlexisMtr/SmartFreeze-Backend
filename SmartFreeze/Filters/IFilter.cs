using System.Linq;

namespace SmartFreeze.Filters
{
    public interface IFilter<TSource, TResult>
    {
        IQueryable<TResult> FilterSource(IQueryable<TSource> source);
    }

    public interface IFilter<T> : IFilter<T, T> { }
}
