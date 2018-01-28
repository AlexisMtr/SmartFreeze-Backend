using MongoDB.Driver.Linq;

namespace SmartFreeze.Sorters
{
    public interface IMongoSorter<T>
    {
        IOrderedMongoQueryable<T> SortSource(IMongoQueryable<T> source);
    }
}
