using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_utilities;
using hotel_booking_utilities.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Interfaces
{
    public interface IAdminService
    {
        Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetManagerTransactionsAsync(string managerId, Paging paging, TransactionFilter filter);
    }
}
