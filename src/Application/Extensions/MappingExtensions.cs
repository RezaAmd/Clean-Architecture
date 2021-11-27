using Application.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Extentions
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TOrigin>> PaginatedListAsync<TOrigin>(this IQueryable<TOrigin> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = new())
            => PaginatedList<TOrigin>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);
    }
}