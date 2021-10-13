using hotel_booking_dto;
using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Interfaces
{
    public interface IAdminService
    {
        Task<Response<List<TransactionResponseDto>>> GetManagerTransactionsAsync(string managerId, /*string filter, string searchQuery,*/ Paginator paging);
    }
}
