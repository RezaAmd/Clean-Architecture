using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PaginatedList<T>
    {
        public List<T> items { get; set; }
        public int pageIndex { get; }
        public int totalPages { get; }
        public int totalCount { get; }

        public PaginatedList()
        {
        }

        public PaginatedList(List<T> Items, int Count, int PageIndex, int PageSize)
        {
            pageIndex = PageIndex;
            totalPages = (int)Math.Ceiling(Count / (double)PageSize);
            totalCount = Count;
            items = Items;
        }

        public bool hasPreviousPage => pageIndex > 1;

        public bool hasNextPage => pageIndex < totalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            pageSize = pageSize <= 0 ? 5 : pageSize;

            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}