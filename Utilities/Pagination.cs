using Microsoft.EntityFrameworkCore;
using UserManagementApp.Dtos;

namespace UserManagementApp.Utilities;

public static class Pagination
{
    public static async Task<PaginatorDto<IEnumerable<TSource>>> PaginateAsync<TSource>(
        this IQueryable<TSource> queryable, PaginationFilter? paginationFilter = null)
        where TSource : class
    {
        var count = queryable.Count();
        paginationFilter ??= new PaginationFilter();

        var numberOfPages = (int)Math.Ceiling((double)count / paginationFilter.PageSize);

        var pageItems = await queryable.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync() ?? [];

        return new PaginatorDto<IEnumerable<TSource>>(pageItems, paginationFilter.PageSize,
            paginationFilter.PageNumber, numberOfPages, count);
    }
}