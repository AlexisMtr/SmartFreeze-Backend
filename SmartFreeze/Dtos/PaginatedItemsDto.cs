using System.Collections.Generic;

namespace SmartFreeze.Dtos
{
    public class PaginatedItemsDto<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
