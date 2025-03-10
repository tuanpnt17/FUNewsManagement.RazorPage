using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList() { }

        public PaginatedList(List<T> items, int totalPages, int totalElements, int pageIndex)
        {
            AddRange(items);
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalElements = totalElements;
        }

        private PaginatedList(
            List<T> items,
            int count,
            int pageIndex,
            int pageSize,
            int totalElement
        )
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
            TotalElements = totalElement;
        }

        public int TotalPages { get; private set; }
        public int TotalElements { get; set; }

        public int PageIndex { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize
        )
        {
            var count = source.Count();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize, count);
        }
    }
}
