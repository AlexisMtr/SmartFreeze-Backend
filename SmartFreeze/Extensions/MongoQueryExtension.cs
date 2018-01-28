using MongoDB.Driver.Linq;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Sorters;
using System;
using System.Linq;

namespace SmartFreeze.Extensions
{
    public static class MongoQueryExtension
    {
        public static IMongoQueryable<TResult> Filter<TSource, TResult>(this IMongoQueryable<TSource> source, IMongoFilter<TSource, TResult> filter)
        {
            return filter.FilterSource(source);
        }

        public static IOrderedMongoQueryable<T> Sort<T>(this IMongoQueryable<T> source, IMongoSorter<T> sorter)
        {
            return sorter.SortSource(source);
        }

        public static PaginatedItems<T> Paginate<T>(this IMongoQueryable<T> source, int rowsPerPage = 20, int pageNumber = 1)
        {
            var skip = Math.Max(0, pageNumber - 1) * rowsPerPage;
            var totalCount = source.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / rowsPerPage);

            return new PaginatedItems<T>
            {
                PageCount = pageCount,
                TotalItemsCount = totalCount,
                Items = source.Skip(skip).Take(rowsPerPage)
            };
        }
    }
}
