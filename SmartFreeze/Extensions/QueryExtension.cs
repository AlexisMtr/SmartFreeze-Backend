using MongoDB.Driver.Linq;
using SmartFreeze.Filters;
using SmartFreeze.Models;
using System;
using System.Linq;

namespace SmartFreeze.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<TResult> Filter<TSource, TResult>(this IQueryable<TSource> source, IFilter<TSource, TResult> filter)
        {
            return filter.FilterSource(source);
        }

        public static PaginatedItems<T> Paginate<T>(this IQueryable<T> source, int rowsPerPage = 20, int pageNumber = 1)
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
