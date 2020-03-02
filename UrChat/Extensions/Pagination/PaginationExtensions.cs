using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrChat.ViewModels;

namespace UrChat.Extensions.Pagination
{
    public static class PaginationExtensions
    {
        public static async Task<PaginationViewModel<Tm>> PaginateAsync<Tq, Tm>(
            this IQueryable<Tq> query,
            PaginationParams paginationParams,
            Func<Tq, Tm> map
        )
        {
            int totalPages = (int) Math.Ceiling((float) await query.CountAsync() / (float) paginationParams.PageSize);

            var pageItems = (await query
                    .Skip(paginationParams.Page * paginationParams.PageSize)
                    .Take(paginationParams.PageSize)
                    .ToListAsync())
                .Select(map);

            return new PaginationViewModel<Tm>
            {
                TotalPages = totalPages,
                Page = pageItems
            };
        }
    }
}