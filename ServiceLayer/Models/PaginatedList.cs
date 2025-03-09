﻿namespace ServiceLayer.Models
{
    public class PaginatedList<T> : List<T>, IEnumerable<T>
    {
        public PaginatedList() { }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
            TotalElements = this.Count();
        }

        public int TotalPages { get; private set; }
        public int TotalElements { get; set; }

        public int PageIndex { get; private set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
