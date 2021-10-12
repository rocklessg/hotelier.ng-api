using AutoMapper;
using hotel_booking_dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_utilities.Pagination
{
    public static partial class Paginator
    {

        public static async Task<PageResult<IEnumerable<TDestination>>> PaginationAsync<TSource, TDestination>(this IQueryable<TSource> querable, int pageSize, int pageNumber, IMapper mapper)
            where TSource : class
            where TDestination : class
        {
            var count = querable.Count();
            var pageResult = new PageResult<IEnumerable<TDestination>>
            {
                PageSize = (pageSize > 10) ? 10 : pageSize,
                CurrentPage = pageNumber,
                PreviousPage = pageNumber - 1
            };
            pageResult.NumberOfPages = count % pageResult.PageSize != 0
                ? count / pageResult.PageSize + 1
                : count / pageResult.PageSize;
            var sourceList = await querable.Skip(pageResult.CurrentPage - 1).Take(pageResult.PageSize).ToListAsync();
            var destinationList = mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);
            pageResult.PageItems = destinationList;
            return pageResult;
        }

        
    }
}
