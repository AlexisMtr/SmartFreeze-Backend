using System.Collections.Generic;

namespace SmartFreeze.Models
{
    public class PaginatedItems<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
