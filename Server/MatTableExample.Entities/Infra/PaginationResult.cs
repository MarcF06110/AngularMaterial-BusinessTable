using System.Collections.Generic;

namespace MatTableExample.Entities.Infra
{
    public class PaginationResult<T> where T : class
    {
        public PaginationResult(int count, IEnumerable<T> items)
        {
            Count = count;
            Items = items;
        }
        public IEnumerable<T> Items { get; set; }
        public int Count { get; set; }
    }
}