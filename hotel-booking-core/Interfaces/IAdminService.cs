using hotel_booking_dto;
using hotel_booking_dto.commons;
using hotel_booking_utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static hotel_booking_utilities.Pagination.Paginator;

namespace hotel_booking_core.Interfaces
{
    public interface IAdminService
    {
        Task<Response<PageResult<IEnumerable<TransactionResponseDto>>>> GetAllTransactions(TransactionFilter filter);
    }
}
