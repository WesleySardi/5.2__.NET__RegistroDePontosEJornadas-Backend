using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ProjetoBMA.Utils
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
            this IQueryable<TSource> query,
            int page,
            int pageSize,
            Func<TSource, TResult> selector,
            CancellationToken ct = default)
        {
            var total = await query.CountAsync(ct);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<TResult>
            {
                Items = items.Select(selector).ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public static string ToJson<T>(this T item)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(item, options);
        }
    }

}
